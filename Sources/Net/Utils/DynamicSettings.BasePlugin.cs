using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Gma.DataStructures;
using System.Collections;
using Advexp.DynamicSettings.Plugin;

namespace Advexp
{
    class DynamicSettingInfo
    {
        public object SettingValue { get; set; }
        public Type SettingType { get; set; }

        //------------------------------------------------------------------------------
        public DynamicSettingInfo()
        {
        }

        //------------------------------------------------------------------------------
        public DynamicSettingInfo(SettingInfo settingInfo)
        {
            this.SettingValue = settingInfo.SettingValue;
            this.SettingType = settingInfo.SettingValueType; 
        }

        //------------------------------------------------------------------------------
        public DynamicSettingInfo(object settingValue, Type settingType)
        {
            this.SettingValue = settingValue;
            this.SettingType = settingType;
        }
    }

    public class DynamicSettingsBasePlugin<ATTRIBUTE> : 
        SettingsBasePlugin, 
        IEnumerable<string>,
        IDynamicSettingsPlugin
        where ATTRIBUTE : SettingBaseAttribute
    {
        DynamicSettingsOrderedCollection m_dynamicSettings = new DynamicSettingsOrderedCollection();
        Dictionary<string, object> m_dynamicSettingsDefaultValues = null;

        FunctionalityHook m_FunctionalityHook;
        readonly object m_lockObject = new object();

        //------------------------------------------------------------------------------
        public override string PluginName
        {
            get
            {
                return "DynamicSettings";
            }
        }

        #region IDynamicSettingsPlugin implementation

        //------------------------------------------------------------------------------
        public virtual void LoadSettings()
        {
            lock (m_lockObject)
            {
                var serializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>();
                var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context, typeof(ATTRIBUTE), ref serializersCache);

                var settingsNames = GetDynamicSettingsNamesForType(serializer, this.Context.GetType(), true);

                m_dynamicSettings.Clear();

                foreach (var name in settingsNames)
                {
                    m_dynamicSettings.Add(name, new DynamicSettingInfo());
                }

                var settingsHelperData = new SettingsHelperData() { SerializersCache = serializersCache };
                ProcessDynamicSettings(SettingsEnumerationMode.Load, ref settingsHelperData);

                var settingsCustomOrder = LoadDynamicSettingsCustomOrder(serializer, this.Context.GetType());
                SetSettingsOrder(settingsCustomOrder);
            }
        }

