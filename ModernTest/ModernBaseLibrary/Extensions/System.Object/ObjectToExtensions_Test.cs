/*
 * <copyright file="ObjectToExtensions_Test.cs" company="Lifeprojects.de">
 *     Class: ObjectToExtensions_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 15:10:18</date>
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

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectToExtensions_Test
    {

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToExtensions_Test"/> class.
        /// </summary>
        public ObjectToExtensions_Test()
        {
        }

        [TestMethod]
        public void ToForInt()
        {
            string nullValue = null;
            string value = "1";
            object dbNullValue = DBNull.Value;

            // Exemples
            var result1 = value.To<int>(); // return 1;
            var result2 = value.To<int?>(); // return 1;
            var result3 = nullValue.To<int?>(); // return null;
            var result4 = dbNullValue.To<int?>(); // return null;

            // Unit Test
            Assert.AreEqual(1, result1);
            Assert.AreEqual(1, result2.Value);
            Assert.IsFalse(result3.HasValue);
            Assert.IsFalse(result4.HasValue);
        }

        [TestMethod]
        public void ToForGuid()
        {
            // Exemples
            Guid value = Guid.NewGuid();
            string result = value.To<string>();
            string result1 = value.ToOrDefault<string>();

            // Unit Test
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.IsTrue(result1.GetType() == typeof(string));
        }

        [TestMethod]
        public void ToForDecimal()
        {
            // Exemples
            string valueDecimal = "1,1";
            decimal resultDecimal = valueDecimal.To<decimal>(); // return 1.1

            // Unit Test
            Assert.IsTrue(resultDecimal.GetType() == typeof(decimal));
            Assert.IsTrue(resultDecimal == 1.1M);
        }

        [TestMethod]
        public void ToForNullString()
        {
            // Exemples
            string nullValue = null;
            string resultString = nullValue.ToOrDefault<string>();

            // Unit Test
            Assert.IsTrue(resultString.GetType() == typeof(string));
            Assert.IsTrue(resultString == string.Empty);
        }

        [TestMethod]
        public void ToForNullStringWithDefaultString()
        {
            // Exemples
            string nullValue = null;
            string resultString = nullValue.ToOrDefault("-");

            // Unit Test
            Assert.IsTrue(resultString.GetType() == typeof(string));
            Assert.IsTrue(resultString == "-");
        }

        [TestMethod]
        public void ToForNullStringWithDefaultChar()
        {
            // Exemples
            string nullValue = null;
            string resultString = nullValue.ToOrDefault('-').ToString();

            // Unit Test
            Assert.IsTrue(resultString.GetType() == typeof(string));
            Assert.IsTrue(resultString == "-");
        }

        [TestMethod]
        public void ToForString()
        {
            // Exemples
            string nullValue = "Gerhard";
            string resultString = nullValue.ToOrDefault<string>();

            // Unit Test
            Assert.IsTrue(resultString.GetType() == typeof(string));
            Assert.IsTrue(resultString == "Gerhard");
        }

        [DataRow("1", true)]
        [DataRow("y", true)]
        [DataRow("true", true)]
        [DataRow("True", true)]
        [TestMethod]
        public void ToBoolIsTrue(object input, bool expected)
        {
            bool result = input.ToBool();
            Assert.IsTrue(result);
        }

        [DataRow("0", false)]
        [DataRow("n", false)]
        [DataRow("false", false)]
        [DataRow("False", false)]
        [TestMethod]
        public void ToBoolIsFalse(object input, bool expected)
        {
            bool result = input.ToBool();
            Assert.IsFalse(result);
        }

        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("10", false)]
        [DataRow("hallo", false)]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void NotBoolStringWithException(object input, bool expected)
        {
            bool result = input.ToBool();
            Assert.IsFalse(result);
        }

        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("10", false)]
        [DataRow("hallo", false)]
        [TestMethod]
        public void NotBoolStringWithoutException(object input, bool expected)
        {
            bool result = input.ToBool(true);
            Assert.IsFalse(result);
        }
    }
}
