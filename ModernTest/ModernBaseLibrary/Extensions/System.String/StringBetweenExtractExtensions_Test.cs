namespace ModernTest.ModernBaseLibrary
{
    using System.Collections.Generic;
    using System.Linq;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringBetweenExtractExtensions_Test
    {

        [TestMethod]
        [DataRow("<UserControl x:Class='HomeInfoV3.Core.UI.Rating'><Grid>Test_1</Grid></UserControl>", "Gerhard")]
        public void ExtractFromString(string input, string expected)
        {
            IEnumerable<string> result = input.ExtractFromString("<",">");
            Assert.IsNotNull(result);
            Assert.That.CountGreaterZero<string>(result);
        }

        [TestMethod]
        public void BetweenFirstWord()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            string word = text.Between("[", "]").First();
            Assert.IsTrue(string.IsNullOrEmpty(word) == false);
            Assert.IsTrue(word == "Gerhard");
        }

        [TestMethod]
        public void BetweenLastWord()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            string word = text.Between("[", "]").Last();
            Assert.IsTrue(string.IsNullOrEmpty(word) == false);
            Assert.IsTrue(word == "Beate");
        }

        [TestMethod]
        public void BetweenWordListWithoutSeparator()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            IEnumerable<string> textList = text.Between("[", "]", false);
        }

        [TestMethod]
        public void BetweenWordListWithSeparator()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            IEnumerable<string> textList = text.Between("[", "]", true);
        }
    }
}