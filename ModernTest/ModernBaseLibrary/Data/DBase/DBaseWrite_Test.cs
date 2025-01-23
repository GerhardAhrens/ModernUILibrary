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
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);
                byte[] data = ReadBytes(pathFileName);
                Assert.AreEqual(0x30, data[0], "Version should be 0x30.");
            }
        }

        [TestMethod]
        public void WriteOneField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
                dbf.Fields.Add(field);
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual("TEST", dbfRead.Fields[0].Name, "Field name should be TEST.");
            }
        }

        [TestMethod]
        public void WriteFieldAndRecord()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = "HELLO";
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual("HELLO", dbfRead.Records[0][0], "Record content should be HELLO.");
            }
        }

        [TestMethod]
        public void WriteFieldAndThreeRecords()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEXT", DbfFieldType.Character, 12);
                dbf.Fields.Add(field);
                field = new DbfField("INT", DbfFieldType.Integer, 4);
                dbf.Fields.Add(field);
                field = new DbfField("NUMERIC", DbfFieldType.Numeric, 12, 2);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = "HELLO-A";
                record.Data[1] = 100;
                record.Data[2] = 100.99;
                record = dbf.CreateRecord();
                record.Data[0] = "Hello-B";
                record.Data[1] = 101;
                record.Data[2] = 100.88;
                record = dbf.CreateRecord();
                record.Data[0] = "Hallo-C";
                record.Data[1] = 102;
                record.Data[2] = 100.77;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);
                Assert.AreEqual("Hallo-C", dbfRead.Records[2][0], "Record content should be OUT THERE.");
            }
        }

        [TestMethod]
        public void IntegerField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Integer, 4);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = 34;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(34, dbfRead.Records[0][0], "Record content should be 34.");
            }
        }

        [TestMethod]
        public void NumericField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Numeric, 12, 2);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = 3.14;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(3.14, dbfRead.Records[0][0], "Record content should be 3.14.");
            }
        }

        [TestMethod]
        public void FloatField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Float, 12,2);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = 3.14f;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(3.14f, dbfRead.Records[0][0], "Record content should be 3.14.");
            }
        }

        [TestMethod]
        public void LogicalField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Logical, 12);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = true;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(true, dbfRead.Records[0][0], "Record content should be TRUE.");
            }
        }

        [TestMethod]
        public void DateField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Date, 12);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = new DateTime(2025, 1, 23);
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(new DateTime(2025, 1, 23), dbfRead.Records[0][0], "Record content should be 2025-01-23.");
            }
        }

        [TestMethod]
        public void DateTimeField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.DateTime, 8);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = new DateTime(2025, 1, 23, 20, 00, 00);
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual(new DateTime(2025, 1, 23, 20, 00, 00), dbfRead.Records[0][0], "Record content should be 2025-01-23 20:00:00.");
            }
        }

        [TestMethod]
        public void CurrencyField()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\DBase\\DemoData\\dbaseWriteRead.dbf");

            using (Dbf dbf = new Dbf())
            {
                DbfField field = new DbfField("TEST", DbfFieldType.Currency, 4);
                dbf.Fields.Add(field);
                DbfRecord record = dbf.CreateRecord();
                record.Data[0] = 4.34F;
                dbf.Write(pathFileName, DbfVersion.FoxBaseDBase3NoMemo);

                Dbf dbfRead = new Dbf();
                dbfRead.Read(pathFileName);

                Assert.AreEqual((float)4.34, dbfRead.Records[0][0], "Record content should be 4.34.");
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