using System;
using Foundation;
using System.IO;

namespace Advexp
{
    class PlatformHelpers : Logger, IPlatformHelper
    {
        //------------------------------------------------------------------------------
        protected override void LogMessageImpl(LogLevel logLevel, string tag, string message)
        {
            String prefix = base.GetLogPrefix();
            String ouptutMsg = prefix + tag + ": " + message;

            TextWriter writer = null;

            switch (logLevel)
            {
                case LogLevel.Info:
                case LogLevel.Debug:

                    writer = Console.Out;

                    break;

                case LogLevel.Error:
                case LogLevel.Warning:
                case LogLevel.Fatal:

                    writer = Console.Error;

                    break;

                default:

                    writer = Console.Out;

                    break;
            }

            if (writer != null)
            {
                writer.WriteLine(ouptutMsg);
            }
        }

        #region IPlatformHelper implementation

        //------------------------------------------------------------------------------
        public object ToUnderlyingObject(object obj, Type preferedDestinationType)
        {
            NSObject nsObj = obj as NSObject;
            if (nsObj != null)
            {
                obj = nsObj.ToUnderlyingObject(preferedDestinationType);
            }

            return obj;
        }

        #endregion

    }
}