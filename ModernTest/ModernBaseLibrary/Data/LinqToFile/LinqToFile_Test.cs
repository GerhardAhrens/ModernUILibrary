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

    [TestClass]
    public class LinqToFile_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
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
        public void BuildFlatFile()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\LinqToFile\\DemoData\\Book1.txt");

            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);
                List<FlatTable> fileContent = null;

                using (FileQuery<FlatTable> fq = new FileQuery<FlatTable>(pathFileName))
                {
                    fileContent = fq.Content.Where(i => i.Field2 <= 10 && i.Field2 >= 0).ToList();
                }

                foreach (FlatTable item in fileContent)
                {
                    base.Trace(string.Format("{0},{1},{2}", item.Field1, item.Field2, item.DateField));
                }
            }
        }

        [TestMethod]
        public void BuildCsvFile()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\LinqToFile\\DemoData\\Book1.csv");

            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);
                List<CsvTable> fileContent = null;

                using (FileQuery<CsvTable> fq = new FileQuery<CsvTable>(pathFileName))
                {
                    fileContent = fq.Content.Where(i => i.Field2 <= 10 && i.Field2 >= 0).ToList();
                }

                foreach (CsvTable item in fileContent)
                {
                    base.Trace(string.Format("{0},{1},{2}", item.Field1, item.Field2, item.DateField));
                }
            }
        }

        [TestMethod]
        public void ReadFileWithLINQ()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\LinqToFile\\DemoData\\LINQDemo.txt");

            if (File.Exists(pathFileName) == true)
            {
                base.Trace(pathFileName);

                File.OpenText(pathFileName).Use(stream =>
                {
                    stream
                        .ReadToEnd()
                        .Split(' ')
                        .Convert(str => str.Trim())
                        .GetCounts((x, y) => x == y)
                        .OrderBy(f => f.Key)
                        //.CountSum(p => Console.WriteLine(String.Format("count: {0}", p)));
                        .ForEach(kvp => base.Trace(String.Format("{0} count: {1}",kvp.Key, kvp.Value.ToString())));
                });
            }
        }
    }

    [DebuggerDisplay("Field1 = {Field1}, Field2 = {Field2}, DateField = {DateField}")]
    [FlatFile]
    internal class FlatTable
    {
        [FlatFileField(0, 4)]
        public string Field1 { set; get; }

        [FlatFileField(4, 3, NegativeSign = "n", PositiveSign = "p")]
        public int Field2 { set; get; }

        [FlatFileField(7, 8, DateTimeFormat = "yyyyMMdd")]
        public DateTime DateField { get; set; }
    }

    [DebuggerDisplay("Field1 = {Field1}, Field2 = {Field2}, DateField = {DateField}")]
    [CsvFile(true, '"', ';')]
    internal class CsvTable
    {
        [CsvFileField]
        public string Field1 { set; get; }

        [CsvFileField]
        public int Field2 { set; get; }

        [CsvFileField("Field3", DateTimeFormat = "yyyyMMdd")]
        public DateTime DateField { get; set; }
    }
}