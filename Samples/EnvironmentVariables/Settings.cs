using Advexp;
using Advexp.EnvironmentVariables.Plugin;

namespace Sample.EnvironmentVariables
{
    class Settings : Advexp.Settings<Settings>
    {
        [EnvironmentVariable(Name = "PATH")]
        public static string Path { get; set; }

        [EnvironmentVariable(Name = "USER_VARIABLE")]
        public static string UserVariable { get; set; }
    }
}
