namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;
    using System.Windows;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_AllBoolToInverseBoolConverter_Test
    {
        [TestMethod]
        [DataRow(new object[] { false, false }, typeof(bool), null, "de-DE", true)]
        [DataRow(new object[] { true, false }, typeof(bool), null, "de-DE", true)]
        [DataRow(new object[] { true, true }, typeof(bool), null, "de-DE", false)]
        public void AllBoolToInverseBoolConverterWithResult(object[] input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var converter = new AllBoolToInverseBoolConverter();
            var culture = new CultureInfo(cultureString);
            var output = converter.Convert(input, targetType, parameter, culture); 
            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void Convert_returns_DependencyPropertyUnsetValue_when_values_null()
        {
            var converter = new AllBoolToInverseBoolConverter();
            var culture = new CultureInfo("de-DE");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var output = converter.Convert(null, typeof(bool), null, culture);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.AreEqual(DependencyProperty.UnsetValue, output);
        }

        [TestMethod]
        public void ConvertBack_throws_NotSupportedException()
        {
            var converter = new AllBoolToInverseBoolConverter();
            var culture = new CultureInfo("de-DE");
            Assert.ThrowsException<NotSupportedException>(() => converter.ConvertBack(true, new[] { typeof(bool) }, null, culture));
        }

        [TestMethod]
        public void ProvideValue_returns_instance()
        {
            var converter = new AllBoolToInverseBoolConverter();
            var providedValue = converter.ProvideValue(null);
            Assert.IsTrue(providedValue.GetType() == typeof(AllBoolToInverseBoolConverter));
        }

        [TestMethod]
        public void Instance_returns_instance()
        {
            var instance = AllBoolToInverseBoolConverter.Instance;
            Assert.IsTrue(instance.GetType() == typeof(AllBoolToInverseBoolConverter));
        }
    }
}