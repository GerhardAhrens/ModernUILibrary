//-----------------------------------------------------------------------
// <copyright file="HeapsAlgorithm_Test.cs" company="www.pta.de">
//     Class: HeapsAlgorithm_Test
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>11.12.2024 13:26:33</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core.Algorithm;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class HeapsAlgorithm_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapsAlgorithm_Test"/> class.
        /// </summary>
        public HeapsAlgorithm_Test()
        {
        }

        [TestMethod]
        public void HeapsChar_Test()
        {
            char[] tasks = { 'A', 'B', 'C' };
            List<char[]> permutations = new List<char[]>();

            HeapsAlgorithm.GeneratePermutations(tasks, tasks.Length, permutations);

            Assert.AreEqual(permutations.Count, 6);
            Assert.AreEqual(string.Join(", ", permutations[0]), "A, B, C");
            Assert.AreEqual(string.Join(", ", permutations[1]), "B, A, C");
            Assert.AreEqual(string.Join(", ", permutations[2]), "C, A, B");
            Assert.AreEqual(string.Join(", ", permutations[3]), "A, C, B");
            Assert.AreEqual(string.Join(", ", permutations[4]), "B, C, A");
            Assert.AreEqual(string.Join(", ", permutations[5]), "C, B, A");
        }

        [TestMethod]
        public void HeapsInt_Test()
        {
            int[] tasks = { 1, 2, 3 };
            List<int[]> permutations = new List<int[]>();

            HeapsAlgorithm.GeneratePermutations(tasks, tasks.Length, permutations);

            Assert.AreEqual(permutations.Count, 6);
            Assert.AreEqual(string.Join(", ", permutations[0]), "1, 2, 3");
            Assert.AreEqual(string.Join(", ", permutations[1]), "2, 1, 3");
            Assert.AreEqual(string.Join(", ", permutations[2]), "3, 1, 2");
            Assert.AreEqual(string.Join(", ", permutations[3]), "1, 3, 2");
            Assert.AreEqual(string.Join(", ", permutations[4]), "2, 3, 1");
            Assert.AreEqual(string.Join(", ", permutations[5]), "3, 2, 1");
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
