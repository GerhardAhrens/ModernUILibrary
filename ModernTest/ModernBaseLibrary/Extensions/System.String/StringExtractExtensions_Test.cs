namespace ModernTest.ModernBaseLibrary
{
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class StringExtractExtensions_Test
    {
        [TestMethod]
        public void ExtractInts()
        {
            string text = "Hallo [Gerhard] ist ab 28.6.1960 59 Jahre alt";
            int[] result = text.ExtractInts();
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[3] == 59);
        }

        [TestMethod]
        public void ExtractFromString()
        {
            string text = "Hallo [Gerhard] ist ab [28.6.1960] [59] Jahre alt";
            List<string> result  = text.ExtractFromString("[","]").ToList();
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count > 0);
        }
    }
}