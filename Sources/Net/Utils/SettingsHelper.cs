using System;
using System.Collections.Generic;
using System.Reflection;
using Gma.DataStructures;

namespace Advexp
{
    internal enum SettingsEnumerationMode
    {
        Load,
        Save,
        Delete,
        LoadDefaults,
    }

    internal static class SettingsHelper
    {
        //------------------------------------------------------------------------------
        public static ISettingsSerializer GetSerializerDependFromSettingAttributeType(object settings, Type settingAttributeType)
        {
            var serializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>();
            var serializer = SettingsHelper.GetSerializerDependFromSettingAttributeType(settings, settingAttributeType, ref serializersCache);

            return serializer;
        }

        //------------------------------------------------------------------------------
        public static ISettingsSerializer GetSerializerDependFromSettingAttributeType(object settings,
                                                                                      Type settingAttributeType,
                                                                                      ref Dictionary<Type/*serializer type*/, KeyValuePair<ISettingsSerializer, bool/*skip external serializer*/>> serializersCache)
        {
            var serializerInfo = SettingsHelper.GetSerializerInfoDependFromSettingAttributeType(settings, settingAttributeType, ref serializersCache);

            return serializerInfo.Key;
        }

        //------------------------------------------------------------------------------
        public static KeyValuePair<ISettingsSerializer, bool> GetSerializerInfoDependFromSettingAttributeType(object settings, Type settingAttributeType)
        {
            var serializersCache = new Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>>();
            var serializerInfo = SettingsHelper.GetSerializerInfoDependFromSettingAttributeType(settings, settingAttributeType, ref serializersCache);

            return serializerInfo;
        }

        //------------------------------------------------------------------------------
        public static KeyValuePair<ISettingsSerializer, bool> GetSerializerInfoDependFromSettingAttributeType(object settings,
                                                                                                   Type settingAttributeType,
                                                                                                   ref Dictionary<Type/*serializer type*/, KeyValuePair<ISettingsSerializer, bool/*skip external serializer*/>> serializersCache)
        {
            Debug.Assert(settingAttributeType != null);

            var context = settings as IPluginContext;
            if (context != null)
            {
                var serializer = context.GetSettingsForegroundSerializer();
                if (serializer != null)
                {
                    var result = new KeyValuePair<ISettingsSerializer, bool>(serializer, false);
                    return result;
                }
            }

            KeyValuePair<ISettingsSerializer, bool> pair;

            bool isExist = serializersCache.TryGetValue(settingAttributeType, out pair);
            if (!isExist)
            {
                Type serializerType;
                bool exist = InternalConfiguration.SettingsAttributes.TryGetValue(settingAttributeType, out serializerType);
                if (!exist)
                {
                    var msg = String.Format("Can't find serializer for '{0}' attribute. Please enable plugin or register corresponding attribute.", settingAttributeType.FullName);
                    throw new KeyNotFoundException(msg);
                }
                pair = GetCustomSerializer(settings, serializerType, ref serializersCache);
            }

            return pair;
        }

        //------------------------------------------------------------------------------
        public static KeyValuePair<ISettingsSerializer, bool> GetCustomSerializer(object context,
                                                                           Type customSerializerType,
                                                                           ref Dictionary<Type/*serializer type*/,
                                                                           KeyValuePair<ISettingsSerializer,
                                                                           bool/*skip external serializer*/>> serializersCache)
        {
            ISettingsSerializer serializer = null;
            KeyValuePair<ISettingsSerializer, bool> pair;

            bool isExist = serializersCache.TryGetValue(customSerializerType, out pair);
            if (!isExist)
            {
                serializer = (ISettingsSerializer)Activator.CreateInstance(customSerializerType);

                ISettingsSerializerParams serializerParams = serializer as ISettingsSerializerParams;
                if (serializerParams != null)
                {
                    serializerParams.SetSerializerParams(context);
                }

                pair = new KeyValuePair<ISettingsSerializer, bool>(serializer, false);
                serializersCache[customSerializerType] = pair;
            }

            return pair;
        }


