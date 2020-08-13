using System;
using Security;

namespace Advexp
{
    public class SettingsConfiguration : SettingsBaseConfiguration
    {
        static public SecAccessible KeyChainSecAccessible = SecAccessible.Always;
    }
}

