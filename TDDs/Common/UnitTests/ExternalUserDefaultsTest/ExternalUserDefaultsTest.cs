﻿using System;
using Foundation;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    [TestFixture]
    public class ExternalUserDefaultsTest
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

            const String stringValue = "external string value";
            const Int32 int32Value = 12345;

            const String stringKeyName = "v2.string";
            const String intKeyName = "v2.int";

            var classPrefs = new NSUserDefaults();

            classPrefs.SetString(stringValue, stringKeyName);
            ExternalUserDefaultsClassSerializer.SetMyUserDefaults(classPrefs);

            var fieldPrefs = new NSUserDefaults();

            fieldPrefs.SetInt(int32Value, intKeyName);
            ExternalUserDefaultsFieldSerializer.SetMyUserDefaults(fieldPrefs);

            ExternalUserDefaultsSettings.IntValue = 0;
            ExternalUserDefaultsSettings.StringValue = String.Empty;

            ExternalUserDefaultsSettings.LoadSettings();

            Assert.AreEqual(ExternalUserDefaultsSettings.IntValue, int32Value);
            Assert.AreEqual(ExternalUserDefaultsSettings.StringValue, stringValue);

            tddHandler.CheckErrors();
        }
    }
}

