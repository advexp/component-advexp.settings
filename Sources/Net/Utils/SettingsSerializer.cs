using System;
using System.Reflection;

namespace Advexp
{
    interface IInternalSettingsSerializer
    {
        bool Load(string settingName, SettingBaseAttribute attr, out object value);
        void Save(string settingName, SettingBaseAttribute attr, object value);
        void Delete(string settingName, SettingBaseAttribute attr);

        bool Contains(string settingName, SettingBaseAttribute attr);

        void Synchronize();
    }

    static class SettingsSerializer
    {
        //------------------------------------------------------------------------------
        public static object GetSettingValue(MemberInfo mi, object o)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
            {
                return fi.GetValue(o);
            }

            var pi = mi as PropertyInfo;
            if (pi != null)
            {
                var getMethod = pi.GetMethod;

                return getMethod.Invoke(o, new object[0]);
            }

            return null;
        }

        //------------------------------------------------------------------------------
        public static void SetSettingValue(MemberInfo mi, object settings, object settingValue)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
            {
                fi.SetValue(settings, settingValue);

                return;
            }

            var pi = mi as PropertyInfo;
            if (pi != null)
            {
                var setMethod = pi.SetMethod;
                if (setMethod == null)
                {
                    var exceptionMsg = String.Format("{0} property is read only", mi.Name);
                    throw new NullReferenceException(exceptionMsg);
                }

                setMethod.Invoke(settings, new object [] { settingValue });

                return;
            }

