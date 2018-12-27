using System;
using Advexp;
using System.Reflection;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class DefaulValueTest
    {
        //------------------------------------------------------------------------------
        [SetUp]
        public void Setup()
        {
        }

        //------------------------------------------------------------------------------
        object GetTypeDefaultValue<T>(T value)
        {
            if(typeof(T).GetTypeInfo().IsValueType)
            {
                var defaultValue = Activator.CreateInstance(typeof(T));
                return defaultValue;
            }

            return null;
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            DefaultValueSettings.DeleteSettings();

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.IntValue);
            Assert.AreEqual(DefaultValueSettings.DefailtIntValue, DefaultValueSettings.IntValue);

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.EnumValue);
            Assert.AreEqual(DefaultValueSettings.DefailtEnumValue, DefaultValueSettings.EnumValue);

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.StringValue);
            Assert.AreEqual(DefaultValueSettings.DefailtStringValue, DefaultValueSettings.StringValue);

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.NullValue);
            Assert.IsNull(DefaultValueSettings.NullValue);

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.NullableValue);
            Assert.IsNull(DefaultValueSettings.NullableValue);

            const string notSetValue = "not set test";
            DefaultValueSettings.ValueNotSet = notSetValue;
            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.ValueNotSet);
            Assert.AreEqual(notSetValue, DefaultValueSettings.ValueNotSet);

            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.IncorrectDefaultValueType);
            Assert.AreEqual(GetTypeDefaultValue(DefaultValueSettings.IncorrectDefaultValueType), 
                            DefaultValueSettings.IncorrectDefaultValueType);

            DefaultValueSettings.TypeDefaultValue = 123;
            DefaultValueSettings.LoadSetting(s => DefaultValueSettings.TypeDefaultValue);
            Assert.AreEqual(GetTypeDefaultValue(DefaultValueSettings.TypeDefaultValue),
                            DefaultValueSettings.TypeDefaultValue);

            tddHandler.CheckErrors();
        }
    }
}

