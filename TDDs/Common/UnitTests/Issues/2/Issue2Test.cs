using System;
using Advexp;
using System.Reflection;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class Issue2Test
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

            const string token = "cee39cf0f58b252d7add72db41ab209371bf3dd291cdae4c997ebfdd1c645472f2aa293b6221ff527d2f24d0780a4be813e8cc9f2c093097debbb213d8ecff1287685e8de2cc4200add6bb601b3e0328c9c96224e25da0fcb8cf5a16f24360ca3f0111cd4cb1535ebf8b9ab4140a6248";

            Issue2Settings.UserToken = token;

            Issue2Settings.SaveSettings();

            Issue2Settings.UserToken = null;
            Issue2Settings.LoadSettings();

            Assert.AreEqual(token, Issue2Settings.UserToken);

            tddHandler.CheckErrors();
        }
    }
}
