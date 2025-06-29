//-----------------------------------------------------------------------
// <copyright file="HeapMinMax_Test.cs" company="Lifeprojects.de">
//     Class: HeapMinMax_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.06.2025 19:17:02</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class HeapMinMax_Test
    {
        private int[] rawItems = new[] { 45, 47, 19, 22, 117, 94, 58, 145, 1229, 14 };

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapMinMax_Test"/> class.
        /// </summary>
        public HeapMinMax_Test()
        {
        }

        [TestMethod]
        public void MinHeap_Test()
        {
            MinHeap<int> minHeap = new MinHeap<int>();

            foreach (var item in rawItems)
            {
                minHeap.Enqueue(item);
            }

            Assert.AreEqual(10, minHeap.Count);
            Assert.AreEqual(14, minHeap.Peek());
        }

        [TestMethod]
        public void MaxHeap_Test()
        {
            MaxHeap<int> minHeap = new MaxHeap<int>();

            foreach (var item in rawItems)
            {
                minHeap.Enqueue(item);
            }

            Assert.AreEqual(10, minHeap.Count);
            Assert.AreEqual(1229, minHeap.Peek());
        }

        [TestMethod]
        public void AnyHeap_Test()
        {
            MaxHeap<int> maxHeap = new MaxHeap<int>();
            foreach (var item in rawItems)
            {
                maxHeap.Enqueue(item);
            }

            while (maxHeap.Any())
            {
                var item = maxHeap.Dequeue();
            }

            Assert.AreEqual(0, maxHeap.Count);
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
