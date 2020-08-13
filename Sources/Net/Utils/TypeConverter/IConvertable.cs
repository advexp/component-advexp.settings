using System;

namespace TypeConverter
{
    interface IConvertable<in TSource, out TTarget> : IConvertable
    {
        ////bool CanConvert(TSource value, TTarget target);

        /// <summary>
        ///     Converts the given value of type TSource into an object of type TTarget.
        /// </summary>
        /// <param name="value">The source value to be converted.</param>
        //------------------------------------------------------------------------------
        // related to issue4 Default constructor not found for type
        // https://bitbucket.org/advexp/component-advexp.settings/issues/4/default-constructor-not-found-for-type
        [Advexp.Preserve]
        TTarget Convert(TSource value);
    }

    interface IConvertable
    {
    }
}