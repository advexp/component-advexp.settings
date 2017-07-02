using System;
using Android.App;
using Android.Content;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class ExternalSharedPreferencesTest
    {
        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            const String stringKeyName = "v2.string";
            const String intKeyName = "v2.int";

            const String stringValue = "external string value";
            const Int32 intValue = 12345;

            Context classContext = Application.Context;
            Context fieldContext = Application.Context;

            ISharedPreferences classPrefs = classContext.GetSharedPreferences("classPrefs", FileCreationMode.Private);
            ISharedPreferences fieldPrefs = fieldContext.GetSharedPreferences("fieldPrefs", FileCreationMode.Private);

            var classPrefsEditor = classPrefs.Edit();
            classPrefsEditor.PutString(stringKeyName, stringValue);
            classPrefsEditor.Commit();

            var fieldPrefsEditor = fieldPrefs.Edit();
            fieldPrefsEditor.PutInt(intKeyName, intValue);
            fieldPrefsEditor.Commit();

            ExternalSharedPreferencesClassSerializer.SetMySharedPreferences(classPrefs);
            ExternalSharedPreferencesFieldSerializer.SetMySharedPreferences(fieldPrefs);

            ExternalSharedPreferencesSettings.IntValue = 0;
            ExternalSharedPreferencesSettings.StringValue = String.Empty;

            ExternalSharedPreferencesSettings.LoadSettings();

            Assert.AreEqual(ExternalSharedPreferencesSettings.IntValue, intValue);
            Assert.AreEqual(ExternalSharedPreferencesSettings.StringValue, stringValue);
        }
    }
}

