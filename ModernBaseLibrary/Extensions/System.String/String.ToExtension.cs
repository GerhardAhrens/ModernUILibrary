/*
 * <copyright file="StringExtension.cs" company="Lifeprojects.de">
 *     Class: StringExtension
 *     Copyright � Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 15:26:02</date>
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

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class StringExtension
    {
        /// <summary>
        /// Es wird gepr�ft ob der �bergebene String einem Bool-Wert entspricht<br/>
        /// G�ltige Werte f�r True: 1,y,yes,true,ja, j, wahr<br/>
        /// G�ltige Werte f�r False: 0,n,no,false,nein,falsch<br/>
        /// Gro�- und Kleinschrebung wird ignoriert<br/>
        /// </summary>
        /// <param name="this">�bergebener String</param>
        /// <param name="ignorException">True = es wird keine Exception bei einem falschen Wert ausgel�st,<br/>False = Es wird eine InvalidCastException alsgel�st bei einem Fehler</param>
        /// <returns>Wenn der Wert einem entsprechendem Bool-Wert entspricht, wird True oder False zur�ckgegeben.</returns>
        public static bool ToBool(this string @this, bool ignorException = false)
        {
            string[] trueStrings = { "1", "y", "yes", "true", "ja", "j", "wahr" };
            string[] falseStrings = { "0", "n", "no", "false", "nein", "falsch" };

            if (string.IsNullOrEmpty(@this) == true)
            {
                return false;
            }

            if (trueStrings.Contains(@this.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (falseStrings.Contains(@this.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (ignorException == true)
            {
                return false;
            }
            else
            {
                string msg = "only the following are supported for converting strings to boolean: ";
                throw new InvalidCastException($"{msg} {string.Join(",", trueStrings)} and {string.Join(",", falseStrings)}");
            }
        }

        /// <summary>
        ///     A string extension method that converts the @this to a file information.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a FileInfo.</returns>
        public static FileInfo ToFileInfo(this string @this)
        {
            return new FileInfo(@this);
        }

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray(this string @this)
        {
            Encoding encoding = Activator.CreateInstance<ASCIIEncoding>();
            return encoding.GetBytes(@this);
        }

        /// <summary>
        /// Splits a string into a NameValueCollection, where each "namevalue"
        /// is separated by the "OuterSeparator". The parameter "NameValueSeparator"
        /// sets the split between Name and Value.
        /// </summary>
        /// <param name="self">String to process</param>
        /// <param name="outerSeparator">Separator for each "NameValue"</param>
        /// <param name="nameValueSeparator">Separator for Name/Value splitting</param>
        /// <returns>NameValueCollection of values.</returns>
        /// <example>
        /// Example:
        ///     string str = "param1=value1;param2=value2";
        ///     NameValueCollection nvOut = str.ToNameValueCollection(';', '=');
        ///
        /// The result is a NameValueCollection where:
        ///     key[0] is "param1" and value[0] is "value1"
        ///     key[1] is "param2" and value[1] is "value2"
        /// </example>
        public static NameValueCollection ToNameValueCollection(this string @this, char outerSeparator, char nameValueSeparator)
        {
            NameValueCollection nvText = null;
            @this = @this.TrimEnd(outerSeparator);

            if (!string.IsNullOrEmpty(@this))
            {
                string[] arrStrings = @this.TrimEnd(outerSeparator).Split(outerSeparator);
                for (int i = 0; i < arrStrings.Length; i++)
                {
                    string s = arrStrings[i];
                    int posSep = s.IndexOf(nameValueSeparator);
                    string name = s.Substring(0, posSep);
                    string value = s.Substring(posSep + 1);
                    if (nvText == null)
                    {
                        nvText = new System.Collections.Specialized.NameValueCollection();
                    }

                    nvText.Add(name, value);
                }
            }

            return nvText;
        }

        public static string ToCamelCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            if (@this.Length < 2)
            {
                return @this;
            }

            string[] words = @this.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
            }

            return result;
        }

        public static string ToCapitalize(this string @this)
        {
            return ToCapitalize(@this, CultureInfo.CurrentCulture);
        }

        public static string ToCapitalize(this string @this, CultureInfo culture)
        {
            culture.IsArgumentNull(nameof(culture));

            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            if (@this.Length == 0)
            {
                return string.Empty;
            }

            TextInfo textInfo = culture.TextInfo;

            return textInfo.ToTitleCase(@this);
        }

        public static string ToCapitalizeWords(this string input, params string[] dontCapitalize)
        {
            char[] delimiter = new char[] { ' ', '.', '-' };
            var split = input.Split(delimiter);
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = i == 0
                  ? ToCapitalize(split[i])
                  : dontCapitalize.Contains(split[i])
                     ? split[i]
                     : ToCapitalize(split[i]);
            }

            return string.Join(" ", split);
        }

        public static HashSet<char> ToCharListToSet(this string @this)
        {
            HashSet<char> set = new HashSet<char>();

            for (int i = 0; i < @this.Length; i++)
            {
                if ((i + 1) < @this.Length && @this[i + 1] == '-')
                {
                    char startChar = @this[i++];
                    i++;
                    char endChar = (char)0;
                    if (i < @this.Length)
                    {
                        endChar = @this[i++];
                    }

                    for (int j = startChar; j <= endChar; j++)
                    {
                        set.Add((char)j);
                    }
                }
                else
                {
                    set.Add(@this[i]);
                }
            }

            return set;
        }

        public static Dictionary<string, string> ToDictionary(this string @this, string separator = "|")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            NameValueCollection collection = ToNameValueCollection(@this, separator);
            foreach (string key in collection.AllKeys)
            {
                dic.Add(key, collection[key]);
            }

            return dic;
        }

        public static NameValueCollection ToNameValueCollection(this string @this, string separator)
        {
            if (string.IsNullOrEmpty(separator) == true)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            NameValueCollection collection = new NameValueCollection();

            string[] nameValuePairs = @this.Split(separator.ToCharArray());

            foreach (string nvs in nameValuePairs)
            {
                string[] nvp = nvs.Split("=".ToCharArray());

                string name = nvp[0].Trim();
                string value = nvp.Length > 1 ? nvp[1].Trim() : string.Empty;

                if (name.Length > 0)
                {
                    collection.Add(name, value);
                }
            }

            return collection;
        }

        public static NameValueCollection ToNameValueCollection(this string s)
        {
            return ToNameValueCollection(s, "|");
        }

        public static List<string> ToList(this string @this, string separator = "|")
        {
            List<string> list = new List<string>();

            foreach (string e in @this.Split(separator.ToCharArray()))
            {
                list.Add(e.Trim());
            }

            return list;
        }

        public static string ToPascalCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            if (@this.Length <= 2)
            {
                return @this.ToUpper();
            }

            string[] words = @this.ToLower().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            string result = "";
            foreach (string word in words)
            {
                result += word.Substring(0, 1).ToUpper() + word.Substring(1);
            }

            return result;
        }

        public static string ToProperCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            if (@this.Length <= 2)
            {
                return @this.ToUpper();
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this);
        }

        public static string ToProperCase(this string @this, char separator)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            if (@this.Length <= 2)
            {
                return @this.ToUpper();
            }

            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this).Replace(separator.ToString(),string.Empty);

            return result;
        }

        public static string ToInitials(this string @this, bool withNumber = true)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return "??";
            }

            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(@this, @"[\p{P}\p{S}\p{C}]+", "");

            if (withNumber == true)
            {
                initials = Regex.Replace(initials, @"[\p{N}]+", "");
            }

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            // Extract up to 2 initials from the remaining cleaned name.
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

            if (initials.Length > 2)
            {
                // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            return initials.ToUpperInvariant();
        }

        public static string ToInitialsEx(this string @this, int MaxLength = 3)
        {
            char space = ' ';

            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }


            StringBuilder sb = new StringBuilder();
            string[] words = @this.Split(space);

            if (words.Length <= 3)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    sb.Append(words[i].Substring(0, 1));
                }
            }
            else
            {

            }

            return sb.ToString().ToUpper();
        }

        public static string ToTitleCase(this string @this)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this.ToLower());
        }

        #region Numeric Converter
        public static float ToFloat(this string source, string culture = "de-DE")
        {
            CultureInfo cultureInfo = null;

            if (string.IsNullOrEmpty(culture) == false)
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            }
            else
            {
                cultureInfo = CultureInfo.CreateSpecificCulture("de-DE");
            }

            float result = 0;
            if (string.IsNullOrEmpty(source) == true)
            {
                return result;
            }
            else
            {
                float outNumber;
                if (float.TryParse(source, NumberStyles.Any, cultureInfo, out outNumber) == true)
                {
                    result = outNumber;
                }
            }

            return result;
        }

        public static double ToDouble(this string @this, string culture = "de-DE")
        {
            CultureInfo cultureInfo = null;

            if (string.IsNullOrEmpty(culture) == false)
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            }
            else
            {
                cultureInfo = CultureInfo.CreateSpecificCulture("de-DE");
            }

            double result = 0;
            if (string.IsNullOrEmpty(@this) == true)
            {
                return 0;
            }
            else
            {
                double outNumber;
                if (double.TryParse(@this, NumberStyles.Any, cultureInfo, out outNumber) == true)
                {
                    result = outNumber;
                }
            }

            return result;
        }

        public static decimal ToDecimal(this string @this, string culture = "de-DE")
        {
            CultureInfo cultureInfo = null;

            if (string.IsNullOrEmpty(culture) == false)
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            }
            else
            {
                cultureInfo = CultureInfo.CreateSpecificCulture("de-DE");
            }

            decimal result = 0;
            if (string.IsNullOrEmpty(@this) == true)
            {
                return 0;
            }
            else
            {
                decimal outNumber;
                if (decimal.TryParse(@this, NumberStyles.Any, cultureInfo, out outNumber) == true)
                {
                    result = outNumber;
                }
            }

            return result;
        }

        public static int ToInt(this string @this, NumberStyles numberStyles = NumberStyles.Any)
        {

            int outValue = 0;
            if (int.TryParse(@this, numberStyles, CultureInfo.CurrentCulture, out outValue) == true)
            {
                return outValue;
            }
            else
            {
                return 0;
            }
        }
        #endregion Numeric Converter
    }
}
