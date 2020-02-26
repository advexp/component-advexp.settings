using System;
using System.Collections.Generic;

namespace TypeConverter.Converters
{
    class ObjectToBoolConverter : IConvertable<object, bool>, IConvertable<bool, object>
    {
        public bool Convert(object value)
        {
            if (value == null)
            {
                return false;
            }


            string strValue = value.ToString().Trim().ToLower();

            List<string> trueValues = new List<string>(new[] {"1", "j", "ja", "y", "yes", "true", "t", ".t."});
            if(trueValues.Contains(strValue))
            {
                return true;
            }
            List<string> falseValues = new List<string>(new[] {"0", "n", "no", "nein", "false", "f", ".f."});
            if (falseValues.Contains(strValue))
            {
                return false;
            }
            // Ivakin changes
            Int32 intValue = 0;
            if(Int32.TryParse(strValue, out intValue))
            {
                bool result = intValue != 0 ? true : false;
                return result;
            }

            return bool.Parse(strValue);
        }

        public object Convert(bool value)
        {
            throw new NotImplementedException();
        }
    }
}