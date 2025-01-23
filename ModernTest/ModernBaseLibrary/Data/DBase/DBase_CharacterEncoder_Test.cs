namespace ModernTest.ModernBaseLibrary.Core
{
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    using global::ModernBaseLibrary.DBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DBase_CharacterEncoder_Test : BaseTest
    {
        private readonly DbfField characterField = new DbfField("NOMDOC", DbfFieldType.Character, 10);
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
        public void EncodeTestStringShort()
        {
            // Arrange.
            const string val = "010112.01";
            var expectedVal = new[] { '0', '1', '0', '1', '1', '2', '.', '0', '1', ' ' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = CharacterEncoder.Instance.Encode(characterField, val, encoding);

            // Assert.
            for (int i = 0; i < characterField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestStringLong()
        {
            // Arrange.
            const string val = "04072016.280";
            var expectedVal = new[] { '0', '4', '0', '7', '2', '0', '1', '6', '.', '2' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = CharacterEncoder.Instance.Encode(characterField, val, encoding);

            // Assert.
            for (int i = 0; i < characterField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [TestMethod]
        public void EncodeTestStringNull()
        {
            // Arrange.
            const string val = null;
            string expectedVal = characterField.DefaultValue;

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = CharacterEncoder.Instance.Encode(characterField, val, encoding);

            // Assert.
            for (int i = 0; i < characterField.Length; i++)
            {
                Assert.AreEqual(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }
    }
}
