using System;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class SerializerCreationTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        //------------------------------------------------------------------------------
        [TestFixtureTearDown]
        public void Cleanup()
        {
            SettingsConfiguration.Serializer = null;

            LibrarySerializer.s_CreationCount = 0;
            ClassSerializer.s_CreationCount = 0;
            FieldSerializer.s_CreationCount = 0;
        }

        //------------------------------------------------------------------------------
        void Invoke(Type type, String func)
        {
            var method = type.BaseType.BaseType.GetMethod(func, Type.EmptyTypes);
            method.Invoke(null, null);
        }

        //------------------------------------------------------------------------------
        void Test(String func)
        {
            var tddHandler = new TDDHandler();

            { // library serializer
                LibrarySerializer.s_CreationCount = 0;

                SettingsConfiguration.Serializer = new LibrarySerializer();

                Invoke(typeof(SimpleSettings), func);

                Assert.IsTrue(LibrarySerializer.s_CreationCount == 1);
            }

            { // class serializer over library serializer
                LibrarySerializer.s_CreationCount = 0;
                ClassSerializer.s_CreationCount = 0;

                Invoke(typeof(ClassSerializerSettings), func);

                Assert.IsTrue(LibrarySerializer.s_CreationCount == 0);
                Assert.IsTrue(ClassSerializer.s_CreationCount == 1);
            }

            { // field serializer over class and library serializers
                LibrarySerializer.s_CreationCount = 0;
                ClassSerializer.s_CreationCount = 0;
                FieldSerializer.s_CreationCount = 0;

                Invoke(typeof(FieldSerializerSettings), func);

                Assert.IsTrue(LibrarySerializer.s_CreationCount == 0);
                Assert.IsTrue(ClassSerializer.s_CreationCount == 0);
                Assert.IsTrue(FieldSerializer.s_CreationCount == 1);
            }

            { // field and class serializer over library serializer
                LibrarySerializer.s_CreationCount = 0;
                ClassSerializer.s_CreationCount = 0;
                FieldSerializer.s_CreationCount = 0;

                Invoke(typeof(CompoundSerializerSettings), func);

                Assert.IsTrue(LibrarySerializer.s_CreationCount == 0);
                Assert.IsTrue(ClassSerializer.s_CreationCount == 1);
                Assert.IsTrue(FieldSerializer.s_CreationCount == 1);
            }
            { // field and library serializers
                FieldSerializer.s_CreationCount = 0;
                LibrarySerializer.s_CreationCount = 0;

                // recreate for apropriate s_CreationCount value
                SettingsConfiguration.Serializer = new LibrarySerializer();

                Invoke(typeof(FieldAndLibrarySerializerSettings), func);

                Assert.IsTrue(FieldSerializer.s_CreationCount == 1);
                Assert.IsTrue(LibrarySerializer.s_CreationCount == 1);
            }

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void LoadTest()
        {
            Test("LoadSettings");
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SaveTest()
        {
            Test("SaveSettings");
        }

        //------------------------------------------------------------------------------
        [Test]
        public void DeleteTest()
        {
            Test("DeleteSettings");
        }
    }
}

