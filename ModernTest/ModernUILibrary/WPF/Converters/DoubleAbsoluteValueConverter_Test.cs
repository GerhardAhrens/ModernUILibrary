namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;
    using System.Windows;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_DoubleAbsoluteValueConverter_Test
    {
        private const string cultureString = "de-DE";

        [TestMethod]
        [DataRow(12.4, typeof(object), null, "de-DE", 12.4)]
        [DataRow(-99.99, typeof(object), null, "de-DE", 99.99)]
        public void ConvertToAbsoluteVlue(object input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new DoubleAbsoluteValueConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.Convert(input, targetType, parameter, culture);
            Assert.AreEqual(output, expectedOutput);
        }


        [TestMethod]
        public void ProvideValue_returns_instance()
        {
            var converter = new DoubleAbsoluteValueConverter();
            var providedValue = converter.ProvideValue(null);
            Assert.IsTrue(providedValue.GetType() == typeof(DoubleAbsoluteValueConverter));
        }
    }
}