namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;
    using System.Windows;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_BoolReverseConverter_Test
    {
        private const string cultureString = "de-DE";

        [TestMethod]
        [DataRow(false, typeof(bool), null, "de-DE", true)]
        [DataRow(true, typeof(bool), null, "de-DE", false)]
        public void ConvertBool(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new BoolReverseConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.Convert(input, targetType, parameter, culture);
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        [DataRow(false, typeof(bool), null, "de-DE", true)]
        [DataRow(true, typeof(bool), null, "de-DE", false)]
        public void ConvertBackBool(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new BoolReverseConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.ConvertBack(input, targetType, parameter, culture);
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        public void ProvideValue_returns_instance()
        {
            var converter = new BoolReverseConverter();
            var providedValue = converter.ProvideValue(null);
            Assert.IsTrue(providedValue.GetType() == typeof(BoolReverseConverter));
        }
    }
}