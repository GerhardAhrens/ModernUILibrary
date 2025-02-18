namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class DebugBindingConverter_Test : BaseTest
    {
        [DataRow(new object[] { false, false }, typeof(bool), null, "de-DE", false)]
        [TestMethod]
        public void DebugBinding(object[] input, Type targetType, object parameter, string cultureString, object expectedOutput)
        {
            var culture = new CultureInfo(cultureString);
            var converter = new DebugConverter();
            var output = converter.Convert(input, targetType, parameter, culture);
            Assert.AreEqual(output, Binding.DoNothing);
        }
    }
}
