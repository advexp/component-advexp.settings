using System;
using System.Collections.Generic;
using Advexp.FirebaseRemoteConfig.Plugin;
using Advexp;

namespace Sample.FirebaseRemoteConfig.Assembly.Standard
{
    enum EEnumValues
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
    }

    class CustomObject
    {
        public EEnumValues Enum { get; set; }
        public List<int> List { get; set; }

        //public CustomObject()
        //{
        //    this.Enum = EEnumValues.Seven;

        //    this.List = new List<int>();
        //    for (int a = 0; a < 10;a++)
        //    {
        //        this.List.Add(a);
        //    }
        //}

        public override string ToString()
        {
            return String.Format("Enum: {0}; List size: {1}",
                                 this.Enum.ToString(),
                                 this.List.Count);
        }
    }

    class Settings : Advexp.Settings<Settings>
    {
        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_String", Default = "default string value")]
        public static string String {get; set;}

        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_CustomObject", Default = null)]
        public static CustomObject CustomObject { get; set; }
    }
}
