namespace ModernTest.ModernBaseLibrary.Core
{
    using System;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DateTimeSpan_Test
    {
        [TestMethod]
        public void CompareDates()
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(1960, 6, 28), new DateTime(2019, 6, 21));
            Assert.IsTrue(compare.Years == 58);
        }

        [TestMethod]
        public void CompareTimes()
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(2019, 8, 16, 10, 0, 0), new DateTime(2019, 8, 16, 10, 1, 12));
            Assert.IsTrue(compare.Minutes == 1);
            Assert.IsTrue(compare.Seconds == 12);
        }

        [TestMethod]
        [DataRow(72)]
        public void CompareAsSeconds_01(double expected)
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(2019, 8, 16, 10, 0, 0), new DateTime(2019, 8, 16, 10, 1, 12));
            Assert.IsTrue(compare.AsSeconds == expected);
        }

        [TestMethod]
        [DataRow(3661)]
        public void CompareAsSeconds_02(double expected)
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(2019, 8, 16, 10, 0, 0), new DateTime(2019, 8, 16, 11, 1, 1));
            Assert.IsTrue(compare.AsSeconds == expected);
        }

        [TestMethod]
        [DataRow(86400)]
        public void CompareAsSeconds_03(double expected)
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(2019, 8, 16, 0, 0, 0), new DateTime(2019, 8, 17, 0, 0, 0));
            Assert.IsTrue(compare.AsSeconds == expected);
        }

        [TestMethod]
        [DataRow(90071)]
        public void CompareAsSeconds_04(double expected)
        {
            DateTimeSpan compare = DateTimeSpan.CompareDates(new DateTime(2019, 8, 16, 0, 0, 0), new DateTime(2019, 8, 17, 1, 1, 11));
            Assert.IsTrue(compare.AsSeconds == expected);
        }
    }
}
