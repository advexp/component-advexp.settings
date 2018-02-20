using System;
using System.Reflection;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class DifferentTypesTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        //------------------------------------------------------------------------------
        public static void CompareSettings(object settings, object refSettings)
        {
            var members = settings.GetType().GetMembers(
                BindingFlags.DeclaredOnly |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Static);


            foreach(var member in members)
            {
                var dummy = member.GetCustomAttribute<SettingBaseAttribute>();
                if (dummy == null)
                {
                    continue;
                }

                var refMembers = refSettings.GetType().GetMember(member.Name);

                if (refMembers.Length == 0)
                {
                    continue;
                }

                //var dummy2 = refMembers[0].GetCustomAttribute<BaseSettingAttribute>();
                //var dummy3 = refMembers[0].GetCustomAttribute<Advexp_v1X.BaseSettingAttribute>();
                //if (dummy2 == null)
                //{
                //    continue;
                //}

                ProcessMember(member, refMembers[0], settings, refSettings);
            }
        }

        //------------------------------------------------------------------------------
        static void ProcessMember(MemberInfo mi, MemberInfo refMi, object settings, object refSettings)
        {
            var value = GetValue(mi, settings);
            var refValue = GetValue(refMi, refSettings);

            Assert.AreEqual(refValue, value);
        }

        //------------------------------------------------------------------------------
        static object GetValue(MemberInfo mi, object o)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
            {
                return fi.GetValue(o);
            }

            var pi = mi as PropertyInfo;
            if (pi != null)
            {
                var getMethod = pi.GetGetMethod();

                return getMethod.Invoke(o, new object[0]);
            }

            return null;
        }

        //------------------------------------------------------------------------------
        [Test]
        public void LocalTest()
        {
            var tddHandler = new TDDHandler();

            var savedSettings = new DifferentTypesLocalSettings();
            savedSettings.SaveObjectSettings();

            var loadedSettings = new DifferentTypesLocalSettings();
            loadedSettings.LoadObjectSettings();

            CompareSettings(loadedSettings, savedSettings);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SecureTest()
        {
            var tddHandler = new TDDHandler();

            var savedSettings = new DifferentTypesSecureSettings();
            savedSettings.SaveObjectSettings();

            var loadedSettings = new DifferentTypesSecureSettings();
            loadedSettings.LoadObjectSettings();

            CompareSettings(loadedSettings, savedSettings);

            tddHandler.CheckErrors();
        }
    }
}