        //------------------------------------------------------------------------------
        static ISettingsSerializer GetSerializerForMember(SettingsObjectInfo settingsObjectInfo,
                                                          ref Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>> serializersCache)
        {
            if (settingsObjectInfo.SettingAttributeType == null)
            {
                if (settingsObjectInfo.MemberInfo == null)
                {
                    throw new ArgumentNullException(nameof(settingsObjectInfo.MemberInfo));
                }

                SettingBaseAttribute settingBaseAttr = settingsObjectInfo.MemberInfo.GetCustomAttribute<SettingBaseAttribute>();

                if (settingBaseAttr == null)
                {
                    return null;
                }

                settingsObjectInfo.SettingAttributeType = settingBaseAttr.GetType();
            }

            ISettingsSerializer serializer = settingsObjectInfo.ForegroundSerializer;

            bool skipExternalSerializer = false;
            ISettingsSerializer attributeSerializer = null;

            // try to get setting attribute corresponding serializer
            if (serializer == null)
            {
                var serializerInfo = GetSerializerInfoDependFromSettingAttributeType(settingsObjectInfo.Settings, settingsObjectInfo.SettingAttributeType, ref serializersCache);

                attributeSerializer = serializerInfo.Key;
                skipExternalSerializer = serializerInfo.Value;
            }

            if (settingsObjectInfo.MemberInfo != null)
            {
                // try to get setting (field) serializer
                if (serializer == null && !skipExternalSerializer)
                {
                    SerializerAttribute serializerAttr = settingsObjectInfo.MemberInfo.GetCustomAttribute<SerializerAttribute>();
                    MethodSerializerAttribute methodSerializerAttrForField = settingsObjectInfo.MemberInfo.GetCustomAttribute<MethodSerializerAttribute>();

                    if (methodSerializerAttrForField != null)
                    {
                        serializer = new MethodSerializerForField(methodSerializerAttrForField, settingsObjectInfo.Settings);
                    }
                    else if (serializerAttr != null)
                    {
                        serializer = GetCustomSerializer(settingsObjectInfo.Settings, serializerAttr.Type, ref serializersCache).Key;
                    }
                }
            }

            // try to get class serializer
            if (serializer == null && !skipExternalSerializer)
            {
                SerializerAttribute serializerAttr = settingsObjectInfo.Settings.GetType().GetTypeInfo().
                    GetCustomAttribute<SerializerAttribute>();
                MethodSerializerAttribute methodSerializerAttrForClass = settingsObjectInfo.Settings.GetType().GetTypeInfo().
                    GetCustomAttribute<MethodSerializerAttribute>();

                if (methodSerializerAttrForClass != null)
                {
                    serializer = new MethodSerializerForClass(methodSerializerAttrForClass, settingsObjectInfo.Settings);
                }
                else if (serializerAttr != null)
                {
                    serializer = GetCustomSerializer(settingsObjectInfo.Settings, serializerAttr.Type, ref serializersCache).Key;
                }
            }

            // try to get library serializer
            if (serializer == null && !skipExternalSerializer)
            {
                serializer = SettingsBaseConfiguration.Serializer;
            }

            // try to get attribute serializer in case if allowed
            if (serializer == null && !skipExternalSerializer)
            {
                serializer = attributeSerializer;
            }

            return serializer;
        }

        //------------------------------------------------------------------------------
        public static ISettingsSerializer GetSerializerForMemberInfo(object settings, ISettingsSerializer foregroundSerializer, 
                                                                     MemberInfo mi,
                                                                     ref Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>> serializersCache)
        {
            SettingsObjectInfo soi = new SettingsObjectInfo()
            {
                Settings = settings,
                MemberInfo = mi,
                ForegroundSerializer = foregroundSerializer
            };

            return GetSerializerForMember(soi, ref serializersCache);
        }

        //------------------------------------------------------------------------------
        public static ISettingsSerializer GetSerializerForSettingAttributeType(object settings, ISettingsSerializer foregroundSerializer,
                                                                               Type settingAttributeType,
                                                                               ref Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>> serializersCache)
        {
            SettingsObjectInfo soi = new SettingsObjectInfo()
            {
                Settings = settings, 
                SettingAttributeType = settingAttributeType, 
                ForegroundSerializer = foregroundSerializer
            };

            return GetSerializerForMember(soi, ref serializersCache);
        }

