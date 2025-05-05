namespace ModernTest.ModernBaseLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using global::ModernBaseLibrary.Collection;
    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumAsSource_Test
    {
        [TestMethod]
        public void EnumWithEnumHelper()
        {
            IEnumerable<EnumSource> result = EnumSource.EnumToList(typeof(Numbers));
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<ValueListItem>(result);
        }

        [TestMethod]
        public void EnumWithValueListItem()
        {
            IEnumerable<ValueListItem> result = EnumSource.EnumToIEnumerable(typeof(Numbers));
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<ValueListItem>(result);
        }

        [TestMethod]
        public void EnumToDictionary()
        {
            Numbers aa = Numbers.None;
            Dictionary<int,string> result = aa.ToDictionary<Numbers>(false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void EnumToDictionaryWithDescription()
        {
            Numbers aa = Numbers.None;
            Dictionary<int, string> result = aa.ToDictionary<Numbers>(true);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [DefaultValue(None)]
        public enum Numbers

        {
            [System.ComponentModel.Description("Nix")]
            None = 0,
            [System.ComponentModel.Description("Einhundert")]
            OneHundred = 100,
            [System.ComponentModel.Description("Zweihundert")]
            TwoHundred = 200
        }
    }
}