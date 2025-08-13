namespace ModernTest.ModernUILibrary.WPF
{
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class IntToBoolConverter_Test
    {
        [TestMethod]
        public void IntToBoolConverter_True()
        {
            var converter = new IntToBoolConverter();
            var culture = new CultureInfo("de-DE");
            int num = 1;
            var output = converter.Convert(num, typeof(bool), null, culture);
            Assert.AreEqual(output.GetType(),typeof(bool));
            Assert.AreEqual(output, true);
        }

        [TestMethod]
        public void IntToBoolConverter_False()
        {
            var converter = new IntToBoolConverter();
            var culture = new CultureInfo("de-DE");
            int num = 0;
            var output = converter.Convert(num, typeof(bool), null, culture);
            Assert.AreEqual(output.GetType(), typeof(bool));
            Assert.AreEqual(output, false);
        }
    }
}