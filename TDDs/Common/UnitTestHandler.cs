using System;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    class TDDHandler : ITDDHandler
    {
        int m_errorsCount;
        int m_exceptionsCount;
        int m_accertsCount;

        //------------------------------------------------------------------------------
        public TDDHandler()
        {
            Install();
        }

        //------------------------------------------------------------------------------
        public void Install()
        {
            m_errorsCount = 0;
            m_exceptionsCount = 0;
            m_accertsCount = 0;

            SettingsBaseConfiguration.SetTDDHandler(this);
        }

        //------------------------------------------------------------------------------
        public void CheckErrors()
        {
            Assert.IsTrue(m_errorsCount == 0);
            Assert.IsTrue(m_exceptionsCount == 0);
            Assert.IsTrue(m_accertsCount == 0);
        }

        //------------------------------------------------------------------------------
        void ITDDHandler.Log(LogLevel logLevel, string tag, string message)
        {
            if (logLevel >= LogLevel.Error)
            {
                m_errorsCount++;
            }
        }

        //------------------------------------------------------------------------------
        void ITDDHandler.Log(LogLevel logLevel, string tag, Exception exc)
        {
            if (logLevel >= LogLevel.Error)
            {
                m_exceptionsCount++;
            }
        }

        //------------------------------------------------------------------------------
        void ITDDHandler.Assert(bool condition, string message)
        {
            if (!condition)
            {
                m_accertsCount++;
            }
        }
    }
}

