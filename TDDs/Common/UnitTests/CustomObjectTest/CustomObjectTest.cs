using System;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class CustomObjectTest
    {
        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            CustomObjectSettings.FooClassInstance = new FooClass();
            CustomObjectSettings.SecureFooClassInstance = new FooClass();

            Random rand = new MyRandom();

            Int32 fooValue = rand.NextInt32();
            Int32 secureFooValue = rand.NextInt32();

            CustomObjectSettings.FooClassInstance.IntValue = fooValue;
            CustomObjectSettings.SecureFooClassInstance.IntValue = secureFooValue;

            FooEnum fooEnumValue = (FooEnum)rand.Next((Int32)FooEnum.One, (Int32)FooEnum.Ten);
            FooEnum secureFooEnumValue = (FooEnum)rand.Next((Int32)FooEnum.One, (Int32)FooEnum.Ten);

            CustomObjectSettings.FooEnumValue = fooEnumValue;
            CustomObjectSettings.SecureFooEnumValue = secureFooEnumValue;
            CustomObjectSettings.SaveSettings();

            CustomObjectSettings.FooClassInstance = null;
            CustomObjectSettings.SecureFooClassInstance = null;
            CustomObjectSettings.FooEnumValue = FooEnum.Zero;
            CustomObjectSettings.SecureFooEnumValue = FooEnum.Zero;

            CustomObjectSettings.LoadSettings();

            Assert.IsNotNull(CustomObjectSettings.FooClassInstance);
            Assert.IsTrue(typeof(FooClass) == CustomObjectSettings.FooClassInstance.GetType());
            Assert.AreEqual(CustomObjectSettings.FooClassInstance.IntValue, fooValue);
            Assert.AreEqual(CustomObjectSettings.FooEnumValue, fooEnumValue);

            Assert.IsNotNull(CustomObjectSettings.SecureFooClassInstance);
            Assert.IsTrue(typeof(FooClass) == CustomObjectSettings.SecureFooClassInstance.GetType());
            Assert.AreEqual(CustomObjectSettings.SecureFooClassInstance.IntValue, secureFooValue);
            Assert.AreEqual(CustomObjectSettings.SecureFooEnumValue, secureFooEnumValue);
        }
    }
}

