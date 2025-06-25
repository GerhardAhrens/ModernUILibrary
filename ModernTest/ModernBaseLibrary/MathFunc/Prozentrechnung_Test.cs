/*
 * <copyright file="Prozentrechnung_Test.cs" company="Lifeprojects.de">
 *     Class: Prozentrechnung_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>24.06.2025 20:40:44</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary.MathFunc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.MathFunc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernTest.ModernBaseLibrary.ValueTypes;

    [TestClass]
    public class Prozentrechnung_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prozentrechnung_Test"/> class.
        /// </summary>
        public Prozentrechnung_Test()
        {
        }

        [TestMethod]
        public void Grundwert()
        {
            decimal grundwert = Prozentrechnung.Grundwert(5, 20);
            Assert.AreEqual(400, grundwert);
        }

        [DynamicData(nameof(GrundwertData))]
        //[DataRow(12.5,5.0,5.1)]
        [TestMethod]
        public void GrundwertReihe(decimal prozent, decimal prozentWert, decimal result)
        {
            decimal grundwert = Prozentrechnung.Grundwert(prozent, prozentWert);
            Assert.AreEqual(result, Convert.ToDecimal(string.Format("{0:0.00}", grundwert)));
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

        public static IEnumerable<object[]> GrundwertData =>
                    new[] {
                        new object[] { 5m, 20m, 400m },
                        new object[] { 60m, 354m, 590m },
                        new object[] { 20m, 320m, 1600m },
                        new object[] { 12m, 40m, 333.33m },
                        new object[] { 14, 8000m, 57142.86m },
                         };
    }

    /*
    public class DataRowCAttribute : DataRowAttribute
    {
        public DataRowCAttribute(params decimal?[]? data) => Data = data ?? [null];

        new public object?[] Data { get; }
    }
    */
}
