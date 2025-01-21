namespace ModernTest.ModernBaseLibrary.Core
{
    using System.Diagnostics;
    using System;
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Cryptography;
    using global::ModernBaseLibrary.LinqToFile;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using global::ModernBaseLibrary.Extension;
    using System.Globalization;
    using System.Threading;
    using global::ModernBaseLibrary.ExcelReader;
    using System.Data;
    using global::ModernBaseLibrary.DBase;

    [TestClass]
    public class DBase_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void DBase3WithMemo()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbase_83.dbf");
            if (File.Exists(pathFileName) == true)
            {
                using (Dbf dbf = new Dbf())
                {
                    dbf.Read(pathFileName);
                    Assert.AreEqual(67, dbf.Records.Count, "Read 67 Datensätze");
                    Assert.AreEqual(15, dbf.Fields.Count, "Read 15 Columns");
                }
            }
        }

        [TestMethod]
        public void Dbase3WithMemoStreamRead()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileNameDBF = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbase_83.dbf");
            string pathFileNameDBT = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbase_83.dbt");
            if (File.Exists(pathFileNameDBF) == true)
            {
                using (Dbf dbf = new Dbf())
                {
                    using (FileStream baseStream = File.Open(pathFileNameDBF, FileMode.Open, FileAccess.Read))
                    using (FileStream memoStream = File.Open(pathFileNameDBT, FileMode.Open, FileAccess.Read))

                    dbf.Read(baseStream, memoStream);

                    Assert.AreEqual(67, dbf.Records.Count, "Read 67 Datensätze");
                    Assert.AreEqual(15, dbf.Fields.Count, "Read 15 Columns");
                }
            }
        }

        [TestMethod]
        public void DBase3WithoutMemo()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbase_03.dbf");
            if (File.Exists(pathFileName) == true)
            {
                using (Dbf dbf = new Dbf())
                {
                    dbf.Read(pathFileName);
                    Assert.AreEqual(14, dbf.Records.Count, "Read 14 Datensätze");
                    Assert.AreEqual(31, dbf.Fields.Count, "Read 31 Columns");
                    Assert.AreEqual("Point_ID", dbf.Fields[0].Name, "First field name should be 'Point_ID'");
                    Assert.AreEqual(12, dbf.Fields[0].Length, "Point_ID field length must be 12.");
                    Assert.AreEqual(3, dbf.Fields[23].Precision, "GPS_Second field length must be 3.");
                }
            }
        }
    }
}