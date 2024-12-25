namespace ModernTest.ModernBaseLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Core;

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
            Dictionary<int,string> result = aa.ToDictionary<Numbers>();
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<Dictionary<int, string>>(result);
        }

        [DefaultValue(None)]
        private enum Numbers

        {
            None = 0,
            OneHundred = 100,
            TwoHundred = 200
        }
    }
}