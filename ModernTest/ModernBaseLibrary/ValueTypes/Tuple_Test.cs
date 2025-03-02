/*
 * <copyright file="Tuple_Test.cs" company="Lifeprojects.de">
 *     Class: Tuple_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.03.2025 20:47:57</date>
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

namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Tuple_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tuple_Test"/> class.
        /// </summary>
        public Tuple_Test()
        {
        }

        [TestMethod]
        public void Tuple_OfEquivalentValues_AreEqual()
        {
            var t1 = (4, 8, new Point2d(4, 8));
            var t2 = (4, 8, new Point2d(4, 8));

            Assert.AreEqual(t1, t2);
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2);
            Assert.IsTrue(t2 == t1);
            Assert.IsFalse(t1 != t2);
            Assert.IsFalse(t2 != t1);
        }

        [TestMethod]
        public void Tuple_OfNonEquivalentValues_AreEqual()
        {
            var t1 = (4, new Point2d(4, 8), 8);
            var t2 = (4, 8, new Point2d(4, 8));

            //Assert.AreNotEqual(t1, t2);
            Assert.IsFalse(t1.Equals(t2));
            Assert.IsFalse(t1 == t2);
            Assert.IsFalse(t2 == t1);
            Assert.IsTrue(t1 != t2);
            Assert.IsTrue(t2 != t1);
        }

        [TestMethod]
        public void VectorsAsTuple_OfEquivalentValues_AreEqual()
        {
            var t1 = (new Point2d(4, 8), new Point2d(15, 16));
            var t2 = (new Point2d(4, 8), new Point2d(15, 16));

            Assert.AreEqual(t1, t2);
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2);
            Assert.IsTrue(t2 == t1);
            Assert.IsFalse(t1 != t2);
            Assert.IsFalse(t2 != t1);
        }

        [TestMethod]
        public void VectorsAsTuple_OfNonEquivalentValues_AreEqual()
        {
            var t1 = (new Point2d(15, 16), new Point2d(4, 8));
            var t2 = (new Point2d(4, 8), new Point2d(15, 16));

            Assert.AreNotEqual(t1, t2);
            Assert.IsFalse(t1.Equals(t2));
            Assert.IsFalse(t1 == t2);
            Assert.IsFalse(t2 == t1);
            Assert.IsTrue(t1 != t2);
            Assert.IsTrue(t2 != t1);
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
