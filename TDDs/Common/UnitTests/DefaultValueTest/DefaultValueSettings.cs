using System;
using Advexp;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    public class DefaultValueSettings : Advexp.Settings<TDD.DefaultValueSettings>
    {
        public const Int32 DefailtIntValue = 10;
        public const EEnumValues DefailtEnumValue = EEnumValues.Five;
        public const string DefailtStringValue = "StringValue";

        [Setting(Default = DefailtIntValue)]
        public static Int32 IntValue {get;set;}

        [Setting(Default = DefailtEnumValue)]
        public static EEnumValues EnumValue {get;set;}

        [Setting(Default = DefailtStringValue)]
        public static string StringValue {get;set;}

        [Setting(Default = null)]
        public static string NullValue {get;set;}

        [Setting(Default = null)]
        public static DateTime? NullableValue {get;set;}

        [Setting]
        public static string ValueNotSet {get;set;}

        [Setting(Default = null)]
        public static bool IncorrectDefaultValueType {get; set;}

        [Setting(Default = DefaulValueMode.TypeDefaultValue)]
        public static int TypeDefaultValue {get;set;}
	}
}

