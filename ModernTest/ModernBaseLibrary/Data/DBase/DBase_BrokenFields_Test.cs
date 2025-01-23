namespace ModernTest.ModernBaseLibrary.Core
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using global::ModernBaseLibrary.DBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DBase_BrokenFields_Test : BaseTest
    {
        private readonly List<DbfField> fields;
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");

        public DBase_BrokenFields_Test()
        {
            fields = new List<DbfField>
            {
                new DbfField("NUM", DbfFieldType.Numeric, 6),
                new DbfField("CODE", DbfFieldType.Character, 15),
                new DbfField("F", DbfFieldType.Character, 30),
                new DbfField("I", DbfFieldType.Character, 30),
                new DbfField("O", DbfFieldType.Character, 30),
                new DbfField("DOC", DbfFieldType.Character, 100),
                new DbfField("SERIES", DbfFieldType.Character, 7),
                new DbfField("NUMBER", DbfFieldType.Character, 7),
                new DbfField("DATAISSUE", DbfFieldType.Character, 200),
                new DbfField("ZIP", DbfFieldType.Numeric, 6),
                new DbfField("ADDRESS", DbfFieldType.Character, 200),
                new DbfField("SNILS", DbfFieldType.Character, 11),
                new DbfField("SUM", DbfFieldType.Numeric, 12, 2),
                new DbfField("MONTH", DbfFieldType.Character, 15),
                new DbfField("YEAR", DbfFieldType.Numeric, 4),
                new DbfField("NOTE", DbfFieldType.Character, 255),
                new DbfField("REASON", DbfFieldType.Character, 128),
                new DbfField("PERS_NUM", DbfFieldType.Character, 14)
            };
        }

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
        public void ReadBrokenFieldsTest()
        {
            // Arrange.
            var dbf = ReadBrokenFields();

            // Assert.
            Assert.AreEqual(fields.Count, dbf.Fields.Count);
            for (int i = 0; i < fields.Count; i++)
            {
                Assert.AreEqual(fields[i], dbf.Fields[i]);
                Trace($"Field: {dbf.Fields[i]}");
            }
        }

        private Dbf ReadBrokenFields()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\POST986.dbf");

            var dbf = new Dbf();
            dbf.Read(pathFileName);

            return dbf;
        }
    }
}
