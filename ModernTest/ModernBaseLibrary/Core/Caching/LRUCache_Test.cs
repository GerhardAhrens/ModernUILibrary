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
    public class LRUCache_Test
    {
        private LRUCache<string, string> lruCache = null;


        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            lruCache = new LRUCache<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCache_test"/> class.
        /// </summary>
        public LRUCache_Test()
        {
        }

        [TestMethod]
        public void CreateCache()
        {
            lruCache.Set("Test", "Gerhard");
        }

        [TestMethod]
        public void GetCache()
        {
            lruCache.AddOrUpdate("Test", CacheContent);
        }

        private string CacheContent()
        {
            return "Charlie";
        }

        [TestMethod]
        public void GetCacheWithIndex()
        {
            lruCache.Set("Test", "Gerhard");
            string result = lruCache["Test"];
        }

        [TestMethod]
        public void GetAllCacheValues()
        {
            lruCache.Set("Test", "Gerhard");
            lruCache.Set("Test-1", "Charlie");
            IEnumerable<string> result = lruCache.GetAllValues();
        }
    }
}
