using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Gma.DataStructures;
using Advexp.DynamicSettings.Plugin;

using PluginsDictionary = System.Collections.Generic.Dictionary<System.Type/*plugin interface type*/, object/*plugin object*/>;
using ClassMetadataDictionary = System.Collections.Generic.Dictionary<string/*metadata name*/, object/*metadata object*/>;
using SettingsMetadataDictionary = System.Collections.Generic.Dictionary<string/*setting name*/, System.Collections.Generic.Dictionary<string/*metadata name*/, object/*metadata*/>>;

namespace Advexp
{
    static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue ret;
            if (!dictionary.TryGetValue(key, out ret))
            {
                ret = new TValue();
                dictionary.Add(key, ret);
            }
            return ret;
        }
    }

    public partial class SettingsT<ST> : IPluginHolder, IPluginContext where ST : new()
    {
        static ISettingsSerializer s_foregroundSerializer = null;
        static object s_instance = null;
        static readonly object s_lockObject = new object();
        static HashSet<Type/*serializer type*/> s_InitializedSerializers = new HashSet<Type>();

        PluginsDictionary m_plugins = new PluginsDictionary();

        SettingsMetadataDictionary m_SettingsMetadata = new SettingsMetadataDictionary();
        ClassMetadataDictionary m_ClassMetadata = new ClassMetadataDictionary();

        //------------------------------------------------------------------------------
        static void LoadAllDynamicSettings(object settings)
        {
            IPluginHolder ph = settings as IPluginHolder;
            if (ph == null)
            {
                return;
            }

            var plugins = InternalConfiguration.Plugins;

            foreach (var kv in plugins)
            {
                var plugin = ph.GetPlugin(kv.Key);

                var dsp = plugin as IDynamicSettingsPlugin;
                if (dsp == null)
                {
                    continue;
                }

                dsp.LoadSettings();
            }
        }

        //------------------------------------------------------------------------------
        static void SaveAllDynamicSettings(object settings)
        {
            IPluginHolder ph = settings as IPluginHolder;
            if (ph == null)
            {
                return;
            }

            var plugins = InternalConfiguration.Plugins;

            foreach (var kv in plugins)
            {
                var plugin = ph.GetPlugin(kv.Key);

                var dsp = plugin as IDynamicSettingsPlugin;
                if (dsp == null)
                {
                    continue;
                }

                dsp.SaveSettings();
            }
        }

        //------------------------------------------------------------------------------
        static void DeleteAllDynamicSettings(object settings)
        {
            IPluginHolder ph = settings as IPluginHolder;
            if (ph == null)
            {
                return;
            }

            var plugins = InternalConfiguration.Plugins;

            foreach (var kv in plugins)
            {
                var plugin = ph.GetPlugin(kv.Key);

                var dsp = plugin as IDynamicSettingsPlugin;
                if (dsp == null)
                {
                    continue;
                }

                dsp.DeleteSettings();
            }
        }

        //------------------------------------------------------------------------------
        static bool ContainsObjectSettingImpl<T>(object settings, Expression<Func<ST, T>> value)
        {
            SettingBaseAttribute settingBaseAttribute = GetSettingBaseAttribute(value);

            MemberExpression me = (MemberExpression)value.Body;
            MemberInfo mi = me.Member;

            var serializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>();
            var serializer = SettingsHelper.GetSerializerForMemberInfo(settings, GetForegroundSerializer(), mi, ref serializersCache);

            var serializerSettingName = SettingNameFormatInfo.GetFullSettingName(
                settings.GetType(),
                mi,
                settingBaseAttribute,
                serializer as ISettingsSerializerWishes,
                SettingNameMode.Static);

            bool secure = false;

            var settingAttribute = settingBaseAttribute as SettingAttribute;
            if (settingAttribute != null)
            {
                secure = settingAttribute.Secure;
            }

            var contains = serializer.Contains(serializerSettingName, secure, settingBaseAttribute);

            return contains;
        }

        //------------------------------------------------------------------------------
        static void ProcessClass(object settings, SettingsEnumerationMode mode)
        {
            lock (s_lockObject)
            {
                InitializeSettings();

                var members = settings.GetType().GetTypeInfo().DeclaredMembers;

                var settingsHelperData = new SettingsHelperData()
                {
                    SerializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool/*skip external serializer*/>>(),
                    SerializersToSync = new OrderedSet<ISettingsSerializer>(),
                };

                var serializersInClass = GetSerializersInClass(settings, ref settingsHelperData.SerializersCache);
                foreach (var serializerInClass in serializersInClass)
                {
                    DoSerializerInitialization(serializerInClass);
                }

                foreach(var serializerInClass in serializersInClass)
                {
                    DoSerializerStartAction(serializerInClass, mode);
                }

                var settingsObjectInfo = new SettingsObjectInfo();
                var inSettingInfo = new SettingInfo();

                foreach (var member in members)
                {
                    settingsObjectInfo.Settings = settings;
                    settingsObjectInfo.ForegroundSerializer = GetForegroundSerializer();
                    settingsObjectInfo.MemberInfo = member;

                    SettingInfo outSettingInfo;

                    SettingsHelper.ProcessSetting(settingsObjectInfo,
                                                  inSettingInfo,
                                                  mode,
                                                  settingsHelperData,
                                                  out outSettingInfo,
                                                  null);
                }

                SettingsHelper.SynchronizeSerializers(settingsHelperData.SerializersToSync);

                foreach (var serializerInClass in serializersInClass)
                {
                    DoSerializerEndAction(serializerInClass, mode);
                }
            }
        }

        //------------------------------------------------------------------------------
        static void ProcessExpression<T>(object settings, Expression<Func<ST, T>> value, SettingsEnumerationMode mode)
        {
            lock (s_lockObject)
            {
                InitializeSettings();

                MemberExpression me = value.Body as MemberExpression;
                if (me != null)
                {
                    var settingsHelperData = new SettingsHelperData()
                    {
                        SerializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool/*skip external serializer*/>>(),
                        SerializersToSync = new OrderedSet<ISettingsSerializer>(),
                    };

                    var serializer = SettingsHelper.GetSerializerForMemberInfo(settings, GetForegroundSerializer(), me.Member, ref settingsHelperData.SerializersCache);

                    DoSerializerInitialization(serializer);
                    DoSerializerStartAction(serializer, mode);

                    var inSettingInfo = new SettingInfo();

                    var settingsObjectInfo = new SettingsObjectInfo()
                    {
                        Settings = settings,
                        ForegroundSerializer = GetForegroundSerializer(),
                        MemberInfo = me.Member,
                    };

                    SettingInfo outSettingInfo;

                    SettingsHelper.ProcessSetting(settingsObjectInfo,
                                                  inSettingInfo,
                                                  mode,
                                                  settingsHelperData,
                                                  out outSettingInfo,
                                                  null);

                    SettingsHelper.SynchronizeSerializers(settingsHelperData.SerializersToSync);

                    DoSerializerEndAction(serializer, mode);
                }
            }
        }

        //------------------------------------------------------------------------------
        static void DoSerializerInitialization(ISettingsSerializer serializer)
        {
            var initializer = serializer as ISettingsSerializerInitializer;
            if (initializer == null)
            {
                return;
            }

            var serializerType = serializer.GetType();
            var contains = s_InitializedSerializers.Contains(serializerType);
            if (contains == false)
            {
                initializer.Initialize();
                s_InitializedSerializers.Add(serializerType);
            }
        }

        //------------------------------------------------------------------------------
        static void DoSerializerStartAction(ISettingsSerializer serializer, SettingsEnumerationMode mode)
        {
            var generalNotificationSink = serializer as ISerializerNotificationSink;
            if (generalNotificationSink != null)
            {
                generalNotificationSink.OnStartSerializerAction();

                switch (mode)
                {
                    case SettingsEnumerationMode.Load:
                        generalNotificationSink.OnStartLoadSettings();
                        break;
                    case SettingsEnumerationMode.Save:
                        generalNotificationSink.OnStartSaveSettings();
                        break;
                    case SettingsEnumerationMode.Delete:
                        generalNotificationSink.OnStartDeleteSettings();
                        break;
                }
            }
        }

        //------------------------------------------------------------------------------
        static void DoSerializerEndAction(ISettingsSerializer serializer, SettingsEnumerationMode mode)
        {
            var generalNotificationSink = serializer as ISerializerNotificationSink;
            if (generalNotificationSink != null)
            {
                switch (mode)
                {
                    case SettingsEnumerationMode.Load:
                        generalNotificationSink.OnEndLoadSettings();
                        break;
                    case SettingsEnumerationMode.Save:
                        generalNotificationSink.OnEndSaveSettings();
                        break;
                    case SettingsEnumerationMode.Delete:
                        generalNotificationSink.OnEndDeleteSettings();
                        break;
                }

                generalNotificationSink.OnEndSerializerAction();
            }
        }

        //------------------------------------------------------------------------------
        static void InitializeSettings()
        {
            if (!InternalConfiguration.IsInitialized)
            {
                // create or get internal instance manually
                // this create settings object
                var obj = Instance;
                // make compiller happy (avoid warning)
                obj.GetType();

                ModuleBaseInitializer.Instance.Initialize();
            }
        }

        //------------------------------------------------------------------------------
        static OrderedSet<ISettingsSerializer> GetSerializersInClass(object settings,
                                                               ref Dictionary<Type, KeyValuePair<ISettingsSerializer, bool/*skip external serializer*/>> serializersCache)
        {
            var members = settings.GetType().GetTypeInfo().DeclaredMembers;

            var serializersInClass = new OrderedSet<ISettingsSerializer>();

            foreach(var member in members)
            {
                var serializer = SettingsHelper.GetSerializerForMemberInfo(settings, GetForegroundSerializer(), member, ref serializersCache);

                if (serializer != null)
                {
                    serializersInClass.Add(serializer);
                }
            }

            return serializersInClass;
        }

        //------------------------------------------------------------------------------
        static ISettingsSerializer GetForegroundSerializer()
        {
            return s_foregroundSerializer;
        }

        //------------------------------------------------------------------------------
        internal static void SetForegroundSerializer(ISettingsSerializer serializer)
        {
            s_foregroundSerializer = serializer;
        }

        #region IPluginContext implementation

        //------------------------------------------------------------------------------
        void IPluginContext.SaveSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin)
        {
            try
            {
                SetForegroundSerializer(pluginForegroundSerializer);

                var sink = pluginForegroundSerializer as IForegroundSerializerNotificationSink;

                if (sink != null)
                {
                    sink.OnStartStaticSettingsSerialization(this);
                }

                SaveObjectSettings();

                if (sink != null)
                {
                    sink.OnEndStaticSettingsSerialization();
                }

                ProcessDynamicSettingsFromContext(sourcePlugin, sink, (IDynamicSettingsPlugin plugin) => { plugin.SaveSettings(); });
            }
            finally
            {
                SetForegroundSerializer(null);
            }
        }

        //------------------------------------------------------------------------------
        void IPluginContext.LoadSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin)
        {
            try
            {
                SetForegroundSerializer(pluginForegroundSerializer);

                var sink = pluginForegroundSerializer as IForegroundSerializerNotificationSink;

                if (sink != null)
                {
                    sink.OnStartStaticSettingsSerialization(this);
                }

                LoadObjectSettings();

                if (sink != null)
                {
                    sink.OnEndStaticSettingsSerialization();
                }

                ProcessDynamicSettingsFromContext(sourcePlugin, sink, (IDynamicSettingsPlugin plugin) => { plugin.LoadSettings(); });

            }
            finally
            {
                SetForegroundSerializer(null);
            }
        }

        //------------------------------------------------------------------------------
        void IPluginContext.DeleteSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin)
        {
            try
            {
                SetForegroundSerializer(pluginForegroundSerializer);

                var sink = pluginForegroundSerializer as IForegroundSerializerNotificationSink;

                if (sink != null)
                {
                    sink.OnStartStaticSettingsSerialization(this);
                }

                DeleteObjectSettings();

                if (sink != null)
                {
                    sink.OnEndStaticSettingsSerialization();
                }

                ProcessDynamicSettingsFromContext(sourcePlugin, sink, (IDynamicSettingsPlugin plugin) => { plugin.DeleteSettings(); });
            }
            finally
            {
                SetForegroundSerializer(null);
            }
        }

        //------------------------------------------------------------------------------
        bool IPluginContext.TryGetSettingMetadata(string settingName, string metadataName, out object metadataValue)
        {
            return TryGetObjectSettingMetadata(settingName, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        bool IPluginContext.TryGetClassMetadata(string metadataName, out object metadataValue)
        {
            return SettingsT<ST>.TryGetClassMetadata(metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        void IPluginContext.SetSettingMetadata(string settingName, string metadataName, object metadataValue)
        {
            SetObjectSettingMetadata(settingName, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        void IPluginContext.SetClassMetadata(string metadataName, object metadataValue)
        {
            SettingsT<ST>.SetClassMetadata(metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        ISettingsSerializer IPluginContext.GetSettingsForegroundSerializer()
        {
            return GetForegroundSerializer();
        }

        #endregion

        #region IPluginHolder implementation

        //------------------------------------------------------------------------------
        object IPluginHolder.GetPlugin(Type pluginInterfaceType)
        {
            lock(s_lockObject)
            {
                InitializeSettings();

                object plugin = GetPluginInternally(pluginInterfaceType);

                return plugin;
            }
        }

        //------------------------------------------------------------------------------
        T IPluginHolder.GetPlugin<T>()
        {
            lock (s_lockObject)
            {
                InitializeSettings();

                object plugin = GetPluginInternally(typeof(T));

                return (T)plugin;
            }
        }

        #endregion

        //------------------------------------------------------------------------------
        object GetPluginInternally(Type pluginInterfaceType)
        {
            lock (s_lockObject)
            {
                object plugin = null;

                bool exist = m_plugins.TryGetValue(pluginInterfaceType, out plugin);
                if (!exist)
                {
                    plugin = InternalConfiguration.GetPluginHelper(pluginInterfaceType, this);
                    m_plugins.Add(pluginInterfaceType, plugin);
                }

                return plugin;
            }
        }

        //------------------------------------------------------------------------------
        IEnumerable<Type> EnumeratePluginTypes()
        {
            return new List<Type>(m_plugins.Keys);
        }

        //------------------------------------------------------------------------------
        void ProcessDynamicSettingsFromContext(SettingsBasePlugin sourcePlugin, IForegroundSerializerNotificationSink sink, Action<IDynamicSettingsPlugin> pluginAction)
        {
            var localDynamicSettings = GetObjectPlugin<Advexp.LocalDynamicSettings.Plugin.ILocalDynamicSettingsPlugin>();
            if (sink != null)
            {
                sink.OnStartDynamicSettingsSerialization(localDynamicSettings as SettingsBasePlugin);
            }

            pluginAction.Invoke(localDynamicSettings);

            if (sink != null)
            {
                sink.OnEndDynamicSettingsSerialization();
            }

            foreach (var pluginInterfaceType in EnumeratePluginTypes())
            {
                var internalPlugin = GetPluginInternally(pluginInterfaceType);
                var dynamicSettingsPlugin = internalPlugin as IDynamicSettingsPlugin;
                if (dynamicSettingsPlugin != null)
                {
                    if (internalPlugin != sourcePlugin && internalPlugin != localDynamicSettings)
                    {
                        if (sink != null)
                        {
                            sink.OnStartDynamicSettingsPluginSerialization(internalPlugin as SettingsBasePlugin);
                        }

                        pluginAction.Invoke(dynamicSettingsPlugin);

                        if (sink != null)
                        {
                            sink.OnEndDynamicSettingsPluginSerialization();
                        }
                    }
                }
            }
        }

        //------------------------------------------------------------------------------
        static SettingBaseAttribute GetSettingBaseAttribute<T>(Expression<Func<ST, T>> value)
        {
            MemberExpression me = (MemberExpression)value.Body;
            MemberInfo mi = me.Member;

            SettingBaseAttribute settingBaseAttribute = mi.GetCustomAttribute<SettingBaseAttribute>();
            if (settingBaseAttribute == null)
            {
                throw new ArgumentException("Member '{0}' does not contain setting attribute", mi.Name);
            }

            return settingBaseAttribute;
        }

        //------------------------------------------------------------------------------
        static void SetObjectSettingMetadataImpl(object settings, string settingName, string metadataName, object metadataValue)
        {
            lock (s_lockObject)
            {
                var settings2 = (SettingsT<ST>)settings;

                var settingMetadata = settings2.m_SettingsMetadata.GetOrCreate(settingName);

                if (metadataValue != null)
                {
                    settingMetadata[metadataName] = metadataValue;
                }
                else
                {
                    settingMetadata.Remove(metadataName);
                }
            }
        }

        //------------------------------------------------------------------------------
        static bool TryGetObjectSettingMetadataImpl(object settings, string settingName, string metadataName, out object metadataValue)
        {
            lock (s_lockObject)
            {
                var settings2 = (SettingsT<ST>)settings;

                var settingMetadata = settings2.m_SettingsMetadata.GetOrCreate(settingName);
                return settingMetadata.TryGetValue(metadataName, out metadataValue);
            }
        }

        //------------------------------------------------------------------------------
        static void SetClassMetadataImpl(string metadataName, object metadataValue)
        {
            lock (s_lockObject)
            {
                var settings = (SettingsT<ST>)(object)Instance;

                if (metadataValue != null)
                {
                    settings.m_ClassMetadata.Add(metadataName, metadataValue);
                }
                else
                {
                    settings.m_ClassMetadata.Remove(metadataName);
                }
            }
        }

        //------------------------------------------------------------------------------
        static bool TryGetClassMetadataImpl(string metadataName, out object metadataValue)
        {
            lock (s_lockObject)
            {
                var settings = (SettingsT<ST>)(object)Instance;

                return settings.m_ClassMetadata.TryGetValue(metadataName, out metadataValue);
            }
        }

    }
}
