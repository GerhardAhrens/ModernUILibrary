/*
 * <copyright file="CustomLinq_Test.cs" company="Lifeprojects.de">
 *     Class: CustomLinq_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>13.12.2022 19:32:27</date>
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

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows.Data;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CustomLinq_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLinq_Test"/> class.
        /// </summary>
        public CustomLinq_Test()
        {
        }

        [TestMethod]
        public void CreateNumbersCollection()
        {
            NumbersCollection numCollection = CreateNumbers();

            Assert.AreEqual(numCollection.MyCount(), 10);
        }

        [TestMethod]
        public void RemoveItemFromCollection()
        {
            NumbersCollection numCollection = CreateNumbers();

            Nummer numToRemove = numCollection.Get(c => c.IntValue.Equals(1));
            numCollection.Remove(numToRemove);
            Nummer num = numCollection.Get(c => c.IntValue.Equals(1));

            Assert.AreEqual(numCollection.MyCount(), 9);
            Assert.IsNull(num);
        }

        [TestMethod]
        public void RemoveForItemFromCollection()
        {
            NumbersCollection numCollection = CreateNumbers();

            numCollection.Remove(c => c.IntValue.Equals(1));
            Nummer num = numCollection.Get(c => c.IntValue.Equals(1));

            Assert.AreEqual(numCollection.MyCount(), 9);
            Assert.IsNull(num);
        }

        [TestMethod]
        public void GetNumber()
        {
            NumbersCollection numCollection = CreateNumbers();

            Assert.AreEqual(numCollection.MyCount(), 10);

            Nummer result = numCollection.GetNumber(a => a.Active == true);
            Assert.AreEqual(result.IntValue , new Nummer() { IntValue = 1, Active = true }.IntValue);
        }

        [TestMethod]
        public void GetNumbers()
        {
            NumbersCollection numCollection = CreateNumbers();

            Assert.AreEqual(numCollection.MyCount(), 10);

            IEnumerable<Nummer> result = numCollection.GetNumbers(a => a.Active == true);
            Assert.AreEqual(result.MyCount(), 5);
        }

        [TestMethod]
        public void NumbersCollectionMinObject()
        {
            NumbersCollection listeInt = CreateNumbers();
            Nummer result = listeInt.MinObject(p => p.IntValue);
            Assert.IsTrue(result.IntValue == 1);
        }

        [TestMethod]
        public void NumbersCollectionMaxObject()
        {
            NumbersCollection listeInt = CreateNumbers();
            Nummer result = listeInt.MaxObject(p => p.IntValue);
            Assert.IsTrue(result.IntValue == 10);
        }

        [TestMethod]
        public void NumbersCollectionEquals()
        {
            NumbersCollection listeInt = CreateNumbers();
            Nummer num = listeInt.Get(c => c.IntValue.Equals(1));
            Assert.IsTrue(num.IntValue == 1);
        }


        public void GetNumbersForeach()
        {
            NumbersCollection numCollection = CreateNumbers();

            Assert.AreEqual(numCollection.MyCount(), 10);

            numCollection.MyForEach(f => { Console.WriteLine($"{f.IntValue}"); });

            foreach (Nummer item in numCollection)
            {
                Console.WriteLine($"{item.IntValue}");
            }
        }

        [TestMethod]
        public void NumbersCollectionMoveTo()
        {
            NumbersCollection numCollection = CreateNumbers();
            numCollection.MoveTo(5);
            Nummer num = numCollection.Current;
            Assert.IsTrue(num.IntValue == 6);
        }

        [TestMethod]
        public void NumbersCollectionIsOverNull()
        {
            NumbersCollection numCollection = CreateNumbers();
            IEnumerable<Nummer> result = numCollection.Where(p => p.Active == true).IsOverNull().AsEnumerable();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 5);
        }

        [TestMethod]
        public void NumbersCollectionMyFilter()
        {
            NumbersCollection numCollection = CreateNumbers();
            IEnumerable<Nummer> result = numCollection.MyFilter(p => p.IntValue > 0 && p.Active == true).AsEnumerable();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 5);
        }

        [TestMethod]
        public void NumbersCollection_ListCollectionView()
        {
            NumbersCollection numCollection = CreateNumbers();
            ListCollectionView view = numCollection.View(item => DataTextFilter(item as Nummer));
            Assert.IsTrue(view.Count == 10);
        }

        private bool DataTextFilter(Nummer rowItem)
        {
            return true;
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

        private NumbersCollection CreateNumbers()
        {
            NumbersCollection numCollection = new NumbersCollection();
            numCollection.Add(new Nummer() { IntValue = 1, Active = true });
            numCollection.Add(new Nummer() { IntValue = 2, Active = false });
            numCollection.Add(new Nummer() { IntValue = 3, Active = true });
            numCollection.Add(new Nummer() { IntValue = 4, Active = false });
            numCollection.Add(new Nummer() { IntValue = 5, Active = true });
            numCollection.Add(new Nummer() { IntValue = 6, Active = false });
            numCollection.Add(new Nummer() { IntValue = 7, Active = true });
            numCollection.Add(new Nummer() { IntValue = 8, Active = false });
            numCollection.Add(new Nummer() { IntValue = 9, Active = true });
            numCollection.Add(new Nummer() { IntValue = 10, Active = false });

            return numCollection;
        }
    }
}