        //------------------------------------------------------------------------------
        public virtual void SaveSettings()
        {
            lock (m_lockObject)
            {
                var settingsHelperData = new SettingsHelperData()
                {
                    SerializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>(),
                    SerializersToSync = new OrderedSet<ISettingsSerializer>(),
                };

                ProcessDynamicSettings(SettingsEnumerationMode.Save, ref settingsHelperData);

                var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context,
                                                                                            typeof(ATTRIBUTE),
                                                                                            ref settingsHelperData.SerializersCache);

                var settingsNamesInStorage = GetDynamicSettingsNamesForType(serializer, this.Context.GetType(), false);
                var subjectToDeleteSettings = settingsNamesInStorage.Except(m_dynamicSettings.GetFullSettingsNamesWithDefaultOrder());

                var inSettingInfo = new SettingInfo();

                foreach (var settingName in subjectToDeleteSettings)
                {
                    inSettingInfo.SettingName = settingName;

                    inSettingInfo.SettingValue = null;
                    inSettingInfo.SettingValueType = null;
                    inSettingInfo.SettingDefaultValue = null;

                    ProcessDynamicSetting(inSettingInfo, SettingsEnumerationMode.Delete, ref settingsHelperData);
                }

                SaveDynamicSettingsDefaultOrder(serializer, this.Context.GetType(), ref settingsHelperData);
                SaveDynamicSettingsCustomOrder(serializer, this.Context.GetType(), ref settingsHelperData);

                SettingsHelper.SynchronizeSerializers(settingsHelperData.SerializersToSync);
            }
        }

        //------------------------------------------------------------------------------
        public virtual void DeleteSettings()
        {
            lock (m_lockObject)
            {
                var serializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>();

                var settingsHelperData = new SettingsHelperData() { SerializersCache = serializersCache };
                ProcessDynamicSettings(SettingsEnumerationMode.Delete, ref settingsHelperData);

                var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context, typeof(ATTRIBUTE));
                var wishes = serializer as ISettingsSerializerWishes;

                var settingsDefaultOrderFullName = 
                    SettingNameFormatInfo.GetFullDynamicSettingsDefaultOrderSettingName(wishes, this.Context.GetType());
                var settingsCustomOrderFullName =
                    SettingNameFormatInfo.GetFullDynamicSettingsCustomOrderSettingName(wishes, this.Context.GetType());

                var inSettingInfo = new SettingInfo();

                inSettingInfo.SettingName = settingsDefaultOrderFullName;
                ProcessArbitraryDynamicSetting(inSettingInfo, SettingsEnumerationMode.Delete, ref settingsHelperData);

                inSettingInfo.SettingName = settingsCustomOrderFullName;
                ProcessArbitraryDynamicSetting(inSettingInfo, SettingsEnumerationMode.Delete, ref settingsHelperData);

                m_dynamicSettings.Clear();
            }
        }

        //------------------------------------------------------------------------------
        public virtual void LoadSetting(string settingName)
        {
            lock (m_lockObject)
            {
                settingName = CorrectDynamicSettingName(settingName);

                ProcessDynamicSetting(settingName, SettingsEnumerationMode.Load);
            }
        }

        //------------------------------------------------------------------------------
        public virtual void SaveSetting(string settingName)
        {
            lock (m_lockObject)
            {
                settingName = CorrectDynamicSettingName(settingName);

                ProcessDynamicSetting(settingName, SettingsEnumerationMode.Save, true);
            }
        }

        //------------------------------------------------------------------------------
        public virtual void DeleteSetting(string settingName)
        {
            lock (m_lockObject)
            {
                settingName = CorrectDynamicSettingName(settingName);

                ProcessDynamicSetting(settingName, SettingsEnumerationMode.Delete, true);
            }
        }

        //------------------------------------------------------------------------------
        public virtual bool Contains(string settingName)
        {
            lock (m_lockObject)
            {
                var fullSettingName = CorrectDynamicSettingName(settingName);

                return m_dynamicSettings.Contains(fullSettingName);
            }
        }

        //------------------------------------------------------------------------------
        public virtual void SetSettingsOrder(IEnumerable<string> settingsOrder)
        {
            lock (m_lockObject)
            {
                if (settingsOrder != null)
                {
                    var fullSettingsNamesOrder = new List<string>();

                    foreach (var settingName in settingsOrder)
                    {
                        var fullSettingName = CorrectDynamicSettingName(settingName);
                        fullSettingsNamesOrder.Add(fullSettingName);
                    }

                    settingsOrder = fullSettingsNamesOrder;
                }

                m_dynamicSettings.SetCustomOrder(settingsOrder);
            }
        }

        //------------------------------------------------------------------------------
        T GetSettingImpl<T>(string settingName, T defaultValue, bool useInternalDefaultValue)
        {
            lock (m_lockObject)
            {
                var fullSettingName = CorrectDynamicSettingName(settingName);

                object settingValue = null;

                DynamicSettingInfo dsi;
                bool exists = m_dynamicSettings.TryGetValue(fullSettingName, out dsi);

                if (exists)
                {
                    settingValue = dsi.SettingValue;

                    try
                    {
                        T convertedSettingValue = SettingsSerializerHelper.CorrectSettingType<T>(settingValue);

                        return convertedSettingValue;
                    }
                    catch(Exception exc)
                    {
                        var msg = String.Format("Dynamic setting '{0}' type conversion exception", settingName);
                        InternalConfiguration.PlatformHelper.Log(LogLevel.Warning, msg, exc);
                    }
                }

                if (useInternalDefaultValue)
                {
                    if (m_dynamicSettingsDefaultValues == null)
                    {
                        var msg = String.Format("Set default value for setting '{0}'", settingName);
                        throw new KeyNotFoundException(msg);
                    }

                    object objDefaultValue = m_dynamicSettingsDefaultValues[fullSettingName];
                    defaultValue = SettingsSerializerHelper.CorrectSettingType<T>(objDefaultValue);
                }
                else
                {
                    // in this case T defaultValue is valid value
                }

                return defaultValue;
            }
        }

        //------------------------------------------------------------------------------
        public virtual T GetSetting<T>(string settingName)
        {
            return GetSettingImpl<T>(settingName, default(T), true);
        }

        //------------------------------------------------------------------------------
        public virtual T GetSetting<T>(string settingName, T defaultValue)
        {
            return GetSettingImpl<T>(settingName, defaultValue, false);
        }

        //------------------------------------------------------------------------------
        public virtual void SetSetting<T>(string settingName, T settingValue)
        {
            lock (m_lockObject)
            {
                var fullSettingName = CorrectDynamicSettingName(settingName);

                var settingType = typeof(T);
                var settingInfo = new DynamicSettingInfo(settingValue, settingType);

                if (m_dynamicSettings.Contains(fullSettingName))
                {
                    m_dynamicSettings[fullSettingName] = settingInfo;
                }
                else
                {
                    m_dynamicSettings.Add(fullSettingName, settingInfo);
                }
            }
        }

        //------------------------------------------------------------------------------
        public void SetDefaultSettings(IDictionary<string, object> defaultSettings)
        {
            if (defaultSettings != null)
            {
                m_dynamicSettingsDefaultValues = new Dictionary<string, object>();

                foreach(var settingInfo in defaultSettings)
                {
                    var fullSettingName = CorrectDynamicSettingName(settingInfo.Key);
                    m_dynamicSettingsDefaultValues.Add(fullSettingName, settingInfo.Value);
                }
            }
            else
            {
                m_dynamicSettingsDefaultValues = null;
            }
        }

        //------------------------------------------------------------------------------
        public virtual int Count
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_dynamicSettings.Count;
                }
            }
        }

        //------------------------------------------------------------------------------
        public virtual IEnumerator<string> GetEnumerator()
        {
            var thisEnumerable = (IEnumerable<string>)this;
            return thisEnumerable.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation 

        //------------------------------------------------------------------------------
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (m_lockObject)
            {
                var settingsWithCustomOrder = GetSimpleSettingsNamesWithCustomOrder();
                if (settingsWithCustomOrder == null)
                {
                    return GetSimpleSettingsNamesWithDefaultOrder().GetEnumerator();
                }

                return settingsWithCustomOrder.GetEnumerator();
            }
        }

        //------------------------------------------------------------------------------
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            lock (m_lockObject)
            {
                var settingsWithCustomOrder = GetSimpleSettingsNamesWithCustomOrder();
                if (settingsWithCustomOrder == null)
                {
                    return GetSimpleSettingsNamesWithDefaultOrder().GetEnumerator();
                }

                return settingsWithCustomOrder.GetEnumerator();
            }
        }

        #endregion

        //------------------------------------------------------------------------------
        IEnumerable<string> LoadDynamicSettingsOrderImpl(string settingsOrderFullName, ISettingsSerializer serializer, Type settingsType)
        {
            try
            {
                var inSettingInfo = new SettingInfo()
                {
                    SettingName = settingsOrderFullName,
                };

                var settingsHelperData = new SettingsHelperData()
                {
                    SerializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>(),
                    SerializersToSync = new OrderedSet<ISettingsSerializer>(),
                };

                var outSettingInfo = ProcessArbitraryDynamicSetting(inSettingInfo, SettingsEnumerationMode.Load, ref settingsHelperData);

                SettingsHelper.SynchronizeSerializers(settingsHelperData.SerializersToSync);

                if (outSettingInfo == null)
                {
                    return null;
                }

                var settingsNamesOrder = SettingsSerializerHelper.ConvertUsingTypeConverter<string>(outSettingInfo.SettingValue);

                settingsNamesOrder = settingsNamesOrder.Trim();

                var settingsOrderResult = new List<String>();

                if (String.IsNullOrEmpty(settingsNamesOrder))
                {
                    return settingsOrderResult;
                }

                var parts = settingsNamesOrder.Split(',');
                foreach(var settingName in parts)
                {
                    var trimSettingName = settingName.Trim();

                    settingsOrderResult.Add(trimSettingName);
                }

                return settingsOrderResult;
            }
            catch
            {
                // return null in case of any error
            }

            return null;
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> LoadDynamicSettingsDefaultOrder(ISettingsSerializer serializer, Type settingsClassType)
        {
            var settingsOrderFullName = SettingNameFormatInfo.GetFullDynamicSettingsDefaultOrderSettingName(serializer as ISettingsSerializerWishes, settingsClassType);

            return LoadDynamicSettingsOrderImpl(settingsOrderFullName, serializer, settingsClassType);
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> LoadDynamicSettingsCustomOrder(ISettingsSerializer serializer, Type settingsClassType)
        {
            var settingsCustomOrderFullName = SettingNameFormatInfo.GetFullDynamicSettingsCustomOrderSettingName(serializer as ISettingsSerializerWishes, settingsClassType);

            return LoadDynamicSettingsOrderImpl(settingsCustomOrderFullName, serializer, settingsClassType);
        }

        //------------------------------------------------------------------------------
        void SaveDynamicSettingsOrderImpl(string settingsOrderSettingName, IEnumerable<string> settingsNames, ISettingsSerializer serializer, Type settingsType, ref SettingsHelperData settingsHelperData)
        {
            if (settingsNames != null && settingsNames.Count() > 0)
            {
                var simpleSettingsNamesCount = settingsNames.Count();

                StringBuilder builder = new StringBuilder();
                foreach (var settingName in settingsNames)
                {
                    builder.Append(settingName);
                    simpleSettingsNamesCount--;
                    if (simpleSettingsNamesCount != 0)
                    {
                        builder.Append(",");
                    }
                }

                string settingsOrderValue = builder.ToString();

                var inSettingInfo = new SettingInfo()
                {
                    SettingName = settingsOrderSettingName,
                    SettingValue = settingsOrderValue,
                    SettingValueType = settingsOrderValue.GetType(),
                };

                ProcessArbitraryDynamicSetting(inSettingInfo, SettingsEnumerationMode.Save, ref settingsHelperData);

                return;
            }

            bool contains = false;

            try
            {
                contains = serializer.Contains(settingsOrderSettingName, false, null);
            }
            catch(NotSupportedException)
            {

            }

            if (contains)
            {
                var inSettingInfo = new SettingInfo()
                {
                    SettingName = settingsOrderSettingName,
                };

                ProcessArbitraryDynamicSetting(inSettingInfo, SettingsEnumerationMode.Delete, ref settingsHelperData);
            }
        }

        //------------------------------------------------------------------------------
        void SaveDynamicSettingsDefaultOrder(ISettingsSerializer serializer, Type settingsType, ref SettingsHelperData settingsHelperData)
        {
            var settingsDefaultOrderSettingName = SettingNameFormatInfo.GetFullDynamicSettingsDefaultOrderSettingName(serializer as ISettingsSerializerWishes, settingsType);
            var simpleSettingsNames = GetSimpleSettingsNamesWithDefaultOrder();

            SaveDynamicSettingsOrderImpl(settingsDefaultOrderSettingName, simpleSettingsNames, serializer, settingsType, ref settingsHelperData);
        }

        //------------------------------------------------------------------------------
        void SaveDynamicSettingsCustomOrder(ISettingsSerializer serializer, Type settingsType, ref SettingsHelperData settingsHelperData)
        {
            var settingsCustomOrderSettingName = SettingNameFormatInfo.GetFullDynamicSettingsCustomOrderSettingName(serializer as ISettingsSerializerWishes, settingsType);
            var simpleSettingsNames = GetSimpleSettingsNamesWithCustomOrder();

            SaveDynamicSettingsOrderImpl(settingsCustomOrderSettingName, simpleSettingsNames, serializer, settingsType, ref settingsHelperData);
        }

        //------------------------------------------------------------------------------
        void ProcessDynamicSettings(SettingsEnumerationMode mode, ref SettingsHelperData settingHelperData)
        {
            if (settingHelperData.SerializersToSync == null)
            {
                settingHelperData.SerializersToSync = new OrderedSet<ISettingsSerializer>();
            }

            foreach (var fullSettingName in m_dynamicSettings.GetFullSettingsNamesWithDefaultOrder())
            {
                var dynamicSettingInfo = m_dynamicSettings[fullSettingName];
                var inSettingInfo = new SettingInfo(fullSettingName, dynamicSettingInfo);

                ProcessDynamicSetting(inSettingInfo, mode, ref settingHelperData);
            }

            SettingsHelper.SynchronizeSerializers(settingHelperData.SerializersToSync);
        }

        //------------------------------------------------------------------------------
        void ProcessDynamicSetting(string settingName, SettingsEnumerationMode mode, bool saveSettingsOrder = false)
        {
            var settingsHelperData = new SettingsHelperData()
            {
                SerializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>(),
                SerializersToSync = new OrderedSet<ISettingsSerializer>(),
            };

            var dynamicSettingInfo = m_dynamicSettings[settingName];
            var inSettingInfo = new SettingInfo(settingName, dynamicSettingInfo);

            ProcessDynamicSetting(inSettingInfo, mode, ref settingsHelperData);

            if (mode == SettingsEnumerationMode.Delete)
            {
                m_dynamicSettings.Remove(settingName);
            }

            if (saveSettingsOrder)
            {
                var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context, typeof(ATTRIBUTE));
                var settingsClassType = this.Context.GetType();

                SaveDynamicSettingsDefaultOrder(serializer, settingsClassType, ref settingsHelperData);
                SaveDynamicSettingsCustomOrder(serializer, settingsClassType, ref settingsHelperData);
            }

            SettingsHelper.SynchronizeSerializers(settingsHelperData.SerializersToSync);
        }

        //------------------------------------------------------------------------------
        bool ProcessDynamicSetting(SettingInfo inSettingInfo, SettingsEnumerationMode mode,
                                   ref SettingsHelperData settingHelperData)
        {
            var outSettingInfo = ProcessArbitraryDynamicSetting(inSettingInfo, mode, ref settingHelperData);

            bool precessStatus = (outSettingInfo != null);

            if (precessStatus && mode == SettingsEnumerationMode.Load)
            {
                m_dynamicSettings[inSettingInfo.SettingName] = new DynamicSettingInfo(outSettingInfo);
            }
            else if (mode == SettingsEnumerationMode.Load)
            {
                // remove if processStatus is false
                m_dynamicSettings.Remove(inSettingInfo.SettingName);
            }

            return precessStatus;
        }

        //------------------------------------------------------------------------------
        SettingInfo ProcessArbitraryDynamicSetting(SettingInfo inSettingInfo, SettingsEnumerationMode mode,
                                                   ref SettingsHelperData settingHelperData)
        {
            var fh = GetFunctionalityHook();

            var settingsObjectInfo = new SettingsObjectInfo();
            settingsObjectInfo.ForegroundSerializer = this.Context.GetSettingsForegroundSerializer();
            settingsObjectInfo.SettingAttributeType = typeof(ATTRIBUTE);
            settingsObjectInfo.Settings = this.Context;

            if (mode == SettingsEnumerationMode.Save)
            {
                inSettingInfo.SettingValue = DynamicSettingTypeCorrector.MakeDynamicSetting(inSettingInfo);
            }

            SettingInfo outSettingInfo;

            var processStatus = SettingsHelper.ProcessSetting(settingsObjectInfo, inSettingInfo, mode, settingHelperData, out outSettingInfo, fh);

            if (processStatus)
            {
                return outSettingInfo;
            }

            return null;
        }

        //------------------------------------------------------------------------------
        string CorrectDynamicSettingName(string settingName)
        {
            var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context, typeof(ATTRIBUTE));

            settingName = SettingNameFormatInfo.GetFullSettingName(settingName,
                                                                   serializer as ISettingsSerializerWishes,
                                                                   this.Context.GetType(),
                                                                   SettingNameMode.Dynamic);

            return settingName;
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> GetDynamicSettingsNamesForType(ISettingsSerializer serializer, Type settingsType, bool respectOrder)
        {
            IEnumerable<string> result = null;

            var dynamicSettingsInfo = (IDynamicSettingsInfo)serializer;

            var delimeter = SettingNameFormatInfo.GetSettingNameDelimeter(serializer as ISettingsSerializerWishes);

            var settingsTypeInfo = SettingNameFormatInfo.GetSettingsTypeInfo(settingsType, serializer as ISettingsSerializerWishes);
            var settingsNames = dynamicSettingsInfo.GetDynamicSettingsNames();

            var filteredSettingsNames = new List<string>();

            foreach(var rawSettingName in settingsNames)
            {
                string formatVersion;
                string typeInfo;
                string name;

                bool parseResult = SettingNameFormatInfo.Parse(rawSettingName, delimeter, 
                                                         out formatVersion, out typeInfo, out name, 
                                                         SettingNameMode.Dynamic);

                if (parseResult)
                {
                    if (settingsTypeInfo == typeInfo)
                    {
                        filteredSettingsNames.Add(rawSettingName);
                    }
                }
            }

            result = filteredSettingsNames;

            if (respectOrder)
            {
                var settingsOrder = LoadDynamicSettingsDefaultOrder(serializer, settingsType);
                if (settingsOrder != null)
                {
                    var fullSettingsNamesCustomOrder = new List<string>();
                    foreach (var simpleSettingName in settingsOrder)
                    {
                        var fullSettingName = SettingNameFormatInfo.GetFullSettingName(simpleSettingName, 
                                                                                       serializer as ISettingsSerializerWishes, 
                                                                                       settingsType, 
                                                                                       SettingNameMode.Dynamic);
                        fullSettingsNamesCustomOrder.Add(fullSettingName);
                    }

                    // берём только то, что есть в каждой коллекции
                    result = fullSettingsNamesCustomOrder.Intersect(result);
                }
            }

            return result;
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> GetSimpleSettingsNamesImpl(IEnumerable<string> fullSettingsNames)
        {
            if (fullSettingsNames == null)
            {
                return null;
            }

            var settingsNames = new List<string>();
            var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(this.Context, typeof(ATTRIBUTE));

            var delimeter = SettingNameFormatInfo.GetSettingNameDelimeter(serializer as ISettingsSerializerWishes);

            foreach (var fullSettingName in fullSettingsNames)
            {
                string formatVersionPrefix;
                string typeInfo;
                string settingName;

                var parseResult = SettingNameFormatInfo.Parse(fullSettingName, 
                                                              delimeter, 
                                                              out formatVersionPrefix, 
                                                              out typeInfo, 
                                                              out settingName, 
                                                              SettingNameMode.Dynamic);

                if (parseResult)
                {
                    settingsNames.Add(settingName);
                }
            }

            return settingsNames;
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> GetSimpleSettingsNamesWithDefaultOrder()
        {
            var defaultOrder = m_dynamicSettings.GetFullSettingsNamesWithDefaultOrder();

            return GetSimpleSettingsNamesImpl(defaultOrder);
        }

        //------------------------------------------------------------------------------
        IEnumerable<string> GetSimpleSettingsNamesWithCustomOrder()
        {
            var customOrder = m_dynamicSettings.GetFullSettingsNamesWithCustomOrder();

            return GetSimpleSettingsNamesImpl(customOrder);
        }

        //------------------------------------------------------------------------------
        FunctionalityHook GetFunctionalityHook()
        {
            if (m_FunctionalityHook == null)
            {
                m_FunctionalityHook = new FunctionalityHook() { TypeCorrector = new DynamicSettingTypeCorrector() };
            }

            return m_FunctionalityHook;
        }
    }
}
