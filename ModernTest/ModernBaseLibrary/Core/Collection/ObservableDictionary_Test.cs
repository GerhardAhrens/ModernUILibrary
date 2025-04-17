//-----------------------------------------------------------------------
// <copyright file="ObservableDictionary_Test.cs" company="Lifeprojects.de">
//     Class: ObservableDictionary_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>17.04.2025 08:37:30</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Collection;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ObservableDictionary_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary_Test"/> class.
        /// </summary>
        public ObservableDictionary_Test()
        {
        }

        [TestMethod]
        public void Add()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(3, 1);
            Assert.AreEqual(1, dic[3]);
            dic.Add(5, 80);
            Assert.AreEqual(80, dic[5]);
        }

        [TestMethod]
        public void AddDuplicate()
        {
            var dic = new ObservableDictionary<int, int>();
            try
            {
                dic.Add(3, 1);
                dic.Add(3, 80);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }

        }

        [TestMethod]
        public void Set()
        {
            var dic = new ObservableDictionary<int, int>();
            dic[3] = 1;
            Assert.AreEqual(1, dic[3]);
            dic[3] = -6;
            Assert.AreEqual(-6, dic[3]);
        }

        [TestMethod]
        public void Remove()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(3, 6);
            dic.Add(1, 3);

            bool success = dic.Remove(3);
            Assert.IsTrue(success);
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(3, dic[1]);
        }

        [TestMethod]
        public void RemoveOutOfRange()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(1, 1);
            dic.Add(2, 2);
            bool success = dic.Remove(0);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddRange()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.AddRange(new Dictionary<int, int>
            {
                { 3, 10 },
                { 5, 6 },
                { 1090, 10 },
                { -3009, 44 },
            });
            Assert.AreEqual(4, dic.Count);
            Assert.AreEqual(10, dic[3]);
            Assert.AreEqual(6, dic[5]);
            Assert.AreEqual(10, dic[1090]);
            Assert.AreEqual(44, dic[-3009]);
        }

        [TestMethod]
        public void AddRangeDuplicate()
        {
            var dic = new ObservableDictionary<int, int>();

            try
            {
                dic.AddRange(new Dictionary<int, int>
            {
                { 3, 10 },
                { 5, 6 },
                { 3, 11 },
                { -3009, 44 },
            });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void CollectionChangeAdd()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.Add(3, 9);
            Assert.AreEqual(1, collectionChangedCount);
        }

        [TestMethod]
        public void CollectionChangeSet()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic[3] = 9;
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic[3] = 10;
            Assert.AreEqual(2, collectionChangedCount);

            collectionChangedCount = 0;
            dic[3] = 10;
            Assert.AreEqual(0, collectionChangedCount);
        }

        [TestMethod]
        public void CollectionChangeRemove()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.Add(3, 8);
            dic.Add(5, 8);

            collectionChangedCount = 0;
            dic.Remove(3);
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic.Remove(3);
            Assert.AreEqual(0, collectionChangedCount);
        }

        [TestMethod]
        public void CollectionChangeAddRange()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.AddRange(new Dictionary<int, int>
            {
                { 4, 8 },
                { 5, 10 },
            });
            Assert.AreEqual(2, collectionChangedCount);

            collectionChangedCount = 0;
            dic.AddRange(new Dictionary<int, int>());
            Assert.AreEqual(0, collectionChangedCount);
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
    }
}
