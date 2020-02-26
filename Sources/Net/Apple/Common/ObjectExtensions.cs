using System;
using Foundation;

namespace Advexp
{
    static class ObjectExtensions
    {
        public static NSObject ToNSObject(this Object obj)
        {
            var simpleObj = SettingsSerializerHelper.SimplifyObject(obj);
            Debug.Assert(simpleObj != null);

            return NSObject.FromObject(simpleObj);
        }
    }
}
