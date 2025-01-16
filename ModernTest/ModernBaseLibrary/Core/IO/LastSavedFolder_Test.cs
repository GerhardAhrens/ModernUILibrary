namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LastSavedFolder_Test : BaseTest
    {
        private CultureInfo cultureInfo = null;

        [TestInitialize]
        public void OnTestInitialize()
        {
            if (UriParser.IsKnownScheme("pack") == false)
            {
                new System.Windows.Application();
            }

            cultureInfo = new CultureInfo("de-de");
        }

        [TestMethod]
        public void LoadFolderList()
        {
            var content = LastSavedFolder.ToDictionary();
            CollectionAssert.AllItemsAreNotNull(content);
        }

        [TestMethod]
        public void FolderListIsEmpty()
        {
            string content = LastSavedFolder.Get("Source");
            Assert.IsTrue(content == @"C:\\Users\\gerha\\Documents");
        }

        [TestMethod]
        public void SaveClearLoad_Test()
        {
            Type typ = typeof(ReleaseNote);

            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");
            LastSavedFolder.Save();

            LastSavedFolder.GetOrSet("Invention", @"C:\Temp\BlaBla");
            LastSavedFolder.Save();

            LastSavedFolder.Clear();

            LastSavedFolder.Load();
            int count = LastSavedFolder.Count;
        }

        [TestMethod]
        public void InitContent()
        {
            Type typ = typeof(ReleaseNote);

            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");
            LastSavedFolder.Save();

            LastSavedFolder.GetOrSet("Invention", @"C:\Temp\BlaBla");
            LastSavedFolder.Save();

            int count = LastSavedFolder.Count;

            Assert.AreEqual(count,2);
        }

        [TestMethod]
        public void FolderListGetEntryFalse()
        {
            string content = LastSavedFolder.Get("SourceXX");
            Assert.IsFalse(content == @"C:\_LCubeRepositories\");
        }

        [TestMethod]
        public void FolderListGetEntryDefault()
        {
            string defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string content = LastSavedFolder.Get("SourceXX");
            Assert.IsTrue(content == defaultFolder);
        }

        [TestMethod]
        public void FolderListRemoveEntry()
        {
            string typ = "SourceXX";
            LastSavedFolder.GetOrSet(typ, @"C:\_LCubeRepositories\");
            LastSavedFolder.Save();

            string content = LastSavedFolder.Get(typ);
            Assert.IsTrue(content == @"C:\_LCubeRepositories\");

            LastSavedFolder.Remove(typ);

            string defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string content1 = LastSavedFolder.Get(typ);
            Assert.IsTrue(content1 == defaultFolder);
        }

        [TestMethod]
        public void GetOrSetLastFolderFromTypXX()
        {
            Type typ = typeof(ReleaseNote);

            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");

            Dictionary<string,string> content = LastSavedFolder.ToDictionary();

            Assert.IsTrue(content[typ.GetFriendlyName()] == @"C:\Temp\");
        }

        [TestMethod]
        public void GetOrSetLastFolderFromString()
        {
            string typ = "ReleaseNote";

            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");

            Dictionary<string, string> content = LastSavedFolder.ToDictionary();

            Assert.IsTrue(content[typ] == @"C:\Temp\");
        }

        [TestMethod]
        public void CountInLastFolder()
        {
            string typ = "ReleaseNote";
            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");

            int count = LastSavedFolder.Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void ContentLastFolder()
        {
            string typ = "ReleaseNote";
            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");

            var content = LastSavedFolder.ToDictionary();
            CollectionAssert.AllItemsAreNotNull(content);
        }

        [TestMethod]
        public void LastFolderToSaveAndLoad()
        {
            string typ = "ReleaseNote";
            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");
            LastSavedFolder.Save();

            typ = "Rules";
            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");
            LastSavedFolder.Save();

            typ = "Inventions";
            LastSavedFolder.GetOrSet(typ, @"C:\Temp\");
            LastSavedFolder.Save();

            typ = "Source";
            LastSavedFolder.GetOrSet(typ, @"C:\_LCubeRepositories\");
            LastSavedFolder.Save();

            string filename = LastSavedFolder.Filename;
            Assert.IsTrue(File.Exists(filename));

            LastSavedFolder.Load();
            var content = LastSavedFolder.ToDictionary();
            CollectionAssert.AllItemsAreNotNull(content);
        }

        [TestMethod]
        public void LastFolderToDictionary()
        {
            LastSavedFolder.Load();
            Dictionary<string,string> content = LastSavedFolder.ToDictionary();
            CollectionAssert.AllItemsAreNotNull(content);
        }

        [TestMethod]
        public void LastFolderGetFolders()
        {
            LastSavedFolder.Load();
            List<string> content = LastSavedFolder.GetFolders();
            CollectionAssert.AllItemsAreNotNull(content);
        }

        [Serializable]
        private class ReleaseNote
        {
        }
    }
}
