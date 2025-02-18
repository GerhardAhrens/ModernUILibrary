namespace ModernTest.ModernUILibrary.WPF
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_AgeConvertert_Test
    {
        [TestMethod]
        [DataRow("28.06.1960", 59)]
        [DataRow("29.10.1960", 59)]
        [DataRow("01.05.1960", 60)]
        public void AgeConvertert(string input, int expected)
        {
            bool resultOk = DateTime.TryParse(input, out DateTime resultDate);

            var converter = new AgeConverter();
            object result = converter.Convert(resultDate, typeof(int), null, null);
            Assert.IsTrue((int)result == expected);
        }
    }
}