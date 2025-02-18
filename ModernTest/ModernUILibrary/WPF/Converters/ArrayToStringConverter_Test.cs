namespace ModernTest.ModernUILibrary.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_ArrayToStringConverter_Test
    {
        [TestMethod]
        public void ArrayToStringConverter_Array()
        {
            var converter = new ArrayToStringConverter();
            var culture = new CultureInfo("de-DE");
            string[] input = new string[] { "A", "B", "C" };
            var output = converter.Convert(input, typeof(string), null, culture);
            Assert.AreEqual(output.GetType(),typeof(string));
            Assert.AreEqual(output, "A, B, C");
        }

        [TestMethod]
        public void ArrayToStringConverter_ListString()
        {
            var converter = new ArrayToStringConverter();
            var culture = new CultureInfo("de-DE");
            List<string> input = new List<string> { "A", "B", "C" };
            var output = converter.Convert(input, typeof(string), null, culture);
            Assert.AreEqual(output.GetType(), typeof(string));
            Assert.AreEqual(output, "A, B, C");
        }

        [TestMethod]
        public void ArrayToStringConverter_ListInt()
        {
            var converter = new ArrayToStringConverter();
            var culture = new CultureInfo("de-DE");
            List<int> input = new List<int> { 1, 2, 3 };
            var output = converter.Convert(input, typeof(string), null, culture);
            Assert.AreEqual(output.GetType(), typeof(string));
            Assert.AreEqual(output, "1, 2, 3");
        }
    }
}