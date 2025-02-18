namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_TimeSpanToConverter_Test
    {
        [TestMethod]
        public void TimeSpanToDaysConverter()
        {
            var converter = new TimeSpanToDaysConverter();
            var culture = new CultureInfo("de-DE");
            TimeSpan timeSpan = new TimeSpan(12, 00, 0);
            var output = converter.Convert(timeSpan, typeof(bool), null, culture);
            Assert.AreEqual(output.GetType(),typeof(double));
            Assert.AreEqual(output, 0.5);
        }
    }
}