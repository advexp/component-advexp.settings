using System;

namespace Advexp
{
    internal class ExceptionForUser : Exception
    {
        public ExceptionForUser(Exception exc) : base(null, exc)
        {
        }
    }
}
