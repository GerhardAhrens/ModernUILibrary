namespace ModernTest.ModernBaseLibrary
{
    using System;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringMaskExtensions_Test
    {
        [TestMethod]
        public void StringMask_With_X()
        {
            string someInput = "0621-759-2243";
            string result = someInput.Mask('X', 4);
            Assert.IsTrue(result == "XXXXXXXXX2243");

            string result1 = someInput.Mask('X', 4, MaskStyle.AlphaNumericOnly);
            Assert.IsTrue(result1 == "XXXX-XXX-2243");
        }

        [TestMethod]
        public void StringMask_With_Star()
        {
            string someInput = "0621-759-2243";
            string result = someInput.Mask('*', 3, MaskStyle.AlphaNumericOnly);
            Assert.IsTrue(result == "****-***-*243");

            string result1 = someInput.Mask('*', 4, MaskStyle.AlphaNumericOnly);
            Assert.IsTrue(result1 == "****-***-2243");
        }

        [TestMethod]
        public void FormatMask()
        {
            string data = "aaaaaaaabbbbccccddddeeeeeeeeeeee";
            string pattern = "Hello ########-#A###-####-####-############ Oww";
            string text  = data.FormatMask(pattern);
            bool result = text.Equals("Hello aaaaaaaa-bAbbb-cccc-dddd-eeeeeeeeeeee Oww");
            Assert.IsTrue(result);

            string text1 = "abc".FormatMask("###-#");
            bool result1 = text1.Equals("abc-");
            Assert.IsTrue(result1);

            string text2 = "".FormatMask(pattern);
            bool result2 = text2.Equals("");
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void FormatIBAN_DE()
        {
            string data = "DE11 2222 3333 4444 5555 66";
            string pattern = "#### #### #### #### #### ##";
            string text = data.RemoveAllSpace().FormatMask(pattern);
            bool result = text.Equals("DE11 2222 3333 4444 5555 66");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FormatTrimIBAN_DE()
        {
            string data = "DE11222233334444555566";
            string pattern = "#### #### #### #### #### ##";
            string text = data.RemoveAllSpace().FormatMask(pattern);
            bool result = text.Equals("DE11 2222 3333 4444 5555 66");
            Assert.IsTrue(result);
        }
    }
}
