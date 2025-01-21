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

    [TestClass]
    public class ExcelReader_Test : BaseTest
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
        public void ReadExcel_XLSX()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelReader\\DemoData\\Mengengerüst PPV Funktionen.xlsx");
            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);

                FileStream stream = File.Open(pathFileName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataTable table = excelReader.AsDataTable("Aufgaben");
                        if (table != null)
                        {
                            base.Trace($"Name: {table.TableName}; Anzahl Zeilen: {table.Rows.Count}");
                        }
                    }

                }
            }
        }

        [TestMethod]
        public void ReadExcel_XLSX_DataSet()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelReader\\DemoData\\Mengengerüst PPV Funktionen.xlsx");
            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);

                FileStream stream = File.Open(pathFileName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataSet result = excelReader.AsDataSet();
                        if (result != null)
                        {
                            foreach (DataTable table in result.Tables)
                            {
                                Trace($"Name: {table.TableName}; Anzahl Zeilen: {table.Rows.Count}");
                            }
                        }
                    }

                }
            }
        }

        [TestMethod]
        public void ReadExcel_XLSX_Cell()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelReader\\DemoData\\Mengengerüst PPV Funktionen.xlsx");
            if (File.Exists(pathFileName) == true)
            {
                FileStream stream = File.Open(pathFileName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;

                        int row = 0;
                        while (excelReader.Read())
                        {
                            row++;
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                var columnA = excelReader.GetString(i);
                                base.Trace($"Zeile/Spalte: {row}/{i}; Inhalt: {columnA}");
                            }

                            base.Trace("****************************************");
                        }
                    }

                }
            }
        }


        [TestMethod]
        public void ReadExcel_XLS()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelReader\\DemoData\\Mengengerüst PPV Funktionen.xls");

            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);

                FileStream stream = File.Open(pathFileName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataTable table = excelReader.AsDataTable("Aufgaben");
                        if (table != null)
                        {
                            base.Trace($"Name: {table.TableName}; Anzahl Zeilen: {table.Rows.Count}");
                        }
                    }

                }
            }
        }

        [TestMethod]
        public void ReadExcel_XLS_Cell()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\ExcelReader\\DemoData\\Mengengerüst PPV Funktionen.xls");

            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);

                FileStream stream = File.Open(pathFileName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;

                        int row = 0;
                        while (excelReader.Read())
                        {
                            row++;
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                var columnA = excelReader.GetString(i);
                                base.Trace($"Zeile/Spalte: {row}/{i}; Inhalt: {columnA}");
                            }

                            base.Trace("****************************************");
                        }
                    }
                }
            }
        }
    }
}