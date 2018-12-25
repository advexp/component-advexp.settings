using System;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class DifferentMemberTypesTest
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

            Random rand = new MyRandom();

            var randInt1 = rand.NextInt32();
            var randInt2 = rand.NextInt32();
            var randInt3 = rand.NextInt32();
            var randInt4 = rand.NextInt32();

            // init values
            DifferentMemberTypesLocalSettings.m_staticField = randInt1;
            DifferentMemberTypesLocalSettings.StaticProperty = randInt2;
            DifferentMemberTypesLocalSettings.Instance.m_memberField = randInt3;
            DifferentMemberTypesLocalSettings.Instance.MemberProperty = randInt4;

            DifferentMemberTypesLocalSettings.SaveSettings();

            // randomize values
            DifferentMemberTypesLocalSettings.m_staticField = rand.NextInt32();
            DifferentMemberTypesLocalSettings.StaticProperty = rand.NextInt32();
            DifferentMemberTypesLocalSettings.Instance.m_memberField = rand.NextInt32();
            DifferentMemberTypesLocalSettings.Instance.MemberProperty = rand.NextInt32();

            DifferentMemberTypesLocalSettings.LoadSettings();

            // check results
            Assert.True(DifferentMemberTypesLocalSettings.m_staticField == randInt1);
            Assert.True(DifferentMemberTypesLocalSettings.StaticProperty == randInt2);
            Assert.True(DifferentMemberTypesLocalSettings.Instance.m_memberField == randInt3);
            Assert.True(DifferentMemberTypesLocalSettings.Instance.MemberProperty == randInt4);

            tddHandler.CheckErrors();
        }
    }

}

