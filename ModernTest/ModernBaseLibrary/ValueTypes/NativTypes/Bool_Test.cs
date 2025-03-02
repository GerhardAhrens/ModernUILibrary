/*
 * <copyright file="Bool_Test.cs" company="Lifeprojects.de">
 *     Class: Bool_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.03.2025 19:21:31</date>
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using global::ModernBaseLibrary.ValueTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class Bool_Test : BaseTest
    {
        private readonly IEqualityComparer<ValueBase> _comparer = EqualityComparer<ValueBase>.Default;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bool_Test"/> class.
        /// </summary>
        public Bool_Test()
        {
        }

        [TestMethod]
        public void Value_EqualsNull_IsFalse()
        {
            ValueBase value = false;

            Assert.IsFalse(_comparer.Equals(value, null));
            Assert.IsFalse(value!.Equals(null));
            Assert.IsFalse(value! == null!);
            Assert.IsFalse(null! == value!);
            Assert.IsTrue(value! != null!);
            Assert.IsTrue(null! != value!);
        }

        [TestMethod]
        public void Value_EqualsNull_IsTrue()
        {
            ValueBase value = true;

            Assert.IsTrue(_comparer.Equals(value, value));
            Assert.IsTrue(value.Equals(value));
            Assert.AreEqual(value, value);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(value == value);
            Assert.IsFalse(value != value);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [TestMethod]
        public void Equals_EquivalentInstance_IsTrue()
        {
            ValueBase value1 = true;
            ValueBase value2 = true;

            Assert.IsTrue(_comparer.Equals(value1, value2));
            Assert.AreEqual(value1, value2);
            Assert.IsTrue(value1.Equals(value2));
            Assert.IsTrue(value1 == value2);
            Assert.IsTrue(value2 == value1);
            Assert.IsFalse(value1 != value2);
            Assert.IsFalse(value2 != value1);
        }

        [TestMethod]
        public void Equals_DifferentInstance_IsFalse()
        {
            ValueBase value1 = false;
            ValueBase value2 = true;

            Assert.IsFalse(_comparer.Equals(value1, value2));
            Assert.AreNotEqual(value1, value2);
            Assert.IsFalse(value1.Equals(value2));
            Assert.IsFalse(value2.Equals(value1));
            Assert.IsFalse(value1 == value2);
            Assert.IsFalse(value2 == value1);
            Assert.IsTrue(value1 != value2);
            Assert.IsTrue(value2 != value1);
        }

        [TestMethod]
        public void GetHashCode_ForEquivalentInstances_AreEqual()
        {
            ValueBase value1 = false;
            ValueBase value2 = false;

            Assert.AreEqual(value1.GetHashCode(), value2.GetHashCode());
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
