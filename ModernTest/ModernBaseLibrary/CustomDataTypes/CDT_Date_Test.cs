namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_Date_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void Date_CreateType()
        {
            Date date = new DateTime(1960, 6, 28);
            Assert.IsTrue(date.ToDateTime() == new DateTime(1960, 6, 28));
        }

        [TestMethod]
        public void Date_CreateTypeFromVar()
        {
            var dateVar = new DateTime(1960, 6, 28);
            Date date = dateVar;
            Assert.IsTrue(date.ToDateTime() == new DateTime(1960, 6, 28));
        }

        [TestMethod]
        public void Date_CreateTypeFromNow()
        {
            DateTime now = DateTime.Now;
            var dateVar = now;
            Date date = dateVar;
            Assert.IsTrue(date.ToDateTime() == now.Date);
        }

        [TestMethod]
        public void Date_ToString()
        {
            Date date = new DateTime(1960, 6, 28);
            string dateYear = date.ToString("yyyy");
            string dateDay = date.ToString("D");
            Assert.IsTrue(dateYear == "1960");
            Assert.IsTrue(dateDay == "Dienstag, 28. Juni 1960");
        }

        [TestMethod]
        public void Date_ToStringCulture()
        {
            Date date = new DateTime(1960, 6, 28);
            string dateYear = date.ToString("yyyy");
            string dateDay = date.ToString("D");
            Assert.IsTrue(dateYear == "1960");
            Assert.IsTrue(dateDay == "Dienstag, 28. Juni 1960");

            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Birthday date1 = new DateTime(1960, 6, 28);
            string dateYear1 = date.ToString("yyyy");
            string dateDay1 = date.ToString("D");
            Assert.IsTrue(dateYear1 == "1960");
            Assert.IsTrue(dateDay1 == "Tuesday, June 28, 1960");

        }

        [TestMethod]
        public void Date_GetType()
        {
            Date date = new DateTime(1960, 6, 28);
            Type typ = date.GetType();
            Assert.IsTrue(typ == typeof(Date));
        }

        [TestMethod]
        public void Date_ConvertType()
        {
            Date date = new DateTime(1960, 6, 28);
            Type typ = date.GetType();
            Assert.IsTrue(typ == typeof(Date));

            DateTime dt = date;
            Type typDt = dt.GetType();
            Assert.IsTrue(typDt == typeof(DateTime));
        }

        [TestMethod]
        public void Date_CompareType()
        {
            Date date1 = new DateTime(1960, 6, 28);
            Date date2 = new DateTime(1960, 6, 28);
            Assert.IsTrue(date1 == date2);

            Date date3 = new DateTime(1960, 6, 28);
            Date date4 = new DateTime(1961, 6, 28);
            Assert.IsFalse(date3 == date4);
        }

        [TestMethod]
        public void Date_Default()
        {
            DateTime date = Date.Default;
            Assert.IsTrue(date == new DateTime(1900,1,1));
        }

        [TestMethod]
        public void Date_IsDefault()
        {
            Date date = Date.Default;
            Assert.IsTrue(date.IsDefault);
        }

        [TestMethod]
        public void Date_InRangeOK()
        {
            Date dateStart = new DateTime(1960, 1, 1);
            Date date1 = new DateTime(1960, 6, 28);
            Date dateEnd = new DateTime(1960, 12, 31);
            Assert.IsTrue(date1.InRange(dateStart,dateEnd));
        }

        [TestMethod]
        public void Date_InRangeFail()
        {
            Date dateStart = new DateTime(1960, 1, 1);
            Date date1 = new DateTime(1961, 6, 28);
            Date dateEnd = new DateTime(1960, 12, 31);
            Assert.IsFalse(date1.InRange(dateStart, dateEnd));
        }

        [TestMethod]
        public void Date_InOK()
        {
            Date dateCurrent = new DateTime(1960, 6, 28);
            Date dateA = new DateTime(1960, 1, 1);
            Date dateB = new DateTime(1960, 12, 31);
            Date dateC = new DateTime(1960, 6, 28);

            bool result = dateCurrent.In(dateA,dateB,dateC);

            Assert.IsTrue(result);
        }
    }
}
