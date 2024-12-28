//-----------------------------------------------------------------------
// <copyright file="ComparisonComparer_Test.cs" company="Lifeprojects.de">
//     Class: ComparisonComparer_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>17.05.2022 12:59:05</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ComparisonComparer_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonComparer_Test"/> class.
        /// </summary>
        public ComparisonComparer_Test()
        {
        }

        [TestMethod]
        public void CreateAndCall()
        {
            var example = new List<string> { "c", "b", "a", "d", "foo", "", "1", "e" };

            IComparer<string> comparer = new ComparisonComparer<string>(this.StringComparison);
            example.Sort(comparer);

        }

        private int StringComparison(string x, string y)
        {
            int ix = x == "b" ? 0 : x == "c" ? 1 : 2;
            int iy = y == "b" ? 0 : y == "c" ? 1 : 2;
            return ix.CompareTo(iy);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ConstructionWithNull()
        {
            try
            {
                new ComparisonComparer<string>(null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void CreateComparisonWithNull()
        {
            try
            {
                ComparisonComparer<string>.CreateComparison(null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }
    }
}
