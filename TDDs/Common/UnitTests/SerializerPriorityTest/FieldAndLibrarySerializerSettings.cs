using System;
using Advexp;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    public class FieldAndLibrarySerializerSettings : Advexp.Settings<TDD.FieldAndLibrarySerializerSettings>
    {
        [Setting]
        [Serializer(typeof(FieldSerializer))]
        public static Int32 Setting1 {get; set;}

        [Setting]
        public static Int32 Setting2 {get; set;}

        [Setting(Secure = true)]
        [Serializer(typeof(FieldSerializer))]
        public static Int32 Setting3 {get; set;}

        [Setting(Secure = true)]
        public static Int32 Setting4 {get; set;}
    }
}

