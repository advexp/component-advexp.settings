using System;

#if __TDD__

namespace Advexp
{
    public interface ITDDHandler
    {
        void Log(LogLevel logLevel, string tag, string message);
        void Log(LogLevel logLevel, string tag, Exception exc);

        void Assert(bool condition, string message);
    }
}

#endif // __TDD__
