using System;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    public class FieldSerializer : BaseSerializer
    {
        public static Int32 s_CreationCount = 0;

        public FieldSerializer()
        {
            s_CreationCount++;
        }
    }
}

