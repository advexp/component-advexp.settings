using Foundation;

namespace TDD
{
    public class ExternalUserDefaultsFieldSerializer
        : Advexp.UserDefaultsSerializer
    {
        static NSUserDefaults s_userDefaults;

        //------------------------------------------------------------------------------
        [Advexp.Preserve]
        public ExternalUserDefaultsFieldSerializer()
        {
            SetUserDefaults(s_userDefaults);
        }

        //------------------------------------------------------------------------------
        public static void SetMyUserDefaults(NSUserDefaults userDefaults)
        {
            s_userDefaults = userDefaults;
        }
    }
}