            var msg = String.Format("{0} is not supported. Field name: {1}", mi.GetType(), mi.Name);
            throw new NotSupportedException(msg);
        }

        //------------------------------------------------------------------------------
        public static Type GetSettingType(MemberInfo mi)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
            {
                return fi.FieldType;
            }

            var pi = mi as PropertyInfo;
            if (pi != null)
            {
                return pi.PropertyType;
            }

            var msg = String.Format("{0} is not supported. Field name: {1}", mi.GetType(), mi.Name);
            throw new NotSupportedException(msg);
        }

        //------------------------------------------------------------------------------
        public static object GetTypeDefaultValue(Type type)
        {
            if(type != null && type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        //------------------------------------------------------------------------------
        public static object GetDefaultValue(SettingBaseAttribute attr, Type settingType)
        {
            object defaultValue = null;

            try
            {
                if (attr != null && attr.DefaultValueInUse)
                {
                    defaultValue = attr.Default;

                    if (defaultValue is DefaulValueMode)
                    {
                        var defaultValueMode = (DefaulValueMode)defaultValue;
                        switch(defaultValueMode)
                        {
                            case DefaulValueMode.TypeDefaultValue:
                                {
                                    defaultValue = GetTypeDefaultValue(settingType);
                                    break;
                                }
                            default:
                                {
                                    throw new NotImplementedException("Unknown DefaultValueMode");
                                }
                        }
                    }
                    else
                    {
                        defaultValue = SettingsSerializerHelper.ConvertUsingTypeConverter(settingType, defaultValue);
                    }
                }
                else
                {
                    defaultValue = GetTypeDefaultValue(settingType);
                }
            }
            catch(Exception exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, "GetDefaultValue exception", exc);
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "Use type default value instead");
                defaultValue = GetTypeDefaultValue(settingType);
            }

            return defaultValue;
        }

        //------------------------------------------------------------------------------
        public static Tuple<object/*value*/, Type/*destinationType*/, object/*defaultValue*/> CorrectValueType(object value, 
                                                                                                               Type destinationType, 
                                                                                                               object defaultValue, 
                                                                                                               FunctionalityHook functionalityHook)
        {
            Tuple<object, Type, object> result;

            try
            {
                if (functionalityHook != null && functionalityHook.TypeCorrector != null)
                {
                    result = functionalityHook.TypeCorrector.CorrectType(value, destinationType, defaultValue);
                }
                else
                {
                    object settingValue = SettingsSerializerHelper.CorrectSettingType(value, destinationType);
                    result = Tuple.Create(settingValue, destinationType, defaultValue);
                }
            }
            catch(Exception exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, exc);

                //throw;
                result = Tuple.Create(defaultValue, destinationType, defaultValue);
            }

            return result;
        }

        /*
        //------------------------------------------------------------------------------
        static bool TryToMigrateFromPrevFormat(ISettingsSerializer serializer, 
                                               string curentSettingName, bool secure, 
                                               Type settingType,
                                               out object value)
        {
            var delimeter = SettingNameFormatInfo.GetSettingNameDelimeter(serializer as ISettingsSerializerWishes);
            SettingNameFormatInfo fi = new SettingNameFormatInfo(curentSettingName, delimeter, SettingNameMode.Unknown);

            try
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "Format migration", 
                                                         String.Format("Try to migrate '{0}' from previous format version", curentSettingName));
                bool loaded = serializer.Load(fi.V1SettingName, secure, out value);

                if (loaded)
                {
                    value = InternalConfiguration.PlatformHelper.CorrectSettingType_V1(value, settingType);

                    InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "Format migration", 
                                                             String.Format("From '{0}' to '{1}' - success", 
                                                                           fi.V1SettingName, curentSettingName));
                    
                    return true;
                }
                else
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "Format migration", 
                                                             String.Format("'{0}' skip migration, no previous value", curentSettingName));
                }
            }
            catch (Exception exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, 
                                                         String.Format("Exception during format migration from '{0}' to '{1}'", 
                                                                       fi.V1SettingName, curentSettingName), 
                                                         exc);
            }

            value = null;

            return false;
        }
        */

        //------------------------------------------------------------------------------
        static public bool Load(ISettingsSerializer serializer,
                                SettingBaseAttribute attr,
                                string settingName, bool secure,
                                Wrapper attributeDefaultValueWrapper,
                                Type destinationType,
                                ref SettingInfo outSettingInfo,
                                FunctionalityHook functionalityHook)
        {
            bool valueWasLoaded = false;
            object loadedValue = null;

            object defaultValue;
            if (attributeDefaultValueWrapper != null)
            {
                defaultValue = attributeDefaultValueWrapper.Value;
            }
            else
            {
                defaultValue = GetTypeDefaultValue(destinationType);
            }

            try
            {
                valueWasLoaded = serializer.Load(settingName, secure, attr, out loadedValue);

                bool valueWasSet = false;

                if (valueWasLoaded)
                {
                    // CorrectValueType in case of exception default value will be returned
                    var outTuple = CorrectValueType(loadedValue, destinationType, defaultValue, functionalityHook);

                    outSettingInfo.SettingValue = outTuple.Item1;
                    outSettingInfo.SettingValueType = outTuple.Item2;
                    outSettingInfo.SettingDefaultValue = outTuple.Item3;
                    outSettingInfo.SettingName = settingName;

                    valueWasSet = true;
                }
                else
                {
                    bool valueWasMigrated = false;
                    object migratedValue = null;

                    /*
                    if (SettingsBaseConfiguration.EnableFormatMigration)
                    {
                        try
                        {
                            valueWasMigrated = TryToMigrateFromPrevFormat(serializer, settingName, secure, destinationType, out migratedValue);
                        }
                        catch(Exception exc)
                        {
                            InternalConfiguration.PlatformHelper.Log(LogLevel.Error, 
                                                                 String.Format("Format migration exception for setting '{0}'", settingName),
                                                                 exc);

                            return false;
                        }
                    }
                    */

                    if (valueWasMigrated)
                    {
                        // in success, object with non-curent type can be returned
                        // correct setting type
                        // CorrectValueType function in case of exception return default value
                        var outTuple = CorrectValueType(migratedValue, destinationType, defaultValue, functionalityHook);

                        outSettingInfo.SettingValue = outTuple.Item1;
                        outSettingInfo.SettingValueType = outTuple.Item2;
                        outSettingInfo.SettingDefaultValue = outTuple.Item3;
                        outSettingInfo.SettingName = settingName;

                        valueWasSet = true;
                    }
                    else if (attributeDefaultValueWrapper != null)
                    {
                        // Use default value only if it was set
                        outSettingInfo.SettingValue = defaultValue;

                        outSettingInfo.SettingValueType = destinationType;
                        outSettingInfo.SettingDefaultValue = defaultValue;
                        outSettingInfo.SettingName = settingName;

                        valueWasSet = true;
                    }
                }

                return valueWasSet;
            }
            catch (ExceptionForUser exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         String.Format("Load exception for setting '{0}'", settingName),
                                                         exc.InnerException);

                throw exc.InnerException;
            }
            catch (Exception exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         String.Format("Load exception for setting '{0}'", settingName),
                                                         exc);
            }

            return false;
        }

        //------------------------------------------------------------------------------
        static public bool Save(ISettingsSerializer serializer,
                                SettingBaseAttribute attr,
                                string settingName, bool secure,
                                object settingValue,
                                FunctionalityHook functionalityHook)
        {
            try
            {
                serializer.Save(settingName, secure, attr, settingValue);
            }
            catch (ExceptionForUser exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         String.Format("Save exception for setting '{0}'", settingName),
                                                         exc.InnerException);

                throw exc.InnerException;
            }
            catch(Exception exc)
            {   
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, 
                                                         String.Format("Save exception for setting '{0}'", settingName),
                                                         exc);
                return false;
            }

            return true;
        }

        //------------------------------------------------------------------------------
        static public bool Delete(ISettingsSerializer serializer, 
                                  SettingBaseAttribute attr,
                                  string settingName, bool secure,
                                  FunctionalityHook functionalityHook)
        {
            try
            {
                serializer.Delete(settingName, secure, attr);
            }
            catch(ExceptionForUser exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         String.Format("Delete exception for setting '{0}'", settingName),
                                                         exc.InnerException);

                throw exc.InnerException;
            }
            catch(Exception exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, 
                                                         String.Format("Delete exception for setting '{0}'", settingName),
                                                         exc);
                return false;
            }

            return true;
        }
    }
}

