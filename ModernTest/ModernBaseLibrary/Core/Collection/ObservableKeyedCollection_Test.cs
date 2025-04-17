//-----------------------------------------------------------------------
// <copyright file="ObservableKeyedCollection_Test.cs" company="Lifeprojects.de">
//     Class: ObservableKeyedCollection_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>17.04.2025 08:19:34</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ObservableKeyedCollection_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableKeyedCollection_Test"/> class.
        /// </summary>
        public ObservableKeyedCollection_Test()
        {
        }

        [TestMethod]
        public void Add()
        {
            var coll = new TestKeyedCollection();
            coll.Add(3);
            Assert.AreEqual(3, coll[3]);
            coll.Add(5);
            Assert.AreEqual(5, coll[5]);
        }

        [TestMethod]
        public void AddDuplicate()
        {
            try
            {
                var coll = new TestKeyedCollection();
                coll.Add(3);
                coll.Add(3);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void Remove()
        {
            var coll = new TestKeyedCollection();
            coll.Add(3);
            coll.Add(1);

            bool success = coll.Remove(3);
            Assert.IsTrue(success);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(1, coll[1]);
        }

        [TestMethod]
        public void RemoveOutOfRange()
        {
            var coll = new TestKeyedCollection();
            coll.Add(1);
            coll.Add(2);
            bool success = coll.Remove(0);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddRange()
        {
            var coll = new TestKeyedCollection();
            coll.AddRange(new[]
            {
                10,
                6,
                1090,
                -3009,
            });
            Assert.AreEqual(4, coll.Count);
            Assert.AreEqual(10, coll[10]);
            Assert.AreEqual(6, coll[6]);
            Assert.AreEqual(1090, coll[1090]);
            Assert.AreEqual(-3009, coll[-3009]);
        }

        [TestMethod]
        public void AddRangeDuplicate()
        {
            var coll = new TestKeyedCollection();

            try
            {
                coll.AddRange(new[]
                {
                10,
                6,
                -3009,
                6,
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
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.Add(9);
            Assert.AreEqual(1, collectionChangedCount);
        }

        [TestMethod]
        public void CollectionChangeRemove()
        {
            int collectionChangedCount = 0;
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.Add(8);
            coll.Add(6);

            collectionChangedCount = 0;
            coll.Remove(8);
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            coll.Remove(8);
            Assert.AreEqual(0, collectionChangedCount);
        }

        [TestMethod]
        public void CollectionChangeAddRange()
        {
            int collectionChangedCount = 0;
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.AddRange(new[]
            {
                4,
                10,
            });
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            coll.AddRange(new int[0]);
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

        private class TestKeyedCollection : ObservableKeyedCollection<int, int>
        {
            protected override int GetKeyForItem(int item)
            {
                return item;
            }
        }
    }
}
