using System;

namespace Advexp
{
    class PlatformHelpers : Logger, IPlatformHelper
    {
        //------------------------------------------------------------------------------
        protected override void LogMessageImpl(LogLevel logLevel, string tag, string message)
        {
            String prefix = base.GetLogPrefix();
            String ouptutMsg = prefix + tag + ": " + message;

            System.Diagnostics.Debug.WriteLine(ouptutMsg);
        }

        #region IPlatformHelper implementation

        //------------------------------------------------------------------------------
        public object ToUnderlyingObject(object obj, Type preferedDestinationType)
        {
            return obj;
        }
        /*
        //------------------------------------------------------------------------------
        public object CorrectSettingType_V1(object originalValue, Type destinationType)
        {
            return TypeCorrector.CorrectSettingType_V1(originalValue, destinationType);
        }
        */
        #endregion
    }
}