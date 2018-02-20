using System;
using System.Collections.Generic;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class CollectionsTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            const Int32 iterations = 10000;

            InitializeCollections(iterations);

            for(Int32 i=0; i<iterations; i++)
            {
                CollectionsSettings.IntList.Add(i);
                CollectionsSettings.Int2StringDictionary.Add(i, i.ToString());
                CollectionsSettings.IntSet.Add(i);

                CollectionsSettings.SecureIntList.Add(i);
                CollectionsSettings.SecureInt2StringDictionary.Add(i, i.ToString());
                CollectionsSettings.SecureIntSet.Add(i);
            }

            CollectionsSettings.SaveSettings();

            // reset existing values
            InitializeCollections(iterations);

            CollectionsSettings.LoadSettings();

            // Check collections size
            Assert.AreEqual(CollectionsSettings.IntList.Count, iterations);
            Assert.AreEqual(CollectionsSettings.Int2StringDictionary.Count, iterations);
            Assert.AreEqual(CollectionsSettings.IntSet.Count, iterations);

            Assert.AreEqual(CollectionsSettings.SecureIntList.Count, iterations);
            Assert.AreEqual(CollectionsSettings.SecureInt2StringDictionary.Count, iterations);
            Assert.AreEqual(CollectionsSettings.SecureIntSet.Count, iterations);

            for(Int32 i=0; i<iterations; i++)
            {
                // Check list
                Assert.AreEqual(CollectionsSettings.IntList[i], i);

                // Check dictionary
                string strValue;
                CollectionsSettings.Int2StringDictionary.TryGetValue(i, out strValue);
                Assert.IsTrue(strValue == i.ToString());

                // Check hash set
                Assert.IsTrue(CollectionsSettings.IntSet.Contains(i));

                // Check secure list
                Assert.AreEqual(CollectionsSettings.SecureIntList[i], i);

                // Check secure dictionary
                string secureStrValue;
                CollectionsSettings.SecureInt2StringDictionary.TryGetValue(i, out secureStrValue);

                Assert.IsTrue(secureStrValue == i.ToString());

                // Check secure hash set
                Assert.IsTrue(CollectionsSettings.SecureIntSet.Contains(i));

                tddHandler.CheckErrors();
            }
        }

        //------------------------------------------------------------------------------
        void InitializeCollections(Int32 capacity)
        {
            CollectionsSettings.IntList = new List<Int32>(capacity);
            CollectionsSettings.Int2StringDictionary = new Dictionary<int, string>(capacity);
            CollectionsSettings.IntSet = new HashSet<int>();

            CollectionsSettings.SecureIntList = new List<Int32>(capacity);
            CollectionsSettings.SecureInt2StringDictionary = new Dictionary<int, string>(capacity);
            CollectionsSettings.SecureIntSet = new HashSet<int>();
        }
    }
}

