using System;

namespace Advexp
{
    static class Debug
    {
        //------------------------------------------------------------------------------
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
#if __TDD__
            System.Diagnostics.Debug.Assert(condition);
            if (SettingsBaseConfiguration.s_TDDHandler != null)
            {
                SettingsBaseConfiguration.s_TDDHandler.Assert(condition, String.Empty);
            }
#endif // __TDD__
        }

        //------------------------------------------------------------------------------
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
#if __TDD__
            System.Diagnostics.Debug.Assert(condition, message);
            if (SettingsBaseConfiguration.s_TDDHandler != null)
            {
                SettingsBaseConfiguration.s_TDDHandler.Assert(condition, message);
            }
#endif // __TDD__
        }        
    }
}