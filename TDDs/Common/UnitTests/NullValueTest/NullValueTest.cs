using NUnit.Framework;
using Advexp;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    [TestFixture]
    public class NullValueTest
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

            tddHandler.CheckErrors();
        }
    }
}

