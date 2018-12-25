using System;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class NotLoadedValueTest
    {
        //------------------------------------------------------------------------------
        [SetUp]
        public void Setup()
        {
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            String refString = "Test";
            DateTime refDateTime = DateTime.Now;
            Int32 refInt32 = 10;

            NotLoadedValuesSettings.DeleteSettings();

            NotLoadedValuesSettings.StringValue = refString;
            NotLoadedValuesSettings.DateTimeValue = refDateTime;
            NotLoadedValuesSettings.Int32Value = refInt32;

            NotLoadedValuesSettings.SecureStringValue = refString;
            NotLoadedValuesSettings.SecureDateTimeValue = refDateTime;
            NotLoadedValuesSettings.SecureInt32Value = refInt32;

            NotLoadedValuesSettings.LoadSettings();

            Assert.AreEqual(refString, NotLoadedValuesSettings.StringValue);
            Assert.AreEqual(refDateTime, NotLoadedValuesSettings.DateTimeValue);
            Assert.AreEqual(refInt32, NotLoadedValuesSettings.Int32Value);

            Assert.AreEqual(refString, NotLoadedValuesSettings.SecureStringValue);
            Assert.AreEqual(refDateTime, NotLoadedValuesSettings.SecureDateTimeValue);
            Assert.AreEqual(refInt32, NotLoadedValuesSettings.SecureInt32Value);

            tddHandler.CheckErrors();
        }
    }
}

