using System;

namespace Advexp
{
    public enum LogLevel
    {
        None, // No log messages at all
        Debug, // Designates fine-grained informational events that are most useful to debug an application
        Info, // Designates informational messages that highlight the progress of the application at coarse-grained level
        Warning, // Designates potentially harmful situations
        Error, // Designates error events that might still allow the application to continue running
        Fatal, // Designates very severe error events that will presumably lead the application to abort
    }

    interface ILogger
    {
        void Log(LogLevel logLevel, string tag, string message);
        void Log(LogLevel logLevel, string message);

        void Log(LogLevel logLevel, string tag, Exception exc);
        void Log(LogLevel logLevel, Exception exc);
        void Log(Exception exc);
    }
}