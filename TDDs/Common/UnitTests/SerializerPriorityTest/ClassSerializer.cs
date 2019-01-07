using System;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    public class ClassSerializer : BaseSerializer
    {
        public static Int32 s_CreationCount = 0;

        public ClassSerializer()
        {
            s_CreationCount++;
        }
    }
}
