using System;
using Advexp;
using NUnit.Framework;
using System.Globalization;

namespace TDD
{
    [TestFixture]
    public class DateTimeTest
    {
        //------------------------------------------------------------------------------
        void Test(ref DateTime value, DateTime refValue)
        {
            value = refValue;
            DateTimeSettings.SaveSettings();

            // invalidate value
            value = new DateTime();

            DateTimeSettings.LoadSettings();

            Assert.AreEqual(value.Ticks, refValue.Ticks);
            Assert.AreEqual(value.Kind, refValue.Kind);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void DefaultValueTest()
        {
            // this local DateTime value was specified in the settings
            DateTime localDateTime = DateTime.ParseExact(DateTimeSettings.DefaultDateTimeLocalStringValue, 
                                                 "o", CultureInfo.InvariantCulture,
                                                 DateTimeStyles.RoundtripKind);
            // this UTC DateTime value was specified in the settings
            DateTime utcDateTime = DateTime.ParseExact(DateTimeSettings.DefaultDateTimeUtcStringValue, 
                                                       "o", CultureInfo.InvariantCulture,
                                                       DateTimeStyles.RoundtripKind);

            DateTimeSettings.DeleteSetting(s => DateTimeSettings.DateTimeLocalValue);
            DateTimeSettings.DeleteSetting(s => DateTimeSettings.DateTimeUtcValue);

            // after delete operation all fields has defaul values
            Assert.AreEqual(DateTimeKind.Local, DateTimeSettings.DateTimeLocalValue.Kind);
            Assert.AreEqual(localDateTime, DateTimeSettings.DateTimeLocalValue);

            Assert.AreEqual(DateTimeKind.Utc, DateTimeSettings.DateTimeUtcValue.Kind);
            Assert.AreEqual(utcDateTime, DateTimeSettings.DateTimeUtcValue);

            // Invalidate value
            DateTimeSettings.DateTimeLocalValue = new DateTime();
            // Load from default value
            DateTimeSettings.LoadSetting(s => DateTimeSettings.DateTimeLocalValue);
            // Check consistency
            Assert.AreEqual(DateTimeKind.Local, DateTimeSettings.DateTimeLocalValue.Kind);
            Assert.AreEqual(localDateTime, DateTimeSettings.DateTimeLocalValue);

            // Invalidate value
            DateTimeSettings.DateTimeUtcValue = new DateTime();
            // Load from default value
            DateTimeSettings.LoadSetting(s => DateTimeSettings.DateTimeUtcValue);
            // Check consistency
            Assert.AreEqual(DateTimeKind.Utc, DateTimeSettings.DateTimeUtcValue.Kind);
            Assert.AreEqual(utcDateTime, DateTimeSettings.DateTimeUtcValue);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            Test(ref DateTimeSettings.DateTime, DateTime.Now);
            Test(ref DateTimeSettings.DateTime, DateTime.UtcNow);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SecureTest()
        {
            Test(ref DateTimeSettings.SecureDateTime, DateTime.Now);
            Test(ref DateTimeSettings.SecureDateTime, DateTime.UtcNow);
        }
    }
}