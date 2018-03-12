using System;
using System.Collections.Generic;
using Advexp.FirebaseRemoteConfig.Plugin;
using Advexp;

// Important to have the same namespace name (or maybe other type attributes) 
// for different platforms. This allow to deserialize custom objects
namespace Sample.FirebaseRemoteConfig 
{
    public enum EEnumValues
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

    public class CustomObject
    {
        public EEnumValues Enum { get; set; }
        public List<int> List { get; set; }

        public override string ToString()
        {
            return String.Format("Enum: {0}; List size: {1}", 
                                 this.Enum.ToString(), 
                                 this.List.Count);
        }
    }

    class Settings : Advexp.Settings<Settings>
    {
        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_String", Default = "default value")]
        public static String String { get; set; }

        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_Boolean", Default = false)]
        public static Boolean Boolean { get; set; }

        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_SliderValue", Default = 50)]
        public static Int32 SliderValue { get; set; }

        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_DateTime", Default = "2009-06-15T13:45:30.0000000Z")]
        public static DateTime DateTime { get; set; }

        [FirebaseRemoteConfig(Name = "FirebaseRemoteConfig_CustomObject", Default = null)]
        public static CustomObject CustomObject { get; set; }
    }
}
