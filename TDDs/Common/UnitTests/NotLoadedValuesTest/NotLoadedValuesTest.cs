using System;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class NotLoadedValueTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
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
        }
    }
}