        //------------------------------------------------------------------------------
        public static bool ProcessSetting(SettingsObjectInfo settingsObjectInfo,
                                          SettingInfo inSettingInfo,
                                          SettingsEnumerationMode mode,
                                          SettingsHelperData settingsHelperData,
                                          out SettingInfo outSettingInfo,
                                          FunctionalityHook functionalityHook)
        {
            bool processStatus = false;

            outSettingInfo = new SettingInfo();

            SettingBaseAttribute settingBaseAttribute = null;
            if (settingsObjectInfo.MemberInfo != null)
            {
                settingBaseAttribute = settingsObjectInfo.MemberInfo.GetCustomAttribute<SettingBaseAttribute>();
                if (settingBaseAttribute == null)
                {
                    return false;
                }
            }

            ISettingsSerializer serializer = null;

            if (settingsObjectInfo.MemberInfo != null)
            {
                serializer = GetSerializerForMemberInfo(settingsObjectInfo.Settings, 
                                                        settingsObjectInfo.ForegroundSerializer, 
                                                        settingsObjectInfo.MemberInfo, 
                                                        ref settingsHelperData.SerializersCache);
            }
            else
            {
                serializer = GetSerializerForSettingAttributeType(settingsObjectInfo.Settings,
                                                                  settingsObjectInfo.ForegroundSerializer,
                                                                  settingsObjectInfo.SettingAttributeType,
                                                                  ref settingsHelperData.SerializersCache);
            }

            Debug.Assert(serializer != null);

            if (settingsObjectInfo.MemberInfo != null)
            {
                inSettingInfo.SettingName = SettingNameFormatInfo.GetFullSettingName(settingsObjectInfo.Settings.GetType(), 
                                                                                     settingsObjectInfo.MemberInfo, 
                                                                                     settingBaseAttribute, 
                                                                                     serializer as ISettingsSerializerWishes, 
                                                                                     SettingNameMode.Static);
            }

            if (settingsObjectInfo.MemberInfo != null)
            {
                var settingValueType = SettingsSerializer.GetSettingType(settingsObjectInfo.MemberInfo);

                inSettingInfo.SettingValueType = settingValueType;
                inSettingInfo.SettingValue = SettingsSerializer.GetSettingValue(settingsObjectInfo.MemberInfo, settingsObjectInfo.Settings);
            }

            bool secure = false;

            var settingSecureAttr = settingBaseAttribute as SettingAttribute;
            if (settingSecureAttr != null)
            {
                secure = settingSecureAttr.Secure;
            }

            switch (mode)
            {
                case SettingsEnumerationMode.Load:
                    {
                        Wrapper defaultValueWrapper = null;
                        if (settingBaseAttribute != null && settingBaseAttribute.DefaultValueInUse)
                        {
                            var defaultValue = SettingsSerializer.GetDefaultValue(settingBaseAttribute, inSettingInfo.SettingValueType);
                            defaultValueWrapper = new Wrapper() { Value = defaultValue };
                        }

                        processStatus = SettingsSerializer.Load(serializer,
                                                              settingBaseAttribute,
                                                              inSettingInfo.SettingName, secure,
                                                              defaultValueWrapper,
                                                              inSettingInfo.SettingValueType,
                                                              ref outSettingInfo,
                                                              functionalityHook);

                        // outSettingInfo.SettingValue type was corrected 
                        // in the SettingsSerializer.Load method
                        if (processStatus && settingsObjectInfo.MemberInfo != null)
                        {
                            SettingsSerializer.SetSettingValue(settingsObjectInfo.MemberInfo, settingsObjectInfo.Settings, outSettingInfo.SettingValue);
                        }

#if __TDD__
                        SettingsBaseConfiguration.TDDData.SerializerAction(serializer.GetType(),
                                                                           TDDData.SerializerActions.Load,
                                                                           inSettingInfo.SettingName,
                                                                           secure);
#endif // __TDD__

                        break;
                    }
                case SettingsEnumerationMode.Save:
                    {
                        if (settingsObjectInfo.MemberInfo != null)
                        {
                            inSettingInfo.SettingValue = SettingsSerializer.GetSettingValue(settingsObjectInfo.MemberInfo, settingsObjectInfo.Settings);
                        }

                        processStatus = SettingsSerializer.Save(serializer,
                                                            settingBaseAttribute,
                                                            inSettingInfo.SettingName, secure,
                                                            inSettingInfo.SettingValue,
                                                            functionalityHook);

                        settingsHelperData.SerializersToSync.Add(serializer);

                        outSettingInfo = inSettingInfo;
#if __TDD__
                        SettingsBaseConfiguration.TDDData.SerializerAction(serializer.GetType(),
                                                                           TDDData.SerializerActions.Save,
                                                                           inSettingInfo.SettingName,
                                                                           secure);
#endif // __TDD__

                        break;
                    }
                case SettingsEnumerationMode.Delete:
                    {
                        processStatus = SettingsSerializer.Delete(serializer,
                                                                settingBaseAttribute,
                                                                inSettingInfo.SettingName, secure,
                                                                functionalityHook);

                        settingsHelperData.SerializersToSync.Add(serializer);

                        object defaultValue = SettingsSerializer.GetDefaultValue(settingBaseAttribute, inSettingInfo.SettingValueType);

                        outSettingInfo.SettingName = inSettingInfo.SettingName;
                        outSettingInfo.SettingValue = defaultValue;
                        outSettingInfo.SettingDefaultValue = defaultValue;
                        outSettingInfo.SettingValueType = inSettingInfo.SettingValueType;

                        if (processStatus && settingsObjectInfo.MemberInfo != null)
                        {
                            SettingsSerializer.SetSettingValue(settingsObjectInfo.MemberInfo, 
                                                               settingsObjectInfo.Settings, 
                                                               defaultValue);
                        }
#if __TDD__
                        SettingsBaseConfiguration.TDDData.SerializerAction(serializer.GetType(),
                                                                           TDDData.SerializerActions.Delete,
                                                                           inSettingInfo.SettingName,
                                                                           secure);
#endif // __TDD__

                        break;
                    }
                case SettingsEnumerationMode.LoadDefaults:
                    {
                        Debug.Assert(inSettingInfo.SettingValueType != null);
                        var defaultValue = SettingsSerializer.GetDefaultValue(settingBaseAttribute, inSettingInfo.SettingValueType);

                        if (settingsObjectInfo.MemberInfo != null)
                        {
                            SettingsSerializer.SetSettingValue(settingsObjectInfo.MemberInfo, 
                                                               settingsObjectInfo.Settings, 
                                                               defaultValue);
                        }

                        outSettingInfo.SettingName = inSettingInfo.SettingName;
                        outSettingInfo.SettingValue = defaultValue;
                        outSettingInfo.SettingDefaultValue = defaultValue;
                        outSettingInfo.SettingValueType = inSettingInfo.SettingValueType;

                        processStatus = true;
#if __TDD__
                        SettingsBaseConfiguration.TDDData.SerializerAction(serializer.GetType(),
                                                                           TDDData.SerializerActions.LoadDefaults,
                                                                           inSettingInfo.SettingName,
                                                                           secure);
#endif // __TDD__
                        break;
                    }
                default:
                    Debug.Assert(false);
                    throw new NotImplementedException(String.Format("Not implemented for '{0}' mode", mode));
            }

            return processStatus;
        }

        //------------------------------------------------------------------------------
        public static void SynchronizeSerializers(OrderedSet<ISettingsSerializer> serializersToSync)
        {
            foreach (var serializer in serializersToSync)
            {
                try
                {
                    serializer.Synchronize();

#if __TDD__
                    SettingsBaseConfiguration.TDDData.SerializerAction(serializer.GetType(),
                                                                   TDDData.SerializerActions.Synchronize,
                                                                   String.Empty,
                                                                   false);
#endif // __TDD__
                }
                catch (Exception exc)
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                             String.Format("Synchronization exception for serializer '{0}'",
                                                                           serializer.GetType().GetTypeInfo().FullName), exc);
                }
            }
        }
    }
}
