namespace System
{
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public sealed class Money : CustomDataTypeBase, IComparable, IComparable<Money>
    {
        public decimal Amount { get; private set; }
        public readonly Currency Currency;

        public Money(decimal amount, Currency currency)
        {
            AssertNotNull(currency);
            this.Amount = amount;
            this.Currency = currency;
        }

        public Money(decimal amount, string isoCode)
        {
            this.Amount = (decimal)amount;
            this.Currency = new Currency(isoCode.ToUpper());
            AssertNotNull(Currency);
        }

        public Money(double amount, string isoCode)
        {
            this.Amount = (decimal)amount;
            this.Currency = new Currency(isoCode.ToUpper());
            AssertNotNull(Currency);
        }

        public Money(float amount, string isoCode)
        {
            this.Amount = (decimal)amount;
            this.Currency = new Currency(isoCode.ToUpper());
            AssertNotNull(Currency);
        }

        public Money(int amount, string isoCode)
        {
            this.Amount = (decimal)amount;
            this.Currency = new Currency(isoCode.ToUpper());
            AssertNotNull(Currency);
        }

        #region Comparable and Operator >, <, >=, <=

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is Money)) throw new ArgumentException("Object is not Money object");

            return CompareTo((Money)obj);
        }

        public int CompareTo(Money other)
        {
            if (other == null) return 1;
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }

        public static bool operator >(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount > right.Amount;
        }

        public static bool operator >=(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount >= right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount < right.Amount;
        }

        public static bool operator <=(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount <= right.Amount;
        }

        #endregion

        #region Operator +, -
        public static Money operator +(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator +(Money left, decimal right)
        {
            AssertNotNull(left);
            return new Money(left.Amount + right, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static Money operator -(Money left, decimal right)
        {
            AssertNotNull(left);
            return new Money(left.Amount - right, left.Currency);
        }

        #endregion

        #region Operator *, /

        public static Money operator *(Money left, decimal right)
        {
            AssertNotNull(left);
            return new Money(left.Amount * right, left.Currency);
        }

        public static Money operator /(Money left, decimal right)
        {
            AssertNotNull(left);
            return new Money(left.Amount / right, left.Currency);
        }

        #endregion

        #region implicit operator
        public static implicit operator Money(decimal value)
        {
            return new Money(value, "EUR");
        }

        public static implicit operator Money(double value)
        {
            return new Money((decimal)value, "EUR");
        }

        public static implicit operator Money(float value)
        {
            return new Money((decimal)value, "EUR");
        }

        public static implicit operator Money(long value)
        {
            return new Money((decimal)value, "EUR");
        }

        public static implicit operator Money(int value)
        {
            return new Money((decimal)value, "EUR");
        }
        #endregion implicit operator

        #region Helper functions
        public static void AssertNotNull(Money money)
        {
            Argument.NotNull(money, "Money Is Null");
        }

        public static void AssertNotNull(Currency currency)
        {
            Argument.NotNull(currency, "Currency Is Null");
        }

        public static void AssertSameCurrency(Money first, Money second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("Any Money Is Null");
            }

            if (first.Currency != second.Currency)
            {
                throw new ArgumentException("Money Currency Not Equal");
            }
        }
        #endregion

        #region Extension Methodes
        public decimal TruncatePrecision(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentException($"Number of decimal places ({precision} is invalid!)");
            }

            var multiplied = this.Amount * (decimal)(Math.Pow(10, precision));

            decimal skippedValue;
            if (this.Amount >= 0)
            {
                skippedValue = Math.Floor(multiplied);
            }
            else
            {
                skippedValue = Math.Ceiling(multiplied);
            }

            return skippedValue / (decimal)(Math.Pow(10, precision));
        }

        public int GetDecimalPlaces()
        {
            return BitConverter.GetBytes(decimal.GetBits(this.Amount)[3])[2];
        }

        public Money ToBrutto(double tax)
        {
            decimal result = 0;

            decimal taxValue = this.Amount * (decimal)tax / 100;

            result = this.Amount + taxValue;

            return new Money(result, this.Currency);
        }

        public Money ToNetto(double tax)
        {
            decimal result = 0;

            decimal taxValue = this.Amount / (1 + (decimal)tax / 100) * (decimal)tax / 100;

            result = this.Amount - taxValue;

            return new Money(result, this.Currency);
        }

        public Money FullHundredRoundDown()
        {
            int result = Convert.ToInt32(Math.Floor(this.Amount));

            int rest = result % 100;
            if (rest < 99)
            {
                return new Money(result - rest, this.Currency);
            }
            else
            {
                return new Money(result + (100 - rest), this.Currency);
            }
        }

        public Money FullHundredRoundUp()
        {
            int result = Convert.ToInt32(Math.Floor(this.Amount));

            int rest = result % 100;
            if (rest < 50)
            {
                return new Money(result - rest, this.Currency);
            }
            else
            {
                return new Money(result + (100 - rest), this.Currency);
            }
        }

        public Currency GetCurrency()
        {
            return this.Currency;
        }

        #endregion Extension Methodes

        protected override IEnumerable<object> GetValues()
        {
            yield return this.Amount;
            yield return this.Currency;
        }

        /// <summary>
        /// Use the decorated interal Currency object to display the string
        /// </summary>
        /// 
        /// <returns>string</returns>
        public override string ToString()
        {
            return Currency.ToString(this);
        }
    }
}