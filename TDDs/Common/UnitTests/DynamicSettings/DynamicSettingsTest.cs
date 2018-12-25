using System;
using System.Collections.Generic;
using Advexp;
using NUnit.Framework;
using Advexp.LocalDynamicSettings.Plugin;

namespace TDD
{
    [TestFixture]
    public class DynamicSettingsTest
    {
        //------------------------------------------------------------------------------
        [SetUp]
        public void Setup()
        {
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestAddAndGetFunctions()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            const int settingsCount = 100;
            const string settingNameFormat = "setting{0}";

            for (int i = 0; i < settingsCount; i++)
            {
                var settingName = String.Format(settingNameFormat, i);
                plugin1_1.SetSetting(settingName, i);
            }

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();

            int index1_2 = 0;
            foreach (var settingName in plugin1_2)
            {
                var dynamicValue1_1 = plugin1_1.GetSetting<int>(settingName);
                var dynamicValue1_2 = plugin1_2.GetSetting<int>(settingName);

                Assert.AreEqual(dynamicValue1_1, index1_2);
                Assert.AreEqual(dynamicValue1_1, dynamicValue1_2);

                index1_2++;
            }

            Assert.AreEqual(index1_2, settingsCount);
            Assert.AreEqual(index1_2, plugin1_2.Count);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestInterferenceBetweenDifferentSettingsClasses()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_1.SetSetting("test", 0);
            plugin1_1.SaveSettings();

            var settings2_1 = new DynamicSettings2();
            var plugin2_1 = settings2_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin2_1.LoadSettings();

            Assert.IsTrue(plugin1_1.Count == 1);
            Assert.IsTrue(plugin2_1.Count == 0);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestContainsFunction()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            var dsn1 = "name1";

            plugin1_1.SetSetting(dsn1, true);
            var dsn_contains1 = plugin1_1.Contains(dsn1);
            Assert.IsTrue(dsn_contains1);

            var dsn2 = "incorrect_name1";

            var dsn_contains2 = plugin1_1.Contains(dsn2);
            Assert.IsFalse(dsn_contains2);

            plugin1_1.DeleteSetting(dsn1);
            var dsn_contains3 = plugin1_1.Contains(dsn1);
            Assert.IsFalse(dsn_contains3);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestRandomValues()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            var dsn1 = "name1";
            var dsn2 = "name2";

            var rand = new MyRandom();
            var randValue = rand.NextDouble();
            var randEnumValue = (EEnumValues)rand.Next((int)EEnumValues.Ten);

            plugin1_1.SetSetting(dsn1, randValue);
            plugin1_1.SetSetting(dsn2, randEnumValue);

            var storedValue1 = plugin1_1.GetSetting<double>(dsn1);
            Assert.AreEqual(randValue, storedValue1);

            var storedEnum1 = plugin1_1.GetSetting<EEnumValues>(dsn2);
            Assert.AreEqual(randEnumValue, storedEnum1);

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();

            var storedValue2 = plugin1_2.GetSetting<double>(dsn1);
            Assert.AreEqual(randValue, storedValue2);

            var storedEnum2 = plugin1_2.GetSetting<EEnumValues>(dsn2);
            Assert.AreEqual(randEnumValue, storedEnum2);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestAddAndSetFunctions()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            var dsn = "name1";

            var rand = new MyRandom();

            var randStringValue = rand.NextString(10);
            var randDecimalValue = rand.NextDecimal();

            plugin1_1.SetSetting(dsn, randStringValue);
            var storedStringValue = plugin1_1.GetSetting<string>(dsn);
            Assert.AreEqual(randStringValue, storedStringValue);

            plugin1_1.SetSetting(dsn, randDecimalValue);
            var storedDecimalValue = plugin1_1.GetSetting<Decimal>(dsn);
            Assert.AreEqual(randDecimalValue, storedDecimalValue);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestCustomAndComplexObjectsAsSettings()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            var rand = new MyRandom();
            var fooList = new List<Foo>();

            for (int i = 0; i < 10; i++)
            {
                var foo = new Foo();
                foo.Value = rand.NextInt32();

                fooList.Add(foo);
            }

            var dsn1 = "name1";

            plugin1_1.SetSetting(dsn1, fooList);

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();

            var loadedFooList = plugin1_2.GetSetting<List<Foo>>(dsn1);

            Assert.AreEqual(fooList, loadedFooList);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestEnumValues()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            var dsn = "test";
            var enumValue = EEnumValues.One;

            plugin1_1.SetSetting(dsn, enumValue);

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();
            var loadedEnumvalue = plugin1_2.GetSetting<EEnumValues>(dsn);

            Assert.AreEqual(enumValue, loadedEnumvalue);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestDynamicSettingsOrder()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            const int settingsCount = 10;

            for (int i = 0; i < settingsCount; i++)
            {
                var settingName = String.Format("setting{0}", i);
                plugin1_1.SetSetting(settingName, i);
            }

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();

            int whileIndex = 0;

            using (var e1 = plugin1_1.GetEnumerator())
            using (var e2 = plugin1_2.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    // compare settings names
                    Assert.AreEqual(e1.Current, e2.Current);

                    var dynamicValue1_1 = plugin1_1.GetSetting<int>(e1.Current);
                    var dynamicValue1_2 = plugin1_2.GetSetting<int>(e2.Current);

                    // compare settings values
                    Assert.AreEqual(dynamicValue1_1, dynamicValue1_2);

                    whileIndex++;
                }
            }

