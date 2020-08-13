using System.Collections.Generic;

namespace Advexp
{
    public interface IDynamicSettingsInfo
    {
        IEnumerable<string> GetDynamicSettingsNames();
    }
}
