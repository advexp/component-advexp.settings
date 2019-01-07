using System;
using System.Collections.Generic;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    [TestFixture]
    public class SerializerPriorityTest
    {
        List<Type> m_actualSerializerTypes = new List<Type>();
        List<TDDData.SerializerActions> m_actualSerializerActions = new List<TDDData.SerializerActions>();
        List<bool> m_actualActionSecurityFlags = new List<bool>();

        //------------------------------------------------------------------------------
        bool IsSyncAction(TDDData.SerializerActions acton)
        {
            if (acton == TDDData.SerializerActions.Delete || acton == TDDData.SerializerActions.Save)
            {
                return true;
            }

            return false;
        }

        //------------------------------------------------------------------------------
        void SerializerAction(Type serializerType, TDDData.SerializerActions action, string settingName, bool secure)
        {
            m_actualSerializerTypes.Add(serializerType);
            m_actualSerializerActions.Add(action);
            m_actualActionSecurityFlags.Add(secure);
        }

        //------------------------------------------------------------------------------
        void Invoke(Type type, string func)
        {
            var method = type.BaseType.BaseType.GetMethod(func, Type.EmptyTypes);
            method.Invoke(null, null);
        }

        //------------------------------------------------------------------------------
        [SetUp]
        public void Setup()
        {
            SettingsConfiguration.TDDData.SerializerAction += SerializerAction;
        }

        //------------------------------------------------------------------------------
        [TearDown]
        public void Cleanup()
        {
            SettingsConfiguration.TDDData.SerializerAction -= SerializerAction;

            SettingsConfiguration.Serializer = null;

            LibrarySerializer.s_CreationCount = 0;
            ClassSerializer.s_CreationCount = 0;
            FieldSerializer.s_CreationCount = 0;
        }

        //------------------------------------------------------------------------------
        void Test(string func, TDDData.SerializerActions action)
        {
            var tddHandler = new TDDHandler();

            { // library serializer
                SettingsConfiguration.Serializer = new LibrarySerializer();
                Invoke(typeof(SimpleSettings), func);

                Type librarySerializerType = typeof(LibrarySerializer);
                var serializerTypes = new List<String>()
                {
                    librarySerializerType.FullName,
                    librarySerializerType.FullName,
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add(librarySerializerType.FullName);
                }

                var actions = new List<TDDData.SerializerActions>()
                {
                    action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            { // class serializer over library serializer
                Invoke(typeof(ClassSerializerSettings), func);

                Type classSerializerType = typeof(ClassSerializer);

                var serializerTypes = new List<String>()
                {
                    classSerializerType.FullName,
                    classSerializerType.FullName,
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add(classSerializerType.FullName);
                }

                var actions = new List<TDDData.SerializerActions>()
                {
                    action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            { // field serializer over class and library serializers
                Invoke(typeof(FieldSerializerSettings), func);

                Type fieldSerializerType = typeof(FieldSerializer);

                var serializerTypes = new List<String>()
                {
                    fieldSerializerType.FullName,
                    fieldSerializerType.FullName,
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add(fieldSerializerType.FullName);
                }

                var actions = new List<TDDData.SerializerActions>()
                {
                    action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            { // field and class serializer over library serializer
                Invoke(typeof(CompoundSerializerSettings), func);

                Type fieldSerializerType = typeof(FieldSerializer);
                Type classSerializerType = typeof(ClassSerializer);

                var serializerTypes = new List<String>() 
                {
                    classSerializerType.FullName,
                    classSerializerType.FullName,
                    fieldSerializerType.FullName,
                    fieldSerializerType.FullName,
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add(classSerializerType.FullName);
                    serializerTypes.Add(fieldSerializerType.FullName);
                }

                var actions = new List<TDDData.SerializerActions>() 
                {
                    action, action, action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, true, false, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            { // field and library serializers
                Invoke(typeof(FieldAndLibrarySerializerSettings), func);

                Type fieldSerializerType = typeof(FieldSerializer);
                Type librarySerializerType = typeof(LibrarySerializer);

                var serializerTypes = new List<String>() 
                {
                    fieldSerializerType.FullName,
                    librarySerializerType.FullName,
                    fieldSerializerType.FullName,
                    librarySerializerType.FullName,
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add(fieldSerializerType.FullName);
                    serializerTypes.Add(librarySerializerType.FullName);
                }

                var actions = new List<TDDData.SerializerActions>() 
                {
                    action, action, action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, false, true, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            { // built in serializers
                SettingsConfiguration.Serializer = null;

                Invoke(typeof(SimpleSettings), func);

                var serializerTypes = new List<String>()
                {
                    "Advexp.ISettingsSerializerImpl",
                    "Advexp.ISettingsSerializerImpl",
                };
                if (IsSyncAction(action))
                {
                    serializerTypes.Add("Advexp.ISettingsSerializerImpl");
                }

                var actions = new List<TDDData.SerializerActions>()
                {
                    action, action
                };
                if (IsSyncAction(action))
                {
                    actions.Add(TDDData.SerializerActions.Synchronize);
                }

                var securityFlags = new List<bool>()
                {
                    false, true
                };
                if (IsSyncAction(action))
                {
                    securityFlags.Add(false);
                }

                CheckSerializerTypeUsage(serializerTypes);
                CheckSerializerActionUsage(actions);
                CheckActionSecurityUsage(securityFlags);
            }

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        void CheckUsage<T1, T2>(List<T1> actual, List<T2> expected, 
                                Action<T1/*actual*/, T2/*expecteed*/> comparer)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            var expectedEnumerator = expected.GetEnumerator();

            foreach(var actualVal in actual)
            {
                expectedEnumerator.MoveNext();

                comparer(actualVal, expectedEnumerator.Current);
            }
        }

        //------------------------------------------------------------------------------
        void CheckSerializerTypeUsage(List<String> expectedSerializerTypes)
        {
            CheckUsage(m_actualSerializerTypes, expectedSerializerTypes, 
                       (Type actualType, String expectedFullName)=>
            {
                Assert.IsTrue(actualType.FullName == expectedFullName);
            });

            m_actualSerializerTypes.Clear();
        }

        //------------------------------------------------------------------------------
        void CheckSerializerActionUsage(List<TDDData.SerializerActions> expectedSerializerActions)
        {
            CheckUsage(m_actualSerializerActions, expectedSerializerActions, 
                       (TDDData.SerializerActions actualAction, 
                        TDDData.SerializerActions expectedAction)=>
            {
                Assert.IsTrue(actualAction == expectedAction);
            });

            m_actualSerializerActions.Clear();
        }

        //------------------------------------------------------------------------------
        void CheckActionSecurityUsage(List<bool> expectedActionSecurityFlags)
        {
            CheckUsage(m_actualActionSecurityFlags, expectedActionSecurityFlags, 
                       (bool actualSecurityFlag, bool expectedActionSecurityFlag)=>
            {
                Assert.IsTrue(actualSecurityFlag == expectedActionSecurityFlag);
            });

            m_actualActionSecurityFlags.Clear();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void LoadTest()
        {
            Test("LoadSettings", TDDData.SerializerActions.Load);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SaveTest()
        {
            Test("SaveSettings", TDDData.SerializerActions.Save);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void DeleteTest()
        {
            Test("DeleteSettings", TDDData.SerializerActions.Delete);
        }
    }
}

