//-----------------------------------------------------------------------
// <copyright file="Roman.cs" company="Lifeprojects.de">
//     Class: Roman
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.03.2023</date>
//
// <summary>
// Domain Klasse für römische Zahlen
// </summary>
// <Link>
// https://github.com/KeesCBakker/KeesTalksTech-Utility-Pack/tree/master/KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities.Tests
// </Link>
//-----------------------------------------------------------------------

namespace System
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Roman
    {
        public const string NULLA = "NULLA";

        public static readonly IReadOnlyDictionary<string, int> VALUES = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>
        {
            {"I",       1 },
            {"IV",      4 },
            {"V",       5 },
            {"IX",      9 },
            {"X",       10 },
            {"XIIX",    18 },
            {"IIXX",    18 },
            {"XL",      40 },
            {"L",       50 },
            {"XC",      90 },
            {"C",       100 },
            {"CD",      400 },
            {"D",       500 },
            {"CM",      900 },
            {"M",       1000 },

            //alternatives from Middle Ages and Renaissance
            {"O",       11 },
            {"F",       40 },
            {"P",       400 },
            {"G",       400 },
            {"Q",       500 }
        });

        public static readonly string[] NUMERAL_OPTIONS =
        {
            "M", "CM", "D", "Q", "CD", "P", "G", "C", "XC", "L", "F", "XL", "IIXX", "XIIX", "O", "X", "IX", "V", "IV", "I"
        };

        public static readonly string[] SUBTRACTIVE_NOTATION =
        {
            "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"
        };

        public static readonly string[] ADDITIVE_NOTATION =
        {
            "M", "D", "C", "L", "X", "V", "I"
        };

        private readonly int _number;

        public int Number => _number;

        public Roman(int number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Number should be positive.");
            }

            _number = number;
        }

        public override string ToString()
        {
            return ToString(RomanNumeralNotation.Substractive);
        }

        public string ToString(RomanNumeralNotation notation)
        {
            if (Number == 0)
            {
                return NULLA;
            }

            string[] numerals;
            switch (notation)
            {
                case RomanNumeralNotation.Additive:
                    numerals = ADDITIVE_NOTATION;
                    break;
                default:
                    numerals = SUBTRACTIVE_NOTATION;
                    break;
            }

            var resultRomanNumeral = "";

            var position = 0;

            var value = Number;

            do
            {
                var numeral = numerals[position];
                var numeralValue = VALUES[numeral];

                if (value >= numeralValue)
                {
                    value -= numeralValue;

                    resultRomanNumeral += numeral;

                    bool isMultipleNumeral = numeral.Length > 1;
                    if (isMultipleNumeral)
                    {
                        position++;
                    }

                    continue;
                }

                position++;
            }
            while (value != 0);


            return resultRomanNumeral;
        }

        public int ToInt()
        {
            string roman = ToString(RomanNumeralNotation.Substractive);
            return RomanNumbers.From(roman);
        }

        private static bool IsNumeral(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }

            return Parse(str) != null;
        }

        public static Roman Parse(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new Roman(0);
            }

            //upper case the string
            var strToRead = str.ToUpper();

            if (strToRead == NULLA)
            {
                return new Roman(0);
            }

            if (strToRead.EndsWith("J"))
            {
                strToRead = strToRead.Substring(0, strToRead.Length - 1) + "I";
            }

            strToRead = strToRead.Replace("U", "V");

            if (VALUES.ContainsKey(str))
            {
                return new Roman(VALUES[str]);
            }

            var resultNumber = 0;

            var numeralOptionPointer = 0;

            while (!String.IsNullOrEmpty(strToRead) && numeralOptionPointer < NUMERAL_OPTIONS.Length)
            {
                var numeral = NUMERAL_OPTIONS[numeralOptionPointer];

                if (!strToRead.StartsWith(numeral))
                {
                    numeralOptionPointer++;
                    continue;
                }

                var value = VALUES[numeral];
                resultNumber += value;

                strToRead = strToRead.Substring(numeral.Length);


                if (numeral.Length > 1)
                {
                    numeralOptionPointer++;
                }
            }

            if (String.IsNullOrEmpty(strToRead))
            {
                return new Roman(resultNumber);
            }

            return null;
        }

        public static int operator +(int r1, Roman r2)
        {
            var r = new Roman(r1) + r2;
            return r.Number;
        }

        public static string operator +(string r1, Roman r2)
        {
            var r = Roman.Parse(r1) + r2;
            return r.ToString();
        }

        public static Roman operator +(Roman r1, string r2)
        {
            return r1 + Roman.Parse(r2);
        }

        public static Roman operator +(Roman r1, int r2)
        {
            var n = r1.Number + r2;
            return new Roman(n);
        }

        public static Roman operator +(Roman r1, Roman r2)
        {
            if (r1 == null || r2 == null)
            {
                return NULLA;
            }
            else
            {
                var n = r1.Number + r2.Number;
                return new Roman(n);
            }
        }


        public static int operator -(int r1, Roman r2)
        {
            var r = new Roman(r1) - r2;
            return r.Number;
        }

        public static string operator -(string r1, Roman r2)
        {
            var r = Roman.Parse(r1) + r2;
            return r.ToString();
        }

        public static Roman operator -(Roman r1, Roman r2)
        {
            var n = r1.Number - r2.Number;

            if (n < 0)
            {
                n = 0;
            }

            return new Roman(n);
        }

        public static bool operator ==(Roman left, Roman right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Roman left, Roman right)
        {
            return !left.Equals(right);
        }

        public static implicit operator int(Roman r)
        {
            return (r?.Number).GetValueOrDefault();
        }

        public static implicit operator string(Roman r)
        {
            return r?.ToString();
        }

        public static implicit operator Roman(int r)
        {
            return new Roman(r);
        }

        public static implicit operator Roman(string r)
        {
            return Parse(r);
        }

        public override int GetHashCode()
        {
            return 207501131 ^ this.Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as Roman;

            if (item is not Roman)
            {
                return false;
            }

            return this.Number.Equals(item.Number);
        }
    }

    public enum RomanNumeralNotation
    {
        Substractive = 0,
        Additive = 1
    }
}