using System;
using TypeConverter;
using Polenter.Serialization;
using System.IO;
using System.Reflection;
using TypeConverter.Converters;
using System.Globalization;

namespace Advexp
{
    static class SettingsSerializerHelper
    {
        static IConverterRegistry s_ConverterRegistry = null;

        //------------------------------------------------------------------------------
        static public String SaveToStringWithWrapperObject(Object value)
        {
            var wrapper = new Wrapper();
            wrapper.Value = value;
            var stringValue = ObjectToString(wrapper);

            return stringValue;
        }

        //------------------------------------------------------------------------------
        static public object DeserializeToWrapperObjectValue(string stringValue)
        {
            var objectValue = StringToObject<Wrapper>(stringValue);

            return objectValue.Value;
        }

        //------------------------------------------------------------------------------
        public static String ObjectToString(Object objectValue)
        {
            using (var ms = new MemoryStream())
            {
                var settings = SettingsBaseConfiguration.AdvancedConfigurationInternal.SharpSerializerSettings;
                var old_tnc = settings.AdvancedSettings.TypeNameConverter;

                try
                {
                    var tnc = new TypeNameConverter(old_tnc);
                    settings.AdvancedSettings.TypeNameConverter = tnc;

                    var serializer = new SharpSerializer(settings);
                    serializer.Serialize(objectValue, ms);
                }
                finally
                {
                    settings.AdvancedSettings.TypeNameConverter = old_tnc;
                }

                var byteValue = ms.ToArray();
                var stringValue = Convert.ToBase64String(byteValue);

                return stringValue;
            }
        }

        //------------------------------------------------------------------------------
        static public T StringToObject<T>(string stringValue)
        {
            using (var ms = new MemoryStream())
            {
                var byteValue = Convert.FromBase64String(stringValue);
                ms.Write(byteValue, 0, byteValue.Length);
                ms.Seek(0, SeekOrigin.Begin);

                var settings = SettingsBaseConfiguration.AdvancedConfigurationInternal.SharpSerializerSettings;
                var old_tnc = settings.AdvancedSettings.TypeNameConverter;

                try
                {
                    var tnc = new TypeNameConverter(old_tnc);
                    settings.AdvancedSettings.TypeNameConverter = tnc;

                    var serializer = new SharpSerializer(settings);
                    var result = serializer.Deserialize(ms);

                    return (T)result;
                }
                finally
                {
                    settings.AdvancedSettings.TypeNameConverter = old_tnc;
                }
            }
        }

        //------------------------------------------------------------------------------
        static public Object SimplifyObject(Object obj)
        {
            Type type = null;
            bool isEnum = false;

            if (obj != null)
            {
                type = obj.GetType();
                isEnum = type.GetTypeInfo().IsEnum;
            }


            if (type == typeof(Boolean) ||
                type == typeof(Char) ||
                type == typeof(SByte) ||
                type == typeof(Byte) ||
                type == typeof(Int16) ||
                type == typeof(UInt16) ||
                type == typeof(Int32) ||
                type == typeof(UInt32) ||
                type == typeof(Int64) ||
                type == typeof(UInt64) ||
                type == typeof(String) ||
                isEnum
                // this values will be saved with wrapper
                //type == typeof(Single)
                //type == typeof(Double)
                //type == typeof(Decimal)
               )
            {
            }
            else if (type == typeof(DateTime))
            {
                obj = ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture);
            }
            else
            {
                // in case if object is null, custom object or smthng else
                // save it to string with wrapper
                obj = SettingsSerializerHelper.SaveToStringWithWrapperObject(obj);
            }

            return obj;
        }

        //------------------------------------------------------------------------------
        static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        //------------------------------------------------------------------------------
        static bool IsGenericNullable(Type type)
        {
            return type.GetTypeInfo().IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition();
        }

        //------------------------------------------------------------------------------
        static public T CorrectSettingType<T>(object originalValue)
        {
            T result = (T)SettingsSerializerHelper.CorrectSettingType(originalValue, typeof(T));

            return result;
        }

