namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_LineBreakConverter_Test
    {
        [TestMethod]
        public void LineBreakConverter()
        {
            var converter = new LineBreakConverter();
            var culture = new CultureInfo("de-DE");
            string converterInput = "Das ist die erste Zeile\nDas ist die zweite Zeile";
            var output = converter.Convert(converterInput, typeof(string), null, culture);
            Assert.AreEqual(output.GetType(),typeof(string));
            Assert.IsTrue(output.ToString().Contains("&#10;") == true);
        }
    }
}