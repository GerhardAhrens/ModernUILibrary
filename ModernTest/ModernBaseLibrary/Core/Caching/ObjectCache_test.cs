//-----------------------------------------------------------------------
// <copyright file="ObjectCache_Test.cs" company="Lifeprojects.de">
//     Class: ObjectCache_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.12.2022 11:26:08</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ObjectCache_Test
    {
        private Cache<List<string>> cachingList = null;


        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            cachingList = new Cache<List<string>>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCache_test"/> class.
        /// </summary>
        public ObjectCache_Test()
        {
        }

        [TestMethod]
        public void CreateCache()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            int cacheCount = cachingList.Count();
            Assert.IsTrue(cacheCount == 1);
        }

        [TestMethod]
        public void GetCache()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            List<string> fromCache = cachingList.Get("Liste1");
            Assert.IsNotNull(fromCache);
            Assert.IsTrue(fromCache.Count == 8);
        }

        [TestMethod]
        public void GetOfTCache()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            List<string> fromCache = cachingList.Get<List<string>>("Liste1");
            Assert.IsNotNull(fromCache);
            Assert.IsTrue(fromCache.Count == 8);
        }

        [TestMethod]
        public void GetKeys()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            IEnumerable<string> fromCache = cachingList.GetKeys();
            Assert.IsNotNull(fromCache);
            Assert.IsTrue(fromCache.Count() == 1);
        }

        [TestMethod]
        public void GetTyps()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            IEnumerable<string> fromCache = cachingList.GetTyps();
            Assert.IsNotNull(fromCache);
            Assert.IsTrue(fromCache.Count() == 1);
        }

        [TestMethod]
        public void RemoveCache()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString());
            cachingList.Remove("Liste1");
            List<string> fromCache = cachingList.Get("Liste1");
            Assert.IsNull(fromCache);
        }

        [TestMethod]
        public void CacheWithTime()
        {
            cachingList.AddOrUpdate("Liste1", this.ListeString(),5);

            Thread.Sleep(7000);

            List<string> fromCache = cachingList.Get("Liste1");
            Assert.IsNull(fromCache);

            if (fromCache == null)
            {
                cachingList.AddOrUpdate("Liste1", this.ListeString("new"));
            }

            List<string> fromCacheNew = cachingList.Get("Liste1");

            Assert.IsNotNull(fromCacheNew);
            Assert.IsTrue(fromCacheNew.Count == 5);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        private List<string> ListeString(string text = "")
        {
            List<string> liste = null;

            if (text == "null")
            {
                liste = null;
            }
            else if (text == "new")
            {
                liste = new List<string>() { "Gerhard", "Ahrens", "Mannheim", "Lifeprojects.de", "GmbH" };
            }
            else if (text == string.Empty)
            {
                liste = new List<string>() { "Gerhard", "Ahrens", "Mannheim", "Lifeprojects.de", "GmbH", "Maus", "Hund", "Katz" };
            }

            return liste;
        }
    }
}
