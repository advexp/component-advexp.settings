using System;
using System.Reflection;

namespace Advexp
{
    abstract class Logger : ILogger
    {
        //------------------------------------------------------------------------------
        protected abstract void LogMessageImpl(LogLevel logLevel, string tag, string message);

        //------------------------------------------------------------------------------
        protected virtual void LogExceptionImpl(LogLevel logLevel, string tag, Exception exc)
        {
            string msgPrefix = String.Format("{0}: ", exc.GetType().FullName);
            LogMessageImpl(logLevel, tag, msgPrefix + exc.Message);

            if (exc.InnerException != null)
            {
                tag = "Inner exception";
                LogExceptionImpl(logLevel, tag, exc.InnerException);
            }
        }

        //------------------------------------------------------------------------------
        protected void LogMessage(LogLevel logLevel, string tag, string message)
        {
            if (SettingsBaseConfiguration.LogLevel == LogLevel.None)
            {
                return;
            }

            if (logLevel < SettingsBaseConfiguration.LogLevel)
            {
                return;
            }

#if __TDD__

            if (SettingsBaseConfiguration.s_TDDHandler != null)
            {
                SettingsBaseConfiguration.s_TDDHandler.Log(logLevel, tag, message);
            }

#endif // __TDD__

            LogMessageImpl(logLevel, tag, message);
        }

        //------------------------------------------------------------------------------
        protected void LogException(LogLevel logLevel, string tag, Exception exc)
        {
            if (logLevel < SettingsBaseConfiguration.LogLevel)
            {
                return;
            }

#if __TDD__

            if (SettingsBaseConfiguration.s_TDDHandler != null)
            {
                SettingsBaseConfiguration.s_TDDHandler.Log(logLevel, tag, exc);
            }

#endif // __TDD__

            LogExceptionImpl(logLevel, tag, exc);
        }

        //------------------------------------------------------------------------------
        protected string GetLogPrefix()
        {
            var assemblyVersion = GetType().GetTypeInfo().Assembly.GetName().Version.ToString();
            var result = String.Format("[Advexp.Settings {0}]  ", assemblyVersion);

            return result;
        }

        #region ILogger implementation

        //------------------------------------------------------------------------------
        public void Log(LogLevel logLevel, string tag, string message)
        {
            LogMessage(logLevel, tag, message);
        }

        //------------------------------------------------------------------------------
        public void Log(LogLevel logLevel, string message)
        {
            Log(logLevel, String.Empty, message);
        }

        //------------------------------------------------------------------------------
        public void Log(LogLevel logLevel, string tag, Exception exc)
        {
            LogException(logLevel, tag, exc);
        }

        //------------------------------------------------------------------------------
        public void Log(LogLevel logLevel, Exception exc)
        {
            Log(logLevel, String.Empty, exc);
        }

        //------------------------------------------------------------------------------
        public void Log(Exception exc)
        {
            Log(LogLevel.Error, exc);
        }

        #endregion
    }
}