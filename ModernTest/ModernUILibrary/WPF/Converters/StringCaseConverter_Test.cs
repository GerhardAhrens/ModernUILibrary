namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_StringCaseConverter_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WPF_StringCaseConverter_Test"/> class.
        /// </summary>
        public WPF_StringCaseConverter_Test()
        {
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertDefaultValueReturnStringAsThis()
        {
            StringCaseConverter converter = new StringCaseConverter();

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This is A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToNormalCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.Normal);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This is A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToNormalCasingModeSetByProperty()
        {
            StringCaseConverter converter = new StringCaseConverter()
            {
                CasingMode = StringCasingMode.Normal
            };

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This is A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToLowerCaseCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.lowercase);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "hello this is a test");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToUpperCaseCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.UPPERCASE);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HELLO THIS IS A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToSetFirstLetterUpperCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.Firstletterupper);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This is A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToFirstLetterOfEachWordUpperCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.FirstLetterOfEachWordUpper);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This Is A TEST");
        }

        [Category("Convert")]
        [TestMethod]
        public void ConvertToFirstLetterOfEachWordLowerCasingModeSetByConstructor()
        {
            StringCaseConverter converter = new StringCaseConverter(StringCasingMode.FirstLetterOfEachWordUpper);

            var result = converter.Convert("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This Is A TEST");
        }

        [Category("ConvertBack")]
        [TestMethod]
        public void ConvertBackStringAsThis()
        {
            StringCaseConverter converter = new StringCaseConverter()
            {
                CasingMode = StringCasingMode.Normal
            };

            var result = converter.ConvertBack("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result, "HelLo This is A TEST");

            converter.CasingMode = StringCasingMode.lowercase;
            var result1 = converter.ConvertBack("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result1, "HelLo This is A TEST");

            converter.CasingMode = StringCasingMode.UPPERCASE;
            var result2 = converter.ConvertBack("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result2, "HelLo This is A TEST");

            converter.CasingMode = StringCasingMode.firstLetterLower;
            var result3 = converter.ConvertBack("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result3, "HelLo This is A TEST");

            converter.CasingMode = StringCasingMode.Firstletterupper;
            var result4 = converter.ConvertBack("HelLo This is A TEST", null, null, null);
            Assert.AreEqual(result4, "HelLo This is A TEST");
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
