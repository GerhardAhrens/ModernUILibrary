namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class DateTimeHelper_Test : BaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void WeekNumber_NowDate()
        {
            int weekNr = DateTimeHelper.CalendarWeek(new DateTime(2025,9,3));
            Assert.AreEqual(36, weekNr);
        }

        [TestMethod]
        public void TotalWeeksThisYear_NowDate()
        {
            int totalWeeks = DateTimeHelper.TotalWeeksThisYear();
            Assert.AreEqual(52, totalWeeks);
        }
    }
}