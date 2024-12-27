/*
 * <copyright file="DictionaryExtension_Test.cs" company="Lifeprojects.de">
 *     Class: DictionaryExtension_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 09:15:32</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * UnitTest für IDictionaryExtension
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionaryExtension_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDictionaryExtension_Test"/> class.
        /// </summary>
        public DictionaryExtension_Test()
        {
        }

        [TestMethod]
        public void IsNullOrEmpty_SetNull()
        {
            Dictionary<int,int> dict = null;
            bool result = dict.IsNullOrEmpty();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullOrEmpty_SetInstanz()
        {
            IDictionary dict = new Dictionary<int,int>();
            bool result = dict.IsNullOrEmpty();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullOrEmpty_SetGreater0()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(0, 0);
            bool result = dict.IsNullOrEmpty();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddIfNotExists()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.AddIfNotExists(1,1);
            dict.AddIfNotExists(2,2);
            Assert.IsTrue(dict.Count == 2); 
        }

        [TestMethod]
        public void AddRangeFromList()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "1");
            dict.Add(2, "2");

            List<KeyValue> liste = new List<KeyValue>();
            liste.Add(new KeyValue() {Id = 10, Name = "10" });

            dict.AddRange(liste, c => c.Id, c => c.Name, true);
        }

        [TestMethod]
        public void AddRangeFromIEnumerable()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "1");
            dict.Add(2, "2");

            List<KeyValue> liste = new List<KeyValue>();
            liste.Add(new KeyValue() { Id = 10, Name = "10" });

            IEnumerable<KeyValue> listeAs = liste.AsEnumerable();
            dict.AddRange(listeAs, c => c.Id, c => c.Name);
        }

        [TestMethod]
        public void AddOrGet()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "1");
            dict.Add(2, "2");
            var result = dict.AddOrGet(3, "3");
            Assert.IsTrue(result == "3");
        }

        [TestMethod]
        public void DeleteIfExistsKey()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            dict.DeleteIfExistsKey(1);
            Assert.IsTrue(dict.Count == 1);
            dict.DeleteIfExistsKey(1);
            Assert.IsTrue(dict.Count == 1);
        }

        [TestMethod]
        public void Update_With_Key()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            dict.Update(1, 11);
            Assert.IsTrue(dict[1] == 11);
        }

        [TestMethod]
        public void Update_With_KeyValuePair()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(1, 11);

            dict.Update(pair);
            Assert.IsTrue(dict[1] == 11);
        }

        [TestMethod]
        public void Delete_IfExistsValue()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            dict.DeleteIfExistsValue(1);
            Assert.IsTrue(dict.Count == 1);
        }

        [TestMethod]
        public void AreValuesEmpty()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            bool result = dict.AreKeysEmpty();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreKeysEmpty()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            bool result = dict.AreKeysEmpty();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToStringFromDictionary()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(2, 2);
            Assert.IsTrue(dict.Count == 2);

            string result = dict.ToString(";");
            Assert.IsTrue(result == "1=1;2=2");
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

        private class KeyValue
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
