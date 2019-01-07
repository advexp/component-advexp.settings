using System;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    public class LibrarySerializer : BaseSerializer
    {
        public static Int32 s_CreationCount = 0;

        public LibrarySerializer()
        {
            s_CreationCount++;
        }
    }
}

