namespace ModernTest.ModernUILibrary.WPF
{
    using System;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernIU.Converters;

    [TestClass]
    public class DateTimeToHumanConverter_Test
    {
        [TestMethod]
        public void DateTimeToHumanConverter_Now()
        {
            DateTime input1 = DateTime.Now;
            using (DateTimeToHumanConverter d = new DateTimeToHumanConverter())
            {
                string result1 = d.Get(input1);
            }
        }

        [TestMethod]
        public void DateTimeToHumanConverter_Years()
        {
            DateTime input1 = new DateTime(1960, 6, 28);
            using(DateTimeToHumanConverter d = new DateTimeToHumanConverter())
            {
                string result1 = d.Get(input1);
            }
        }

        [TestMethod]
        public void ToStringAsHuman()
        {
            string result = DateTime.Now.ToStringAsHuman();
            Assert.IsTrue(result == "Jetzt");

            string result1 = new DateTime(1960, 6, 28).ToStringAsHuman();
            Assert.IsTrue(result1 == "vor 59 Jahre");

            string result11 = new DateTime(2019, 7, 23, 14, 19, 0).ToStringAsHuman();
            Assert.IsTrue(result11 == "Vor über 23 Minuten");

            string result2 = new DateTime(2019, 7, 22, 0, 19, 0).ToStringAsHuman();
            Assert.IsTrue(result2 == "Gestern");

            string result3 = new DateTime(2019, 7, 22, 19, 1, 0).ToStringAsHuman();
            Assert.IsTrue(result3 == "Vor über 19 Stunden");
        }
    }
}
