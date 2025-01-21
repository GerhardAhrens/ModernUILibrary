
namespace System
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class Currency : IEquatable<Currency>
    {
        public readonly string IsoCode;
        public readonly bool IsDigital;
        public readonly string GeneralName;
        public readonly string Symbol;
        public readonly int DecimalPlace;
        public readonly int BaseDecimalPlace;
        public readonly string DecimalMark;
        public readonly string ThousandMark;
        public readonly string SymbolPos;
        public readonly Dictionary<string, CurrencySubType> SubTypes;
        private CurrencyTypeRepository _repo = new CurrencyTypeRepository();

        public Currency(string isoCode)
        {
            if (_repo.Exists(isoCode) == false)
            {
                throw new ArgumentException("Invalid ISO Currency Code");
            }

            var newCurrency = _repo.Get(isoCode);
            this.IsoCode = newCurrency.IsoCode;
            this.IsDigital = newCurrency.IsDigital;
            this.GeneralName = newCurrency.GeneralName;
            this.Symbol = newCurrency.Symbol;
            this.DecimalPlace = newCurrency.DecimalPlace;
            this.BaseDecimalPlace = newCurrency.BaseDecimalPlace;
            this.DecimalMark = newCurrency.DecimalMark;
            this.ThousandMark = newCurrency.ThousandMark;
            this.SymbolPos = newCurrency.SymbolPos;
        }

        public Currency(string isoCode, bool isDigital, string generalName, string symbol, int decimalPlace, int baseDecimalPlace, string decimalMark, string thousandMark, string symbolPos = "R")
        {
            this.IsoCode = isoCode;
            this.IsDigital = isDigital;
            this.GeneralName = generalName;
            this.Symbol = symbol;
            this.DecimalPlace = decimalPlace;
            this.BaseDecimalPlace = baseDecimalPlace;
            this.DecimalMark = decimalMark;
            this.ThousandMark = thousandMark;
            this.SymbolPos = symbolPos;
        }

        public Currency(string isoCode, bool isDigital, string generalName, string symbol, int decimalPlace, int baseDecimalPlace, string decimalMark, string thousandMark, string symbolPos, Dictionary<string, CurrencySubType> subtypes)
        {
            this.IsoCode = isoCode;
            this.IsDigital = isDigital;
            this.GeneralName = generalName;
            this.Symbol = symbol;
            this.DecimalPlace = decimalPlace;
            this.BaseDecimalPlace = baseDecimalPlace;
            this.DecimalMark = decimalMark;
            this.ThousandMark = thousandMark;
            this.SymbolPos = symbolPos;
            this.SubTypes = subtypes;
        }

        public CurrencySubType DisplayingSubType { get; private set; }

        #region Equals and !Equals

        public bool Equals(Currency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return IsoCode == other.IsoCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(Currency))
            {
                return false;
            }

            return Equals((Currency)obj);
        }

        public override int GetHashCode()
        {
            return IsoCode.GetHashCode();
        }

        public static bool operator ==(Currency left, Currency right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !Equals(left, right);
        }

        #endregion

        public CurrencySubType GetSubType(string key)
        {
            if (this.SubTypes != null)
            {
                if (this.SubTypes.ContainsKey(key))
                {
                    return this.SubTypes[key];
                }
            }

            return null;
        }

        public void DisplayAsSubType(string key)
        {
            this.DisplayingSubType = this.GetSubType(key);
        }

        public string GetStringFormat()
        {
            string decimalZero = "";
            for (int i = 1; i <= this.DecimalPlace; i++)
            {
                decimalZero += "0";
            }
            string specifier = $"#{this.ThousandMark}0{this.DecimalMark}{decimalZero};(#,0{decimalZero})";
            return specifier;
        }

        public override string ToString()
        {
            return $"{this.IsoCode} {this.Symbol}";
        }

        /// <summary>
        /// Display any passed in Money object decorated by its own Currency object
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string ToString(Money m)
        {
            string result = string.Empty;
            string displaySymbol = m.Currency.Symbol;
            decimal displayAmount = m.Amount;

            if (m.Currency.DisplayingSubType != null)
            {
                displaySymbol = m.Currency.DisplayingSubType.Symbol;
                displayAmount = m.Currency.DisplayingSubType.ScaleToUnit * m.Amount;
            }

            if (m.Currency.SymbolPos == "R")
            {
                result = $"{displayAmount.ToString(this.GetStringFormat())} {displaySymbol}";
            }
            else
            {
                result = $"{displaySymbol}{displayAmount.ToString(this.GetStringFormat())}";
            }

            return result;
        }
    }


    public class CurrencySubType
    {
        public string Symbol { get; set; }

        public int ScaleToUnit { get; set; }
    }

    [SupportedOSPlatform("windows")]
    public class CurrencyTypeRepository
    {
        // List of all currencies with their properties.
        public static readonly Dictionary<string, Currency> Currencies =
            new Dictionary<string, Currency>()
            {
            {"BTC", new Currency("BTC", true, "Bitcoin", "฿",                   3, 8, ".", "," ,"L",
                new Dictionary<string, CurrencySubType>(){
                    {"mBTC", new CurrencySubType{Symbol="mBTC ", ScaleToUnit=1000}}
                }
            )},
            {"LTC", new Currency("LTC", true, "Litecoin", "L",                  3, 8, ".", "," ,"L",
                new Dictionary<string, CurrencySubType>(){
                    {"mLTC", new CurrencySubType{Symbol="mLTC ", ScaleToUnit=1000}}
                }
            )},
            {"AUD", new Currency("AUD", false, "Australian dollar", "$",        2, 2, ".", ",","L")},
            {"CAD", new Currency("CAD", false, "Canadian dollar", "$",          2, 2, ".", ",","")},
            {"EUR", new Currency("EUR", false, "Euro", "€" ,                    2, 2, ".", ",","R")},
            {"GBP", new Currency("GBP", false, "Pound sterling", "£",           2, 2, ".", ",", "L")},
            {"USD", new Currency("USD", false, "US dollar", "$",                2, 2, ".", ",","L")},
            };

        public Currency Get(string isoCode)
        {
            if (Currencies.ContainsKey(isoCode.ToUpper()))
            {
                return Currencies[isoCode.ToUpper()];
            }
            else
            {
                return null;
            }
        }

        public bool Exists(string isoCode)
        {
            return Currencies.ContainsKey(isoCode.ToUpper());
        }
    }
}