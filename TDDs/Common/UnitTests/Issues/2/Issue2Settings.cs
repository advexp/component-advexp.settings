using System;
using Advexp;

namespace TDD
{
    public class Issue2Settings : Advexp.Settings<TDD.Issue2Settings>
    {
        [Setting(Name = "UserToken", Secure = false, Default = null)]
        public static string UserToken { get; set; }
    }
}

