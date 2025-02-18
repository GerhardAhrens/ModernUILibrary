using System;
using System.Globalization;
using System.Windows;
namespace ModernTest.ModernUILibrary.WPF
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_TextToBoolConverter_Test
    {
        private const string cultureString = "de-DE";

        [TestMethod]
        [DataRow("ja", typeof(bool), null, "de-DE", true)]
        [DataRow("yes", typeof(bool), null, "de-DE", true)]
        public void ConvertToBoolTrue(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new TextToBoolConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.Convert(input, targetType, parameter, culture);
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        [DataRow("nein", typeof(bool), null, "de-DE", false)]
        [DataRow("no", typeof(bool), null, "de-DE", false)]
        public void ConvertToBoolFalse(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new TextToBoolConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.Convert(input, targetType, parameter, culture);
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        [DataRow(true, typeof(string), null, "de-DE", "True")]
        public void ConvertBackToBool(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new TextToBoolConverter();
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