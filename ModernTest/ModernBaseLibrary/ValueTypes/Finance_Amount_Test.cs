/*
 * <copyright file="Finance_Test.cs" company="Lifeprojects.de">
 *     Class: Finance_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.03.2025 20:18:18</date>
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

    [TestClass]
    public class Finance_Amount_Test
    {
        private ValueBase GetOtherValue() => new Amount(3.14m);
        private ValueBase GetSampleValue1() => new Amount(2.72m);
        private ValueBase GetSampleValue2() => new Amount(2.72m);

        private readonly IEqualityComparer<ValueBase> _comparer = EqualityComparer<ValueBase>.Default;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Finance_Amount_Test"/> class.
        /// </summary>
        public Finance_Amount_Test()
        {
        }

        [TestMethod]
        public void Monies_OfDifferentCurrencyAndSameAmount_AreNotEqual()
        {
            var money1 = new Money(Currency.USD, new Amount(45.08m));
            var money2 = new Money(Currency.EUR, new Amount(45.08m));

            Assert.IsFalse(money1.Equals(money2));
            Assert.IsFalse(money2.Equals(money1));
            Assert.IsFalse(money1 == money2);
            Assert.IsFalse(money2 == money1);
            Assert.IsTrue(money1 != money2);
            Assert.IsTrue(money2 != money1);
        }

        [TestMethod]
        public void Monies_OfSameCurrencyAndDifferentAmount_AreNotEqual()
        {
            var money1 = new Money(Currency.USD, new Amount(45.08m));
            var money2 = new Money(Currency.USD, new Amount(14.15m));

            Assert.IsFalse(money1.Equals(money2));
            Assert.IsFalse(money2.Equals(money1));
            Assert.IsFalse(money1 == money2);
            Assert.IsFalse(money2 == money1);
            Assert.IsTrue(money1 != money2);
            Assert.IsTrue(money2 != money1);
        }

        [TestMethod]
        public void Wallet_WithSameCashAndCards_AreEqual()
        {
            var wallet1 = new Wallet(
                new[] { 20m.Dollars(), 10m.Euros(), 5m.Euros() },
                new[] { CreditCompany.Visa.For(1000m.ToAmount()), CreditCompany.MasterCard.For(5000m.ToAmount())
                });

            var wallet2 = new Wallet(
                new[] { 10m.Euros(), 20m.Dollars(), 5m.Euros() },
                new[] { CreditCompany.MasterCard.For(5000m.ToAmount()), CreditCompany.Visa.For(1000m.ToAmount())
                });

            EqualityComparer<Wallet>.Default.Equals(wallet1, wallet2);
            Assert.AreEqual(wallet1, wallet2);
            Assert.IsTrue(wallet1.Equals(wallet2));
            Assert.IsTrue(wallet1 == wallet2);
            Assert.IsTrue(wallet2 == wallet1);
            Assert.IsFalse(wallet1 != wallet2);
            Assert.IsFalse(wallet2 != wallet1);
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
