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
    public class DBase_NumericEncoder_Test : BaseTest
    {
        private readonly DbfField numericField = new DbfField("SUMMA", DbfFieldType.Numeric, 10, 2);
        private readonly DbfField integerField = new DbfField("NUM", DbfFieldType.Numeric, 10, 0);
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
        public void EncodeTestDecimalOverLength()
        {
            // Arrange.
            const decimal val = 13005200m;
            var expectedVal = new[] { '1', '3', '0', '0', '5', '2', '0', '0', '.', '0' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = NumericEncoder.Instance.Encode(numericField, val, encoding);

            // Assert.
            for (int i = 0; i < numericField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestDecimalOverPrecision()
        {
            // Arrange.
            const decimal val = 1300.5200m;
            var expectedVal = new[] { ' ', ' ', ' ', '1', '3', '0', '0', '.', '5', '2' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = NumericEncoder.Instance.Encode(numericField, val, encoding);

            // Assert.
            for (int i = 0; i < numericField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestDecimalUnderPrecision()
        {
            // Arrange.
            const decimal val = 1300m;
            var expectedVal = new[] { ' ', ' ', ' ', '1', '3', '0', '0', '.', '0', '0' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = NumericEncoder.Instance.Encode(numericField, val, encoding);

            // Assert.
            for (int i = 0; i < numericField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestDecimalNull()
        {
            // Arrange.
            decimal? val = null;
            string expectedVal = numericField.DefaultValue;

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = NumericEncoder.Instance.Encode(numericField, val, encoding);

            // Assert.
            for (int i = 0; i < numericField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestNumber()
        {
            // Arrange.
            const int val = 1;
            var expectedVal = new[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = NumericEncoder.Instance.Encode(integerField, val, encoding);

            // Assert.
            for (int i = 0; i < numericField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }
    }
}
