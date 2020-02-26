using System;

namespace Advexp
{
    interface ITypeCorrector
    {
        Tuple<object, Type, object> CorrectType(object value, Type destinationType, object defaultValue);
    }
}
