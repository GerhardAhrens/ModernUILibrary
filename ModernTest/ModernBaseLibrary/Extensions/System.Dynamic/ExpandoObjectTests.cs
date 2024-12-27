/*
 * <copyright file="ExpandoObjectTests.cs" company="Lifeprojects.de">
 *     Class: ExpandoObjectTests
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>12.02.2024</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Test Klasse für den Typ ExpandoObject
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
    using System.Data;
    using System.Dynamic;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for ExpandoObjectTests
    /// </summary>
    [TestClass]
    public class ExpandoObjectTests : BaseTest
    {
        [TestMethod]
        public void ContainsPropertyTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("Text", "Hello");
            target.SetPropertyValue<int>("Number", 12);

            Assert.IsTrue(target.ContainsProperty("Text"));
            Assert.IsFalse(target.ContainsProperty("text"));

            Assert.IsTrue(target.ContainsProperty("Number"));
            Assert.IsFalse(target.ContainsProperty("NumBer"));

            Assert.IsFalse(target.ContainsProperty("AnyOther"));
        }

        [TestMethod]
        public void GetPropertyValueTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("Text", "Hello");
            target.SetPropertyValue<int>("Number", 12);

            Assert.AreEqual("Hello", target.GetPropertyValue<string>("Text"));
            Assert.IsNull(target.GetPropertyValue<object>("Something"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetPropertyValueInvalidCastTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("Text", "Hello");

            Assert.AreEqual(default(decimal), target.GetPropertyValue<decimal>("Text"));
        }

        [TestMethod]
        public void TryGetPropertyValueFoundTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("Text", "Hello");

            string value;
            bool success = target.TryGetPropertyValue<string>("Text", out value);
            Assert.IsTrue(success);
            Assert.AreEqual("Hello", value);
        }

        [TestMethod]
        public void TryGetPropertyValueNotFoundTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("Text2", "Hello");

            string value;
            bool success = target.TryGetPropertyValue<string>("Text", out value);
            Assert.IsFalse(success);
            Assert.AreEqual(default(string), value);
        }

        [TestMethod]
        public void CreatePropertyTest()
        {
            var target = new ExpandoObject();

            target.CreateProperty<string>("ColorCode");
            
            Assert.IsTrue(target.ContainsProperty("ColorCode"));
            Assert.IsTrue(target.IsNullOrDefault<string>("ColorCode"));
        }

        [TestMethod]
        public void IsNullTest()
        {
            var target = new ExpandoObject();

            target.CreateProperty<DataTable>("ColorTabke");

            Assert.IsTrue(target.IsNull("ColorTabke"));
        }

        [TestMethod]
        public void SetPropertyTest()
        {
            var target = new ExpandoObject();

            target.SetPropertyValue<string>("ColorCode", "Blue");
            target.SetPropertyValue<string>("ColorCode", "Green");

            Assert.IsTrue(target.ContainsProperty("ColorCode"));
            Assert.AreEqual("Green", target.GetPropertyValue<string>("ColorCode"));
        }
    }
}