            Assert.AreEqual(plugin1_1.Count, whileIndex);
            Assert.AreEqual(plugin1_2.Count, whileIndex);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        void CompareDynamicSettings(ILocalDynamicSettingsPlugin plugin1, ILocalDynamicSettingsPlugin plugin2)
        {
            int whileIndex = 0;

            using (var e1 = plugin1.GetEnumerator())
            using (var e2 = plugin2.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    // compare settings names
                    Assert.AreEqual(e1.Current, e2.Current);

                    whileIndex++;
                }
            }

            Assert.AreEqual(plugin1.Count, whileIndex);
            Assert.AreEqual(plugin2.Count, whileIndex);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestDynamicSettingsCustomOrder()
        {
            var tddHandler = new TDDHandler();

            //==============================

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            const int settingsCount = 50;
            const string settingNameFormat = "setting{0}";

            var settingsOrder = new List<string>();

            for (int i = 0; i < settingsCount; i++)
            {
                var settingName = String.Format(settingNameFormat, i);
                plugin1_1.SetSetting(settingName, i);

                settingsOrder.Add(settingName);
            }

            plugin1_1.SaveSettings();

            var settings1_2 = new DynamicSettings1();
            var plugin1_2 = settings1_2.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_2.LoadSettings();

            // set custom filter

            settingsOrder.Shuffle();
            plugin1_2.SetSettingsOrder(settingsOrder);

            int index = 0;
            foreach(var ds in plugin1_2)
            {
                Assert.AreEqual(settingsOrder[index], ds);
                index++;
            }

            //==============================

            // save and restore settings in the new instance
            // check settings custom order

            settingsOrder.Shuffle();
            plugin1_2.SetSettingsOrder(settingsOrder);

            plugin1_2.SaveSettings();

            var settings1_3 = new DynamicSettings1();
            var plugin1_3 = settings1_3.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            plugin1_3.LoadSettings();

            // compare with custom order
            CompareDynamicSettings(plugin1_2, plugin1_3);

            plugin1_2.SetSettingsOrder(null);
            plugin1_3.SetSettingsOrder(null);

            // compare with default order
            CompareDynamicSettings(plugin1_2, plugin1_3);

            //==============================

            // set empty custom filter

            plugin1_2.SetSettingsOrder(new List<string>());
            Assert.AreEqual(0, plugin1_2.Count);

            //==============================

            // reset custom filter

            plugin1_2.SetSettingsOrder(null);

            index = 0;
            foreach (var ds in plugin1_2)
            {
                Assert.AreEqual(String.Format(settingNameFormat, index), ds);
                index++;
            }

            //==============================

            // add new dynamic setting when custom filter was set

            const string newDynamicSettingName = "new";

            plugin1_2.SetSettingsOrder(settingsOrder);
            plugin1_2.SetSetting(newDynamicSettingName, 1);

            index = 0;
            foreach (var ds in plugin1_2)
            {
                if (index < settingsOrder.Count)
                {
                    Assert.AreEqual(settingsOrder[index], ds);
                    index++;
                }
                else
                {
                    Assert.AreEqual(newDynamicSettingName, ds);
                }
            }

            //==============================

            // delete existing dynamic setting when custom filter was set

            var rng = new MyRandom();
            var randomTargetIndex = rng.Next(settingsCount);
            var targetSettingName = String.Format(settingNameFormat, randomTargetIndex);

            plugin1_2.SetSettingsOrder(settingsOrder);
            var preCount = plugin1_2.Count;
            plugin1_2.DeleteSetting(targetSettingName);
            settingsOrder.Remove(targetSettingName);

            Assert.AreEqual(preCount - 1, plugin1_2.Count);
            Assert.AreEqual(settingsOrder.Count, plugin1_2.Count);

            index = 0;
            foreach (var ds in plugin1_2)
            {
                Assert.AreEqual(settingsOrder[index], ds);
                index++;
            }

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void TestDefaultValues()
        {
            var tddHandler = new TDDHandler();

            var settings1_1 = new DynamicSettings1();
            var plugin1_1 = settings1_1.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            // try to get setting with no default value
            const string settingName = "setting"; 
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => 
            {
                plugin1_1.GetSetting<string>(settingName);
            });

            // set default value
            const string settingDefaultValue = "default value";
            plugin1_1.SetDefaultSettings(new Dictionary<string, object>()
            {
                {settingName, settingDefaultValue},
            });

            var setting = plugin1_1.GetSetting<string>(settingName);
            Assert.AreEqual(settingDefaultValue, setting);

            // set setting value and try to get it
            const string settingValue = "value";
            plugin1_1.SetSetting(settingName, settingValue);
            setting = plugin1_1.GetSetting<string>(settingName);
            Assert.AreEqual(settingValue, setting);

            plugin1_1.DeleteSetting(settingName);

            // reset defaults
            plugin1_1.SetDefaultSettings(null);
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() =>
            {
                plugin1_1.GetSetting<string>(settingName);
            });

            tddHandler.CheckErrors();
        }
    }
}