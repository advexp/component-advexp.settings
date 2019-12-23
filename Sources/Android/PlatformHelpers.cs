using System;

namespace Advexp
{
    class PlatformHelpers : Logger, IPlatformHelper
    {
        //------------------------------------------------------------------------------
        protected override void LogMessageImpl(LogLevel logLevel, string tag, string message)
        {
            String prefix = base.GetLogPrefix();
            String ouptutMsg = tag + ": " + message;

            Android.Util.LogPriority logPriority;

            switch (logLevel)
            {
                case LogLevel.Info:
                    logPriority = Android.Util.LogPriority.Info;
                    break;

                case LogLevel.Debug:
                    logPriority = Android.Util.LogPriority.Debug;
                    break;

                case LogLevel.Error:
                    logPriority = Android.Util.LogPriority.Error;
                    break;

                case LogLevel.Warning:
                    logPriority = Android.Util.LogPriority.Warn;
                    break;

                case LogLevel.Fatal:
                    logPriority = Android.Util.LogPriority.Error;
                    break;

                default:
                    logPriority = Android.Util.LogPriority.Info;
                    break;
            }

            Android.Util.Log.WriteLine(logPriority, prefix, ouptutMsg);
        }

        #region IPlatformHelper implementation

        //------------------------------------------------------------------------------
        public object ToUnderlyingObject(object obj, Type preferedDestinationType)
        {
            return obj;
        }

        #endregion
    }
}