        //------------------------------------------------------------------------------
        static public object CorrectSettingType(object originalValue, Type destinationType)
        {
            Debug.Assert(destinationType != null);

            if (originalValue == null)
            {
                return null;
            }

            if (destinationType == typeof(DateTime) && originalValue.GetType() == destinationType)
            {
                return originalValue;
            }

            if (IsGenericNullable(destinationType))
            {
                destinationType = GetUnderlyingType(destinationType);
            }

            originalValue = 
                InternalConfiguration.PlatformHelper.ToUnderlyingObject(originalValue, destinationType);

            String strValue = originalValue as String;
            if (strValue != null && strValue.IsBase64String())
            {
                try
                {
                    // try to deserialize string value to smthng else
                    originalValue = DeserializeToWrapperObjectValue(strValue);
                    return originalValue;
                }
                catch(Polenter.Serialization.Core.DeserializingException)
                {
                }
                catch(System.FormatException)
                {
                }
                catch(System.IO.EndOfStreamException)
                {
                }
            }

            var result = SettingsSerializerHelper.ConvertUsingTypeConverter(destinationType, originalValue);

            return result;
        }

        //------------------------------------------------------------------------------
        static public object ConvertUsingTypeConverter(Type destinationType, object value)
        {
            if (s_ConverterRegistry == null)
            {
                IConverterRegistry converterRegistry = new ConverterRegistry();

                converterRegistry.RegisterConverter<object, bool>(() => new ObjectToBoolConverter());
                converterRegistry.RegisterConverter<object, DateTime>(() => new ObjectToDateTimeConverter());

                s_ConverterRegistry = converterRegistry;
            }

            object result = s_ConverterRegistry.Convert(destinationType, value);

            return result;
        }

        //------------------------------------------------------------------------------
        static public T ConvertUsingTypeConverter<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }

            T result = (T)ConvertUsingTypeConverter(typeof(T), value);

            return result;
        }

        //------------------------------------------------------------------------------
        static public T GetSettingMetadata<T>(WeakReference weakContext, string settingName, string metadataName, T defaultValue)
        {
            if (String.IsNullOrEmpty(settingName))
            {
                return defaultValue;
            }

            IPluginContext context = (IPluginContext)weakContext.Target;

            object metadataValue;
            bool exists = context.TryGetSettingMetadata(settingName, metadataName, out metadataValue);
            if (!exists)
            {
                return defaultValue;
            }

            try
            {
                return (T)metadataValue;
            }
            catch (InvalidCastException exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Warning, exc);

                var msg = String.Format("InvalidCastException when try to cast setting metadata '{0}'. Use default value instead.", metadataName);
                InternalConfiguration.PlatformHelper.Log(LogLevel.Warning, msg);
            }

            return defaultValue;
        }

        //------------------------------------------------------------------------------
        static public T GetClassMetadata<T>(WeakReference weakContext, string metadataName, T defaultValue)
        {
            IPluginContext context = (IPluginContext)weakContext.Target;

            object metadataValue;
            bool exists = context.TryGetClassMetadata(metadataName, out metadataValue);
            if (!exists)
            {
                return defaultValue;
            }

            try
            {
                return (T)metadataValue;
            }
            catch (InvalidCastException exc)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, exc);

                var msg = String.Format("InvalidCastException when try to cast class metadata '{0}'. Use default value instead.", metadataName);
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error, msg);
            }

            return defaultValue;
        }

        //------------------------------------------------------------------------------
        static public void SetSettingMetadata(WeakReference weakContext, string settingName, string metadataName, object metadataValue)
        {
            IPluginContext context = (IPluginContext)weakContext.Target;

            context.SetSettingMetadata(settingName, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        static public void SetClassMetadata(WeakReference weakContext, string metadataName, object metadataValue)
        {
            IPluginContext context = (IPluginContext)weakContext.Target;

            context.SetClassMetadata(metadataName, metadataValue);

        }
    }
}
