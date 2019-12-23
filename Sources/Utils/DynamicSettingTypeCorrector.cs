using System;
using System.Text.RegularExpressions;

namespace Advexp
{
    class DynamicSettingTypeCorrector : ITypeCorrector
    {
        //------------------------------------------------------------------------------
        public Tuple<object, Type, object> CorrectType(object value, Type destinationType, object defaultValue)
        {
            var objDestinationValue = SettingsSerializerHelper.CorrectSettingType(value, typeof(object));

            return Tuple.Create<object, Type, object>(objDestinationValue, typeof(object), null);
        }

        //------------------------------------------------------------------------------
        public static object MakeDynamicSetting(SettingInfo inSettingInfo)
        {
            var simpleValue = SettingsSerializerHelper.SimplifyObject(inSettingInfo.SettingValue);
            var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

            return stringValue;
        }
    }
}
