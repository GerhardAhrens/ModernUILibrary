namespace ModernTest.ModernUILibrary.WPF
{
    using System;


    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class WPF_Converter_Test
    {
        [TestMethod]
        public void DateTimeToStringConverter_Test()
        {
            var converter = new DateTimeToStringConverter();
            object result = converter.Convert(new DateTime(1900, 1, 1), typeof(string), null, null);
            Assert.IsTrue(result == null);

            result = converter.Convert(new DateTime(1, 1, 1), typeof(string), null, null);
            Assert.IsTrue(result == null);

            result = converter.Convert(new DateTime(1960, 6, 28), typeof(string), null, null);
            Assert.IsTrue(result.ToString() == "28.06.1960");
        }

        [TestMethod]
        [DataRow(100, "100B")]
        [DataRow(1000, "1000B")]
        [DataRow(10000, "9,8KB")]
        [DataRow(100000, "97,7KB")]
        [DataRow(1000000, "976,6KB")]
        [DataRow(10000000, "9,5MB")]
        public void FileSizeToStringConverter_Test(long input, string expected)
        {
            var converter = new FileSizeToStringConverter();
            object result = converter.Convert(input, typeof(string), null, null);
            Assert.IsTrue(result.ToString() == expected);
        }

        [TestMethod]
        [DataRow(StatusEnum.None, "Keine Aktion")]
        [DataRow(StatusEnum.Success, "Fertig")]
        public void EnumDescriptionConverter_Test(StatusEnum input, string expected)
        {
            var converter = new EnumDescriptionConverter();
            object result = converter.Convert(input, typeof(string), null, null);
            Assert.IsTrue(result.ToString() == expected);
        }

        [TestMethod]
        [DataRow("28.06.1960", 59)]
        [DataRow("29.10.1960", 59)]
        [DataRow("01.05.1960", 60)]
        public void AgeConverter_Test(string input, int expected)
        {
            bool resultOk = DateTime.TryParse(input, out DateTime resultDate);

            var converter = new AgeConverter();
            object result = converter.Convert(resultDate, typeof(int), null, null);
            Assert.IsTrue((int)result == expected);
        }

        [TestMethod]
        [DataRow(true, "Ja")]
        [DataRow(false, "Nein")]
        public void BooleanToTextConverter_Test(bool input, string expected)
        {
            var converter = new BooleanToTextConverter();
            object result = converter.Convert(input, typeof(string), null, null);
            Assert.IsTrue(result.ToString() == expected);
        }

        public enum StatusEnum : int
        {
            [System.ComponentModel.Description("Keine Aktion")]
            None = 0,

            [System.ComponentModel.Description("Waiting to run")]
            Waiting = 1,

            [System.ComponentModel.Description("Run in progress")]
            Running = 2,

            [System.ComponentModel.Description("Paused by user")]
            Paused = 3,

            [System.ComponentModel.Description("Fertig")]
            Success = 4,

            [System.ComponentModel.Description("Run finished with error")]
            Failure = 5
        }
    }
}