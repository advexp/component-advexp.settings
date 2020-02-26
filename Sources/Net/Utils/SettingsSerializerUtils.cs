using System.Collections.Generic;
using System.Text;

namespace Advexp
{
    static class SettingsSerializerUtils
    {
        //------------------------------------------------------------------------------
        public static string RemoveInappropriateSymbols(string text)
        {
            var inappropriateSymbols = new HashSet<char>();

            inappropriateSymbols.Add(SettingsBaseConfiguration.DefaultDelimeter);
            inappropriateSymbols.Add(',');

            StringBuilder sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!inappropriateSymbols.Contains(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
