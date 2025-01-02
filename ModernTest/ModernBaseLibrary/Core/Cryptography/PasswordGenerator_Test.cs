namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System.Reflection;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Globalization;
    using System.Threading;
    using ModernIU.Controls;
    using System;

    [TestClass]
    public class PasswordGenerator_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordGenerator_Test"/> class.
        /// </summary>
        public PasswordGenerator_Test()
        {
        }

        [TestMethod]
        public void PasswordStrength_Weak()
        {
            const string pw = "1234567890";
            PasswordStrengthResult pwResult = null;
            using (PasswordGenerator pg = new PasswordGenerator())
            {
                pwResult = pg.PasswordStrength(pw);
            }

            Assert.AreEqual(pwResult.Length, 10);
            Assert.AreEqual(pwResult.Value, "Schwach");
        }

        [TestMethod]
        public void PasswordStrength_Medium()
        {
            const string pw = "4711Gerhard";
            PasswordStrengthResult pwResult = null;
            using (PasswordGenerator pg = new PasswordGenerator())
            {
                pwResult = pg.PasswordStrength(pw);
            }

            Assert.AreEqual(pwResult.Length, 11);
            Assert.AreEqual(pwResult.Value, "Mittel");
        }

        [TestMethod]
        public void PasswordStrength_Strong()
        {
            const string pw = "4711+Gerhard";
            PasswordStrengthResult pwResult = null;
            using (PasswordGenerator pg = new PasswordGenerator())
            {
                pwResult = pg.PasswordStrength(pw);
            }

            Assert.AreEqual(pwResult.Length, 12);
            Assert.AreEqual(pwResult.Value, "Stark");
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}