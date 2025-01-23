namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    using global::ModernBaseLibrary.DBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DBase_DateEncoder_Test : BaseTest
    {
        private readonly DbfField dateField = new DbfField("DATADOC", DbfFieldType.Date, 8);
        private readonly Encoding encoding = Encoding.ASCII;
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void EncodeTestDateValid()
        {
            // Arrange.
            var val = new DateTime(1939, 09, 01);
            var expectedVal = new[] { '1', '9', '3', '9', '0', '9', '0', '1' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = DateEncoder.Instance.Encode(dateField, val, encoding);

            // Assert.
            for (int i = 0; i < dateField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestDateNull()
        {
            // Arrange.
            DateTime? val = null;
            string expectedVal = dateField.DefaultValue;

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = DateEncoder.Instance.Encode(dateField, val, encoding);

            // Assert.
            for (int i = 0; i < dateField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }
    }
}
