namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_Money_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void CurrencyWrong()
        {
            try
            {
                Currency cur1 = new Currency("XXX");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void CurrencyCorrect()
        {
            try
            {
                Currency cur1 = new Currency("eur");
                Assert.AreEqual<string>(cur1.IsoCode, "EUR");

                Currency cur2 = new Currency("EUR");
                Assert.AreEqual<string>(cur2.IsoCode, "EUR");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void CurrencyIsEqual()
        {
            Currency cur1 = new Currency("EUR");
            Currency cur2 = new Currency("EUR");
            Assert.AreEqual<Currency>(cur1, cur2);
        }

        [TestMethod]
        public void CurrencyIsNotEqual()
        {
            Currency cur1 = new Currency("EUR");
            Currency cur2 = new Currency("USD");
            Assert.AreNotEqual<Currency>(cur1, cur2);
        }

        [TestMethod]
        public void MoneyCurrencyIsNotEqual()
        {
            Money m1 = new Money(1m, "EUR");
            Money m2 = new Money(1m, "USD");
            Assert.AreNotEqual<Money>(m1, m2);
        }

        [TestMethod]
        public void MoneyIsNotEqualAmount()
        {
            Money m1 = new Money(1m, "EUR");
            Money m2 = new Money(2m, "EUR");
            Assert.AreNotEqual<Money>(m1, m2);
        }

        [TestMethod]
        public void MoneyIsEqual()
        {
            Money m1 = new Money(1.00000001m, "EUR");
            Money m2 = new Money(1.00000001m, "EUR");
            Assert.AreEqual<Money>(m1, m2);
        }

        [TestMethod]
        public void MoneyAddMoney()
        {
            Money m1 = new Money(1.00000001m, "EUR");
            Money m2 = new Money(10.000000019m, "EUR");

            Money result = m1 + m2;
            Assert.AreEqual<Money>(result, new Money(11.000000029m, "EUR"));
        }

        [TestMethod]
        public void MoneyAddDecimal()
        {
            Money m1 = new Money(1.00000001m, "EUR");

            Money result = m1 + 10.000000019m;
            if (result.Amount != 11.000000029m)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MoneySubtractMoney()
        {
            Money m1 = new Money(1.00000001m, "EUR");
            Money m2 = new Money(10.000000019m, "EUR");

            Money result = m2 - m1;
            Assert.AreEqual<Money>(result, new Money(9.000000009m, "EUR"));
        }

        [TestMethod]
        public void MoneySubtractDecimal()
        {
            Money m1 = new Money(1.00000001m, "EUR");

            Money result = m1 - 10.000000019m;
            if (result.Amount != -9.000000009m)
                Assert.Fail();
        }

        [TestMethod]
        public void MoneyMultiplyDecimal()
        {
            Money m1 = new Money(1.02m, "EUR");

            Money result = m1 * 2.5m;
            Assert.AreEqual<Money>(result, new Money(2.55m, "EUR"));
        }

        [TestMethod]
        public void MoneyMultiplyInt()
        {
            Money m1 = new Money(1.000000014m, "EUR");

            Money result = m1 * 2;
            if (result.Amount != 2.000000028m)
                Assert.Fail();
        }

        [TestMethod]
        public void MoneyMultiplySmallDecimal()
        {
            Money m1 = new Money(1.000000005m, "EUR");

            Money result = m1 * (5m / 1000m);
            Assert.AreEqual<Money>(result, new Money(0.005000000025m, "EUR"));
        }

        [TestMethod]
        public void MoneyDividedByInt()
        {
            Money m1 = new Money(2.5m, "EUR");

            Money result = m1 / 2;
            Assert.AreEqual<Money>(result, new Money(1.25m, "EUR"));
        }

        [TestMethod]
        public void MoneyImpliciteValueForDecimal()
        {
            Money m1 = 1.99m;
            Assert.AreEqual<Money>(m1, new Money(1.99m, "EUR"));
        }

        [TestMethod]
        public void MoneyImpliciteValueForDouble()
        {
            Money m1 = 1.99;
            Assert.AreEqual<Money>(m1, new Money(1.99, "EUR"));
        }

        [TestMethod]
        public void MoneyImpliciteValueForInt()
        {
            Money m1 = 1;
            Assert.AreEqual<Money>(m1, new Money(1, "EUR"));
        }

        [TestMethod]
        public void MoneyToString()
        {
            Money m1 = 1.99;
            Assert.AreEqual(m1.ToString(), "1,99 €");

            Money m2 = new Money(1.99, "EUR");
            Assert.AreEqual(m2.ToString(), "1,99 €");

            Money m3 = new Money(1.99, "USD");
            Assert.AreEqual(m3.ToString(), "$1,99");
        }

        [TestMethod]
        public void Money_TruncatePrecision()
        {
            Money m1 = 1.99;
            Assert.AreEqual(m1.TruncatePrecision(1), 1,9m);
        }

        [TestMethod]
        public void Money_GetDecimalPlaces()
        {
            Money m1 = 1.996677;
            Assert.AreEqual(m1.GetDecimalPlaces(), 6);
        }

        [TestMethod]
        public void Money_FullHundredRoundDown()
        {
            Money m1 = 230_345.87;
            Assert.AreEqual(m1.FullHundredRoundDown(), 230_300);
        }

        [TestMethod]
        public void Money_FullHundredRoundUp()
        {
            Money m1 = 230_365.87;
            Money result = m1.FullHundredRoundUp();
            Assert.AreEqual<Money>(result, 230_400);
        }

        [TestMethod]
        public void Money_GetCurrency()
        {
            Money m1 = 230_365.87;
            Assert.AreEqual<string>(m1.GetCurrency().IsoCode, "EUR");
        }
    }
}
