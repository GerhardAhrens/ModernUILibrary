namespace ModernTest.ModernBaseLibrary.Converter
{
    using global::ModernBaseLibrary.Converter;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TextConverter_Test
    {
        [DataRow("Gerhard", "GERHARD")]
        [TestMethod]
        public void ConvertTextToUpper_A(string input, string expected)
        {
            TextConverter textConverter = new TextConverter(s => s.ToUpper());
            string result = textConverter.ConvertText(input);

            Assert.IsTrue(result == expected);
        }

        [DataRow("Gerhard", "GERHARD")]
        [TestMethod]
        public void ConvertTextToUpper_B(string input, string expected)
        {
            TextConverter textConverter = new TextConverter(this.ToUpperString);
            string result = textConverter.ConvertText(input);

            Assert.IsTrue(result == expected);
        }

        [DataRow("Gerhard", "GERHARD")]
        [TestMethod]
        public void ConvertTextToUpper_C(string input, string expected)
        {
            TextConverter textConverter = new TextConverter(s => 
            { 
                return s.ToUpper(); 
            });

            string result = textConverter.ConvertText(input);

            Assert.IsTrue(result == expected);
        }

        private string ToUpperString(string text)
        {
            return text.ToUpper();
        }
    }
}
