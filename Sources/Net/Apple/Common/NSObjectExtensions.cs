using System;
using System.Globalization;
using CoreGraphics;
using Foundation;

namespace Advexp
{
    static class NSObjectExtensions
    {
        public static Object ToUnderlyingObject(this NSObject nsO)
        {
            if (nsO != null)
            {
                return nsO.ToUnderlyingObject(null);
            }

            return null;
        }

        public static Object ToUnderlyingObject(this NSObject nsO, Type preferedDestinationType)
        {
            if (nsO == null)
            {
                return null;
            }

            if (nsO is NSString)
            {
                return nsO.ToString();
            }

            if (nsO is NSDate)
            {
                var nsDate = (NSDate)nsO;
                return (DateTime)nsDate;
            }

            if (nsO is NSDecimalNumber)
            {
                return Decimal.Parse(nsO.ToString(), CultureInfo.InvariantCulture);
            }

            var x = nsO as NSNumber;
            if (x != null)
            {
                switch (Type.GetTypeCode(preferedDestinationType))
                {
                    case TypeCode.Boolean:
                        return x.BoolValue;
                    case TypeCode.Char:
                        return Convert.ToChar(x.UInt16Value);
                    case TypeCode.SByte:
                        return x.SByteValue;
                    case TypeCode.Byte:
                        return x.ByteValue;
                    case TypeCode.Int16:
                        return x.Int16Value;
                    case TypeCode.UInt16:
                        return x.UInt16Value;
                    case TypeCode.Int32:
                        return x.Int32Value;
                    case TypeCode.UInt32:
                        return x.UInt32Value;
                    case TypeCode.Int64:
                        return x.Int64Value;
                    case TypeCode.UInt64:
                        return x.UInt64Value;
                    case TypeCode.Single:
                        return x.FloatValue;
                    case TypeCode.Double:
                        return x.DoubleValue;
                    case TypeCode.Decimal:
                        return x.NSDecimalValue;
                    case TypeCode.String:
                        return x.StringValue;
                }
            }

            var v = nsO as NSValue;
            if (v != null)
            {
                if (preferedDestinationType == typeof(IntPtr))
                {
                    return v.PointerValue;
                }

/*
                if (preferedDestinationType == typeof(CGSize))
                {
                    return v.SizeFValue;
                }

                if (preferedDestinationType == typeof(CGRect))
                {
                    return v.RectangleFValue;
                }

                if (preferedDestinationType == typeof(CGPoint))
                {
                    return v.PointFValue;
                }
*/
            }

            return nsO;
        }
    }
}
