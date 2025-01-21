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
    public class DBaseWrite_Test : BaseTest
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
        public void WriteNoData()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWrite.dbf");

            using (Dbf dbf = new Dbf())
            {
                dbf.Write(pathFileName, DbfVersion.VisualFoxPro);
                byte[] data = ReadBytes(pathFileName);
                Assert.AreEqual(0x30, data[0], "Version should be 0x30.");
            }
        }

        [TestMethod]
        public void WriteOneField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseReadWriteA.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
                dbf.Fields.Add(field);
                dbf.Write(pathFileName, DbfVersion.VisualFoxPro);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual("TEST", dbfRead.Fields[0].Name, "Field name should be TEST.");
            }
        }

        [TestMethod]
        public void WriteFieldAndRecord()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseReadWriteB.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = "HELLO";
                dbf.Write(pathFileName, DbfVersion.VisualFoxPro);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual("HELLO", dbf.Records[0][0], "Record content should be HELLO.");
            }
        }


        private byte[] ReadBytes(string fileName)
        {
            // Open stream for reading.
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            byte[] data = new byte[stream.Length];
            data = reader.ReadBytes((int)stream.Length);
            reader.Close();
            stream.Close();
            return data;
        }
    }
}