using System;
using System.Collections.Generic;
using NUnit.Framework;
using Advexp;

using System.Linq.Expressions;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    [TestFixture]
    public class ContainsTest
    {
        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            var settings = new DifferentTypesLocalSettings();

            settings.DeleteObjectSettings();

            Assert.IsFalse(settings.ContainsObjectSetting(s => s.m_Boolean));

            settings.SaveObjectSettings();

            Assert.IsTrue(settings.ContainsObjectSetting(s => s.m_Boolean));

            Assert.IsTrue(DifferentTypesLocalSettings.ContainsObjectSetting(settings, (DifferentTypesLocalSettings s) => s.m_Boolean));

            // static settings

            CollectionsSettings.DeleteSettings();

            Assert.IsFalse(CollectionsSettings.ContainsSetting(s => CollectionsSettings.IntList));

            CollectionsSettings.SaveSettings();

            Assert.IsTrue(CollectionsSettings.ContainsSetting(s => CollectionsSettings.IntList));

            tddHandler.CheckErrors();
        }
    }
}

