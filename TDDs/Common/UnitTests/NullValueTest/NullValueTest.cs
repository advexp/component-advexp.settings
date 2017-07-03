using NUnit.Framework;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class NullValueTest
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
            const string strValue = "test";

            NullValueSettings.NullValue = null;
            NullValueSettings.NormalValue = strValue;

            NullValueSettings.SecureNullValue = null;
            NullValueSettings.SecureNormalValue = strValue;

            NullValueSettings.SaveSettings();

            NullValueSettings.NullValue = strValue;
            NullValueSettings.NormalValue = null;

            NullValueSettings.SecureNullValue = strValue;
            NullValueSettings.SecureNormalValue = null;

            NullValueSettings.LoadSettings();

            Assert.IsTrue(strValue == NullValueSettings.NormalValue);
            Assert.IsNull(NullValueSettings.NullValue);

            Assert.IsTrue(strValue == NullValueSettings.SecureNormalValue);
            Assert.IsNull(NullValueSettings.SecureNullValue);
        }
    }
}

