namespace ModernTest.ModernBaseLibrary
{
    using System.Globalization;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CharExtension_Test
    {
        [TestMethod]
        public void CharInList()
        {
            char[] chars = new char[] { 'a', 'b','c', 'z' };
            bool value = 'b'.In(chars);

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void CharNotInList()
        {
            char[] chars = new char[] { 'a', 'b', 'c', 'z' };
            bool value = 'x'.NotIn(chars);

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void GetUnicodeCategory()
        {
            UnicodeCategory value = 'x'.GetUnicodeCategory();

            Assert.IsTrue(value == UnicodeCategory.LowercaseLetter);
        }
    }
}