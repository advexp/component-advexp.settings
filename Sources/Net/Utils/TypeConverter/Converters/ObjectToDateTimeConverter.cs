using System;
using System.Globalization;

namespace TypeConverter.Converters
{
    class ObjectToDateTimeConverter : IConvertable<object, DateTime>, IConvertable<DateTime, object>
    {
        //------------------------------------------------------------------------------
        public DateTime Convert(object value)
        {
            if (value == null)
            {
                return new DateTime();
            }

            string stringValue = value.ToString();

            DateTime result;
            bool parsed = DateTime.TryParseExact(stringValue, "o", CultureInfo.InvariantCulture,
                                                               DateTimeStyles.RoundtripKind, out result);
            if (parsed)
            {
                return result;
            }

            parsed = DateTime.TryParse(stringValue, out result);
            if (parsed)
            {
                return result;
            }

            return new DateTime();
        }

        //------------------------------------------------------------------------------
        public object Convert(DateTime value)
        {
            throw new NotImplementedException();
        }
    }
}