namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_Birthday_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void Birthday_CreateType()
        {
            Birthday birthday = new DateTime(1960, 6, 28);
            Assert.IsTrue(birthday.ToDateTime() == new DateTime(1960, 6, 28));
        }

        [TestMethod]
        public void Birthday_CreateTypeFromVar()
        {
            DateTime birthdayVar = new DateTime(1960, 6, 28);
            Birthday birthday = birthdayVar;
            Assert.IsTrue(birthday.ToDateTime() == new DateTime(1960, 6, 28));
        }

        [TestMethod]
        public void Birthday_ToString()
        {
            Birthday birthday = new DateTime(1960, 6, 28);
            string birthdayYear = birthday.ToString("yyyy");
            string birthdayD = birthday.ToString("D");
            Assert.IsTrue(birthdayYear == "1960");
            Assert.IsTrue(birthdayD == "Dienstag, 28. Juni 1960");
        }

        [TestMethod]
        public void Birthday_ToStringCulture()
        {
            Birthday birthday = new DateTime(1960, 6, 28);
            string birthdayYear = birthday.ToString("yyyy");
            string birthdayD = birthday.ToString("D");
            Assert.IsTrue(birthdayYear == "1960");
            Assert.IsTrue(birthdayD == "Dienstag, 28. Juni 1960");

            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Birthday birthday1 = new DateTime(1960, 6, 28);
            string birthdayYear1 = birthday.ToString("yyyy");
            string birthdayD1 = birthday.ToString("D");
            Assert.IsTrue(birthdayYear1 == "1960");
            Assert.IsTrue(birthdayD1 == "Tuesday, June 28, 1960");

        }

        [TestMethod]
        public void Birthday_GetType()
        {
            Birthday birthday = new DateTime(1960, 6, 28);
            Type typ = birthday.GetType();
            Assert.IsTrue(typ == typeof(Birthday));
        }

        [TestMethod]
        public void Birthday_CompareType()
        {
            Birthday birthday1 = new DateTime(1960, 6, 28);
            Birthday birthday2 = new DateTime(1960, 6, 28);
            Assert.IsTrue(birthday1 == birthday2);

            Birthday birthday3 = new DateTime(1960, 6, 28);
            Birthday birthday4 = new DateTime(1961, 6, 28);
            Assert.IsFalse(birthday3 == birthday4);
        }
        [TestMethod]
        public void Birthday_GetAge()
        {
            Birthday birthday = new DateTime(1960, 6, 28);
            int ageYear = birthday.AgeInYear();
            int ageDays = birthday.AgeInDays();
            Assert.IsTrue(ageYear == 64);
            Assert.IsTrue(ageDays == 23583);
        }
    }
}
