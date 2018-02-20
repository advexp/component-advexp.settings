using System;
using System.Collections.Generic;
using Advexp;

namespace TDD
{
    public class CollectionsSettings : Advexp.Settings<TDD.CollectionsSettings>
    {
        [Setting(Name = "CollectionsSettings.IntList")]
        public static List<Int32> IntList {get; set;}

        [Setting]
        public static Dictionary<Int32, String> Int2StringDictionary {get; set;}

        [Setting]
        public static HashSet<Int32> IntSet {get; set;}

        [Setting(Secure = true)]
        public static List<Int32> SecureIntList {get; set;}

        [Setting(Secure = true)]
        public static Dictionary<Int32, String> SecureInt2StringDictionary {get; set;}

        [Setting(Secure = true)]
        public static HashSet<Int32> SecureIntSet {get; set;}
        }
}
