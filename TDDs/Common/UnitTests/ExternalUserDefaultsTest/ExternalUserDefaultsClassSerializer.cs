using Foundation;

namespace TDD
{
    public class ExternalUserDefaultsClassSerializer
        : Advexp.UserDefaultsSerializer
    {
        static NSUserDefaults s_userDefaults;

        //------------------------------------------------------------------------------
        [Advexp.Preserve]
        public ExternalUserDefaultsClassSerializer()
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

