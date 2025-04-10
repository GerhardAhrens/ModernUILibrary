/*
 * <copyright file="StringExtension.cs" company="Lifeprojects.de">
 *     Class: StringExtension
 *     Copyright © Lifeprojects.de 2022
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
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Text.RegularExpressions;

    [SupportedOSPlatform("windows")]
    public static partial class StringExtension
    {
        public static bool In(this string @this, params string[] values)
        {
            bool result = false;

            for (int i = 0; i < values.Length; i++)
            {
                if (@this == values[i].ToString())
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool In(this string @this, params Enum[] values)
        {
            bool result = false;

            for (int i = 0; i < values.Length; i++)
            {
                if (@this.Contains(values[i].ToString(), StringComparison.OrdinalIgnoreCase) == true)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool NotIn(this string @this, params string[] values)
        {
            bool result = true;

            for (int i = 0; i < values.Length; i++)
            {
                if (@this == values[i].ToString())
                {
                    result = false;
                }
            }

            return result;
        }

        public static TResult GetAs<TResult>(this string @this)
        {
            try
            {
                object getAs = null;

                if (typeof(TResult).Name == "Guid")
                {
                    getAs = @this == null ? Guid.Empty : new Guid(@this.ToString());
                }
                else if (typeof(TResult).IsEnum == true)
                {
                    if (@this.ToInt().GetType() == typeof(int))
                    {
                        getAs = (TResult)Enum.ToObject(typeof(TResult), @this.ToInt());
                    }
                    else if (@this.GetType() == typeof(string))
                    {
                        getAs = (TResult)Enum.Parse(typeof(TResult), @this.ToString(), true);
                    }
                }
                else
                {
                    getAs = @this == null ? default(TResult) : (TResult)Convert.ChangeType(@this, typeof(TResult), CultureInfo.InvariantCulture);
                }

                return (TResult)getAs;
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                return default(TResult);
            }
        }

        public static string AddWhenNotFound(this string @this, string addString)
        {
            string result = string.Empty;

            if (@this.EndsWith(addString, StringComparison.InvariantCultureIgnoreCase) == false)
            {
                result = $"{@this}{addString}";
            }
            else
            {
                result = @this;
            }

            return result;
        }

        /// <summary>
        /// Erfernt alle NewLine, Return, CrLf aus einem string
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>String</returns>
        public static string RemoveNewLines(this string @this)
        {
            return @this.Replace("\n", string.Empty, StringComparison.Ordinal)
                        .Replace("\r", string.Empty, StringComparison.Ordinal)
                        .Replace("\\r\\n", string.Empty, StringComparison.Ordinal);
        }

        /// <summary>
        /// Die Methode erstellt aus einem String eine List<string> unter Angabe 
        /// eines Separator '\n' als Default.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="separator">Separator '\n' als Default</param>
        /// <returns></returns>
        public static List<string> ToLines(this string @this, char separator = '\n')
        {
            List<string> result = null;

            if (string.IsNullOrEmpty(@this) == false)
            {
                string[] lines = @this.Split(separator);
                result = new List<string>(lines);
            }

            return result;
        }

        /// <summary>
        ///     A string extension method that line break 2 newline.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string BrToNl(this string @this)
        {
            return @this.Replace("<br />", "\r\n").Replace("<br>", "\r\n");
        }

        /// <summary>
        /// Check that the given string is in a list of potential matches.
        /// </summary>
        /// <returns><c>true</c>, if any was equalsed, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        /// <param name="args">Arguments.</param>
        /// <remarks>Inspired by StackOverflow answer http://stackoverflow.com/a/20644611/23199</remarks>
        /// <example>
        /// string custName = "foo";
        /// bool isMatch = (custName.EqualsAny("bar", "baz", "FOO"));
        /// </example>
        public static bool EqualsAny(this string str, params string[] args)
        {
            return args.Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, str));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="this">String</param>
        /// <param name="n">Anzahl der wiederholten Zeichen</param>
        /// <returns>Zeichenkette mit den wiederholten Zeichen</returns>
        public static string Repeat(this string @this, int n)
        {
            return string.Concat(Enumerable.Repeat(@this, n));
        }

        /// <summary>
        /// Wiederholt ein Zeichen 'n' mal
        /// </summary>
        /// <param name="@this">Char</param>
        /// <param name="n">Anzahl der wiederholten Zeichen</param>
        /// <returns>Zeichenkette mit den wiederholten Zeichen</returns>
        public static string Repeat(this char @this, int n)
        {
            return new String(@this, n);
        }

        /// <summary>
        /// Prüft ob ein ein Start- und End String im Source String vorhanden ist und gibt alle vorkommen als Liste zurück
        /// </summary>
        /// <param name="this">Source String</param>
        /// <param name="startString">Start String</param>
        /// <param name="endString">End String</param>
        /// <param name="isWithSeparator">True, String mit Start- und End String</param>
        /// <returns>Liste von Token zwischen einem Start- und End string</returns>
        public static IEnumerable<string> BetweenToken(this string @this, string startString, string endString, bool isWithSeparator = false)
        {
            if (@this == null || startString == null || endString == null)
            {
                yield return null;
            }

            Regex r = new Regex(Regex.Escape(startString) + "(.*?)" + Regex.Escape(endString));
            MatchCollection matches = r.Matches(@this);
            foreach (Match match in matches)
            {
                if (isWithSeparator == false)
                {
                    yield return match.Groups[1].Value;
                }
                else
                {
                    yield return $"{startString}{match.Groups[1].Value}{endString}";
                }
            }
        }

        /// <summary>
        /// Gibt den String ab dem Startzeichen 'stripAfter' zurück
        /// </summary>
        /// <param name="this"></param>
        /// <param name="stripAfter"></param>
        /// <returns>Reststring ab 'stripAfter'</returns>
        public static string StripStartingWith(this string @this, string stripAfter)
        {
            if (@this == null)
            {
                return null;
            }

            var indexOf = @this.IndexOf(stripAfter, StringComparison.Ordinal);
            if (indexOf > -1)
            {
                return @this.Substring(0, indexOf);
            }

            return @this;
        }

        /// <summary>
        /// Die Methode zählt die Anzahl der Token im übergeben String
        /// </summary>
        /// <param name="this">Übergebener String</param>
        /// <param name="token">Auf das übergeben Zeichen prüfen</param>
        /// <returns>Anzahl der Token</returns>
        public static int CountToken(this string @this, char token)
        {
            return @this.Count(f => f == token);
        }

        /// <summary>
        /// Die Methode zählt die Anzahl der Token im übergeben String
        /// </summary>
        /// <param name="this">Übergebener String</param>
        /// <returns>Anzahl der Token</returns>
        public static int CountToken(this string @this)
        {
            return @this.CountToken();
        }

        /// <summary>
        /// Die Methode wandelt Dialkritische Zeichen z.B. Ä nach AE um.
        /// </summary>
        /// <param name="this">Zu prüfender String</param>
        /// <param name="IsUpper">Alles in Großbuchstaben zurück geben</param>
        /// <returns>String mit umgewandelte Umlaute</returns>
        public static string ConvertDiacriticsGER(this string @this, bool IsUpper = false)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return string.Empty;
            }

            string result = string.Empty;

            if (IsUpper == false)
            {
                result = Regex.Replace(@this, "Ü(?=[a-zäöüß])", "Ue");
                result = Regex.Replace(result, "Ö(?=[a-zäöüß])", "Oe");
                result = Regex.Replace(result, "Ä(?=[a-zäöüß])", "Ae");

                result = result.Replace("Ü", "UE")
                               .Replace("Ö", "OE")
                               .Replace("Ä", "AE");

                result = result.Replace("ü", "ue")
                               .Replace("ö", "oe")
                               .Replace("ä", "ae")
                               .Replace("ß", "SS");
            }
            else
            {
                result = Regex.Replace(@this.ToUpper(), "Ü", "UE")
                              .Replace("Ö", "OE")
                              .Replace("Ä", "AE")
                              .Replace("ß", "SS");
            }

            return result;
        }


        /// <summary>
        /// Die Methode entfernt Diakritische Zeichen aus einem String
        /// </summary>
        /// <param name="this">Zu prüfender String</param>
        /// <returns>String ohne Diakritische Zeichen</returns>
        public static string RemoveDiacritics(this string @this)
        {
            string normalizedString = @this.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        public static string TruncateLeft(this string @this, int maxChars, string addText = "")
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(@this) == true)
            {
                return result;
            }

            if ((maxChars - addText.Length) < 1)
            {
                return result;
            }

            if (@this.Length > (maxChars - addText.Length))
            {
                result = @this.Substring(@this.Length - (maxChars - addText.Length));
            }
            else
            {
                result = @this;
            }

            if (string.IsNullOrEmpty(addText) == false)
            {
                return $"{addText}{result}";
            }


            return result;
        }

        public static string Truncate(this string @this, int length)
        {
            return Truncate(@this, length, string.Empty, false);
        }

        /// <summary>
        /// Truncates the input string given to the length specified and possibly adds an ellipsis at the end to mark a truncation
        /// </summary>
        /// <param name="this">The input string to truncate</param>
        /// <param name="length">The desired maximum length of the resulting string</param>
        /// <param name="ellipsis">An ellipsis to append to the end of a string when it gets truncated or null if no ellipsis is required</param>
        /// <returns>The input string possibly truncated at the desired length with the ellipsis added. The length of the resulting string will never exceed the length specified</returns>

        public static string Truncate(this string @this, int length, string ellipsis)
        {
            return Truncate(@this, length, ellipsis, true);
        }

        /// <summary>
        /// Truncates the input string given to the length specified and possibly adds an ellipsis at the end to mark a truncation
        /// </summary>
        /// <param name="this">The input string to truncate</param>
        /// <param name="length">The desired maximum length of the resulting string</param>
        /// <param name="ellipsis">An ellipsis to append to the end of a string when it gets truncated or null if no ellipsis is required</param>
        /// <param name="inclusiveEllipsis">True if the ellipsis should be taken into account when checking for the length. 
        /// If false, the input string will be cut of at the length specified and the ellipsis will be added even if that means the resulting string will be longer than the desired length</param>
        /// <returns>The input string possibly truncated at the desired length with the ellipsis added</returns>

        public static string Truncate(this string @this, int length, string ellipsis, bool inclusiveEllipsis)
        {
            return Truncate(@this, length, ellipsis, inclusiveEllipsis, null, false, StringComparison.Ordinal);
        }

        /// <summary>
        /// Truncates the input string given to the length specified and possibly adds an ellipsis at the end to mark a truncation
        /// </summary>
        /// <param name="this">The input string to truncate</param>
        /// <param name="length">The desired maximum length of the resulting string</param>
        /// <param name="ellipsis">An ellipsis to append to the end of a string when it gets truncated or null if no ellipsis is required</param>
        /// <param name="inclusiveEllipsis">True if the ellipsis should be taken into account when checking for the length. 
        /// If false, the input string will be cut of at the length specified and the ellipsis will be added even if that means the resulting string will be longer than the desired length</param>
        /// <param name="boundary">A string (e.g. space) on which to break.</param>
        /// <param name="emptyOnNoBoundary">Determines the default behavior When no boundary is found. (Empty string or truncate without boundary)</param>
        /// <param name="comparisonType">The way boundary should be compared to the input string</param>
        /// <returns>The input string possibly truncated at the desired length with the ellipsis added</returns>

        public static string Truncate(this string @this, int length, string ellipsis, bool inclusiveEllipsis, string boundary, bool emptyOnNoBoundary, StringComparison comparisonType)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "length cant be smaller than 0");
            }

            // the length of ellipsis might not be larger than the desired length of the resulting string when inclusiveEllipsis is set
            if (ellipsis != null)
            {
                if (inclusiveEllipsis)
                {
                    if (ellipsis.Length > length)
                    {
                        throw new ArgumentException("Ellipsis cant be larger than the desired length when inclusiveEllipsis is set", "ellipsis");
                    }
                }
            }

            string result = @this;

            if (@this.Length > length)
            {
                int checkLength = length;

                if (inclusiveEllipsis && !string.IsNullOrEmpty(ellipsis))
                {
                    // ensure that we leave space for the ellipsis
                    checkLength -= ellipsis.Length;
                }
                if (!string.IsNullOrEmpty(boundary))
                {
                    int boundaryIndex = @this.LastIndexOf(boundary, checkLength, checkLength, comparisonType);
                    if (boundaryIndex != -1)
                    {
                        int boundaryLength = boundaryIndex; // we want to stop right before the boundary starts so we can use the index of the boundary as the length.

                        result = @this.Left(boundaryLength);
                    }
                    else
                    {
                        if (emptyOnNoBoundary)
                        {
                            result = string.Empty;
                        }
                        else
                        {
                            result = @this.Left(length);
                        }
                    }
                }
                else
                {
                    result = @this.Left(checkLength);
                }

                if (!string.IsNullOrEmpty(ellipsis))
                {
                    result += ellipsis;
                }
            }
            else
            {
                if (!inclusiveEllipsis)
                {
                    if (ellipsis != null)
                    {
                        result += ellipsis;
                    }
                }
            }

            return result;
        }

        public static string FirstWord(this string @this, char separator = ';')
        {
            string result = string.Empty;
            if (@this.Contains(separator) == true)
            {
                string[] words = @this.Split(separator);
                result = words[0].Trim();
            }
            else
            {
                result = @this;
            }

            return result;
        }

        public static string LastWord(this string @this, char separator = ';')
        {
            string result = string.Empty;
            if (@this.Contains(separator) == true)
            {
                string[] words = @this.Split(separator);
                result = words[words.Length - 1].Trim();
            }
            else
            {
                result = @this;
            }

            return result;
        }

        public static char LastChar(this string @this)
        {
            return @this.Last();
        }

        public static string HTMLTagTextBold(this string @this)
        {
            return string.Format("<b>{0}</b>", @this);
        }

        public static string Trim(this string @this, string trimString = "")
        {
            return @this.Trim(trimString.ToCharArray());
        }

        public static StringBuilder DuplicateCharacter(this string @this)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder duplicateChar = new StringBuilder();

            foreach (var item in @this)
            {
                if (result.ToString().IndexOf(item.ToString().ToLower()) == -1)
                {
                    result.Append(item);
                }
                else
                {
                    duplicateChar.Append(item);
                }
            }

            return duplicateChar;
        }

        public static StringBuilder UniqueCharFromString(this string @this)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder uniqueChar = new StringBuilder();

            foreach (var item in @this)
            {
                if (result.ToString().IndexOf(item.ToString().ToLower()) == -1)
                {
                    result.Append(item);
                }
                else
                {
                    uniqueChar.Append(item);
                }
            }

            return result;
        }

        public static int Count(this string @this, string token, bool ignorCase = false)
        {
            int result = 0;
            RegexOptions opt = RegexOptions.None;

            if (ignorCase == false)
            {
                opt = RegexOptions.IgnoreCase;
            }
            else
            {
                opt = RegexOptions.None;
            }

            result = Regex.Matches(@this, token, opt).Count;

            return result;
        }

        public static string EscapeForXaml(this string @this)
        {
            return @this?.Replace("[apos]", "'")
                .Replace("[quot]", "\"")
                .Replace("[lt]", "<")
                .Replace("[gt]", ">")
                .Replace("[amp]", "&")
                .Replace("[br]", "\r\n");
        }

        public static void Out(this string @this)
        {
            if (string.IsNullOrEmpty(@this) == false)
            {
                TextWriter writer = System.Console.Out;
                writer.Write(@this);
            }
        }

        public static MemoryStream ToStream(this string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("String value cannot be empty.", "value");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            return new MemoryStream(encoding.GetBytes(value));
        }

        public static IEnumerable<string> GetTextElements(this string @this)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(@this);
            while (enumerator.MoveNext())
            {
                yield return (string)enumerator.Current;
            }
        }

        public static string Ellipsis(this string @this, int maxLength, string ellipsisString)
        {
            if (maxLength < ellipsisString.Length)
            {
                throw new ArgumentOutOfRangeException($"(Max.Length: {ellipsisString.Length})");
            }

            if (@this.Length <= maxLength)
            {
                return @this;
            }

            return @this.Substring(0, maxLength - ellipsisString.Length) + ellipsisString;
        }

        public static string Ellipsis(this string @this, int maxLength)
        {
            const string ellipsisString = "...";

            if (maxLength < ellipsisString.Length)
            {
                throw new ArgumentOutOfRangeException($"(Max.Length: {ellipsisString.Length})");
            }

            return @this.Ellipsis(maxLength, ellipsisString);
        }

        public static char CharAt(this string @this, int index)
        {
            return index < @this.Length ? @this[index] : '\0';
        }

        public static string Capitalize(this string @this)
        {
            return Capitalize(@this, CultureInfo.CurrentCulture);
        }

        public static string Capitalize(this string @this, CultureInfo culture)
        {
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

        public static string CapitalizeWords(this string input, params string[] dontCapitalize)
        {
            char[] delimiter = new char[] { ' ', '.', '-' };
            var split = input.Split(delimiter);
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = i == 0
                  ? Capitalize(split[i])
                  : dontCapitalize.Contains(split[i])
                     ? split[i]
                     : Capitalize(split[i]);
            }

            return string.Join(" ", split);
        }

        public static string Reverse(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return string.Empty;
            }

            return new string(@this.Select((c, index) => new { c, index })
                                         .OrderByDescending(x => x.index)
                                         .Select(x => x.c)
                                         .ToArray());
        }

        public static IEnumerable<string> Between(this string @this, string startString, string endString, bool isWithSeparator = false)
        {
            if (@this == null || startString == null || endString == null)
            {
                yield return null;
            }

            Regex r = new Regex(Regex.Escape(startString) + "(.*?)" + Regex.Escape(endString));
            MatchCollection matches = r.Matches(@this);
            foreach (Match match in matches)
            {
                if (isWithSeparator == false)
                {
                    yield return match.Groups[1].Value;
                }
                else
                {
                    yield return $"{startString}{match.Groups[1].Value}{endString}";
                }
            }
        }

        public static string FirstCharUpper(this string @this)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this.ToLower());

            return result;
        }

        /// <summary>
        ///     Convert url query string to IDictionary value key pair
        /// </summary>
        /// <param name="queryString">query string value</param>
        /// <returns>IDictionary value key pair</returns>
        public static IDictionary<string, string> QueryStringToDictionary(this string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return null;
            }
            if (!queryString.Contains("?"))
            {
                return null;
            }
            string query = queryString.Replace("?", "");
            if (!query.Contains("="))
            {
                return null;
            }
            return query.Split('&').Select(p => p.Split('=')).ToDictionary(key => key[0].ToLower().Trim(), value => value[1]);
        }

        /// <summary>
        ///     Reverse back or forward slashes
        /// </summary>
        /// <param name="val">string</param>
        /// <param name="direction">
        ///     0 - replace forward slash with back
        ///     1 - replace back with forward slash
        /// </param>
        /// <returns></returns>
        public static string ReverseSlash(this string val, int direction)
        {
            switch (direction)
            {
                case 0:
                    return val.Replace(@"/", @"\");
                case 1:
                    return val.Replace(@"\", @"/");
                default:
                    return val;
            }
        }

        /// <summary>
        ///     Replace Line Feeds
        /// </summary>
        /// <param name="val">string to remove line feeds</param>
        /// <returns>System.string</returns>
        public static string ReplaceLineFeeds(this string val)
        {
            return Regex.Replace(val, @"^[\r\n]+|\.|[\r\n]+$", "");
        }

        /// <summary>
        ///     Validates if a string is valid IPv4
        ///     Regular expression taken from <a href="http://regexlib.com/REDetails.aspx?regexp_id=2035">Regex reference</a>
        /// </summary>
        /// <param name="val">string IP address</param>
        /// <returns>true if string matches valid IP address else false</returns>
        public static bool IsValidIPv4(this string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return Regex.Match(val,
                @"(?:^|\s)([a-z]{3,6}(?=://))?(://)?((?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?))(?::(\d{2,5}))?(?:\s|$)")
                .Success;
        }

        /// <summary>
        ///     Calculates the amount of bytes occupied by the input string encoded as the encoding specified
        /// </summary>
        /// <param name="val">The input string to check</param>
        /// <param name="encoding">The encoding to use</param>
        /// <returns>The total size of the input string in bytes</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentNullException">encoding is null</exception>
        public static int GetByteSize(this string val, Encoding encoding)
        {
            if (val == null)
            {
                throw new ArgumentNullException("val");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            return encoding.GetByteCount(val);
        }

        public static List<string> Split(this string value, string regex, RegexOptions options)
        {
            var parts = new List<string>();
            int pos = 0;
            foreach (Match match in Regex.Matches(value, regex, options))
            {
                parts.Add(value.Substring(pos, match.Index - pos));
                parts.Add(match.Value);
                pos = match.Index + match.Length;
            }

            parts.Add(value.Substring(pos));
            return parts;
        }

        public static bool IsValidRegex(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return false;
            }

            try
            {
                Regex.Match("", @this);
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal static string RegexWrap(this string inString)
        {
            return string.Format("({0})", inString);
        }

        internal static string ToRegex(this List<string> @this)
        {
            string regex = string.Empty;
            foreach (string word in @this)
            {
                if (regex.Length > 0)
                {
                    regex += "|";
                }

                regex += word.RegexWrap();
            }

            return regex;
        }

        /// <summary>
        /// Extracts the left part of the input string limited with the length parameter
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="length">The amount of characters to take from the input string</param>
        /// <returns>The substring starting at startIndex 0 until length</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Left(this string @this, int length)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (length < 0 || length > @this.Length)
            {
                throw new ArgumentOutOfRangeException("length", "length cannot be higher than the amount of available characters in the input or lower than 0");
            }

            string result = @this.Substring(0, length);

            return result;
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first character
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string LeftOf(this string @this, char character)
        {
            return LeftOf(@this, character, 0);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first character
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <param name="skip">The numbers of found characters to skip before taking the left part</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string LeftOf(this string @this, char character, int skip)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");
            }

            string result;

            if (@this.Length == 0)
            {
                result = @this;
            }
            else
            {
                int characterPosition = 0;
                int charactersFound = -1;

                while (charactersFound < skip)
                {
                    characterPosition = @this.IndexOf(character, characterPosition + 1);
                    if (characterPosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        charactersFound++;
                    }
                }

                if (characterPosition == -1)
                {
                    result = @this;
                }
                else
                {
                    result = @this.Substring(0, characterPosition);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string @this, string value)
        {
            return LeftOf(@this, value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string @this, string value, StringComparison comparisonType)
        {
            return LeftOf(@this, value, 0, comparisonType);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the n'th occurrence of value
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="skip">The numbers of found values to skip before taking the left part</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the n'th occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string @this, string value, int skip, StringComparison comparisonType)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");
            }

            string result;

            if (@this.Length <= skip)
            {
                result = @this;
            }
            else
            {
                int valuePosition = 0;
                int valuesFound = -1;

                while (valuesFound < skip)
                {
                    valuePosition = @this.IndexOf(value, valuePosition + 1, comparisonType);
                    if (valuePosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        valuesFound++;
                    }
                }

                if (valuePosition == -1)
                {
                    result = @this;
                }
                else
                {
                    result = @this.Substring(0, valuePosition);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the last character
        /// </summary>
        /// <param name="@this">The input string to take the left part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string LeftOfLast(this string @this, char character)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            string result;
            int lastCharacterPosition = @this.LastIndexOf(character);

            if (lastCharacterPosition == -1)
            {
                result = @this;
            }
            else
            {
                result = @this.Substring(0, lastCharacterPosition);
            }

            return result;
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the last occurrence of value
        /// </summary>
        /// <param name="this">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the last occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOfLast(this string @this, string value)
        {
            return LeftOfLast(@this, value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the last occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the last occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOfLast(this string @this, string value, StringComparison comparisonType)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string result;
            int lastValuePosition = @this.LastIndexOf(value, comparisonType);

            if (lastValuePosition == -1)
            {
                result = @this;
            }
            else
            {
                result = @this.Substring(0, lastValuePosition);
            }

            return result;
        }

        /// <summary>
        /// Extracts the right part of the input string limited with the length parameter
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="length">The amount of characters to take from the input string</param>
        /// <returns>The substring taken from the input string</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Right(this string input, int length)
        {
            // preconditions
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (length < 0 || length > input.Length)
            {
                throw new ArgumentOutOfRangeException("length", "length cannot be higher than the amount of available characters in the input or lower than 0");
            }

            int startIndex = input.Length - length;
            string result = input.Substring(startIndex);

            return result;
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the first character
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string RightOf(this string input, char character)
        {
            return RightOf(input, character, 0);
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the first character
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <param name="skip">The numbers of found characters to skip before taking the right part</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string RightOf(this string input, char character, int skip)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");
            if (skip < 0)
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");

            string result;

            if (input.Length <= skip)
            {
                result = input;
            }
            else
            {
                int characterPosition = input.Length;
                int foundCharacters = -1;

                while (foundCharacters < skip)
                {
                    characterPosition = input.LastIndexOf(character, characterPosition - 1);
                    if (characterPosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        foundCharacters++;

                        if (characterPosition == 0)
                        {
                            break;
                        }
                    }
                }

                if (characterPosition == -1)
                {
                    result = input;
                }
                else
                {
                    result = input.Substring(characterPosition + 1);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string RightOf(this string input, string value)
        {
            return RightOf(input, value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string RightOf(this string input, string value, StringComparison comparisonType)
        {
            return RightOf(input, value, 0, comparisonType);
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the n'th occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="skip">The numbers of found values to skip before taking the right part</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string RightOf(this string input, string value, int skip, StringComparison comparisonType)
        {
            // preconditions
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");
            }

            string result;
            if (input.Length <= skip)
            {
                result = input;
            }
            else
            {
                int valuePosition = input.Length;
                int valuesFound = -1;

                while (valuesFound < skip)
                {
                    valuePosition = input.IndexOf(value, valuePosition, comparisonType);
                    if (valuePosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        valuesFound++;
                    }
                }

                if (valuePosition == -1)
                {
                    result = input.Substring(valuePosition + 1);
                }
                else
                {
                    result = input;
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the last character
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string RightOfLast(this string input, char character)
        {
            // preconditions
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            string result;
            int lastCharacterPosition = input.LastIndexOf(character);

            if (lastCharacterPosition == -1)
            {
                result = input;
            }
            else
            {
                result = input.Substring(lastCharacterPosition + 1);
            }

            return result;
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the last occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the last occurrence of value or the whole input string if the value was not found</returns>
        public static string RightOfLast(this string input, string value)
        {
            return RightOfLast(input, value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the right part of the input string limited by the last occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the right part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the last occurrence of value or the whole input string if the value was not found</returns>
        public static string RightOfLast(this string input, string value, StringComparison comparisonType)
        {
            // preconditions
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string result;
            int lastValuePosition = input.LastIndexOf(value, comparisonType);

            if (lastValuePosition == -1)
            {
                result = input;
            }
            else
            {
                result = input.Substring(0, lastValuePosition);
            }

            return result;
        }

        /// <summary>
        /// Removes the specified string from the trailing end of the input string, if there's a match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffixToRemove"></param>
        /// <param name="comparisonType"></param>
        /// <returns>The string with the specified suffix trimmed</returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType)
        {
            if (suffixToRemove == null)
            {
                return input;
            }

            return input?.EndsWith(suffixToRemove, comparisonType) ?? false
                ? input.Substring(0, input.Length - suffixToRemove.Length)
                : input;
        }

        /// <summary>
        /// <inheritdoc cref="TrimEnd(string,string,System.StringComparison)"/> using StringComparison.CurrentCulture
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffixToRemove"></param>
        /// <returns></returns>
        public static string TrimEnd(this string input, string suffixToRemove)
            => TrimEnd(input, suffixToRemove, StringComparison.Ordinal);

        /// <summary>
        /// Removes the specified string from the beginning of the input string, if there's a match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefixToRemove"></param>
        /// <param name="comparisonType"></param>
        /// <returns>The string with the specified prefix trimmed</returns>
        public static string TrimStart(this string input, string prefixToRemove, StringComparison comparisonType)
        {
            if (prefixToRemove == null)
            {
                return input;
            }

            return input?.StartsWith(prefixToRemove, comparisonType) ?? false
                ? input.Substring(prefixToRemove.Length, input.Length - prefixToRemove.Length)
                : input;
        }

        /// <summary>
        /// <inheritdoc cref="TrimStart(string,string,System.StringComparison)"/> using StringComparison.CurrentCulture
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefixToRemove"></param>
        /// <returns></returns>
        public static string TrimStart(this string input, string prefixToRemove)
            => TrimEnd(input, prefixToRemove, StringComparison.Ordinal);

        /// <summary>
        /// Removes the specified string from both the beginning and end of the input string, if there's a match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefixAndSuffixPrefixToRemove"></param>
        /// <param name="comparisonType"></param>
        /// <returns>The string with the specified string removed from both ends</returns>
        public static string Trim(this string input, string prefixAndSuffixPrefixToRemove, StringComparison comparisonType)
            => input.TrimStart(prefixAndSuffixPrefixToRemove, comparisonType).TrimEnd(prefixAndSuffixPrefixToRemove, comparisonType);

        // <summary>
        /// Taken from StackOverflow: https://stackoverflow.com/q/6309379
        ///
        /// Substantially faster (1251 ns) than wrapping with a try-catch and using `Convert` (20,281 ns)
        /// </summary>
        public static bool IsBase64(this string @this)  => (@this.Length % 4 == 0) && _base64Regex.IsMatch(@this);

        private static readonly Regex _base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.Compiled);

        /// <summary>
        /// Returns true if the entire contents of the string is an ASCII digit (0 1 2 3 4 6 7 8 9)
        /// </summary>
        public static bool IsAsciiDigits(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return false;
            }

            // Compiled regex is slower, LINQ is slower, direct access by index is slower than the foreach(!).
            foreach (var needle in @this)
            {
                if (needle < '0' || needle > '9')
                {
                    return false;
                }
            }

            return true;
        }


        #region String Replace
        /// <summary>
        /// Erstetzt in einem String exact die Fundstellen
        /// So wird 'Add' und 'Additional' unterschiedlich behandelt.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="find">Fundstelle</param>
        /// <param name="replace">Ersetzung</param>
        /// <param name="matchWholeWord">True (Default) wenn eine Fundstelle exakt behandet werden soll</param>
        /// <returns>String mit den Ersetzungen</returns>
        public static string ReplaceExact(this string @this, string find, string replace, bool matchWholeWord = true)
        {
            string searchString = find.StartsWith("@") ? $@"@\b{find.Substring(1)}\b" : $@"\b{find}\b";
            string textToFind = matchWholeWord ? searchString : find;
            return Regex.Replace(@this, textToFind, replace, RegexOptions.None);
        }

        public static string ReplaceAt(this string @this, int index, char newValue)
        {
            var chars = @this.ToCharArray();
            chars[index] = newValue;

            return new string(chars);
        }

        /// <summary>
        /// This Extension replace original text with new string
        /// </summary>
        /// <param name="this">Text</param>
        /// <param name="index">the start location to replace at (0-based)</param>
        /// <param name="length">the number of characters to be removed before inserting</param>
        /// <param name="newValue">the string that is replacing characters</param>
        /// <returns></returns>
        public static string ReplaceAt(this string @this, int index, int length, string newValue)
        {
            return @this.Remove(index, Math.Min(length, @this.Length - index)).Insert(index, newValue);
        }

        public static string Replace(this string @this, string oldValue, string newValue, StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            if (@this == null || oldValue == null || newValue == null)
            {
                return string.Empty;
            }

            int startIndex = 0;
            while (true)
            {
                startIndex = @this.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                {
                    break;
                }

                @this = @this.Substring(0, startIndex) + newValue + @this.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return @this;
        }

        /// <summary>
        /// Die Erweiterung erstetzt eine Liste von Chars mit einem neues Zeichen.
        /// </summary>
        /// <param name="this">String/Test</param>
        /// <param name="separators">Liste von Zeichen</param>
        /// <param name="newValue">Neuer Wert als String</param>
        /// <returns>Ergebnis der neuen Zeichenkette</returns>
        public static string Replace(this string @this, char[] separators, string newValue)
        {
            string[] splitValues = @this.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(newValue, splitValues);
        }

        /// <summary>
        ///     A string extension method that replace first occurence.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the first occurence of old value replace by new value.</returns>
        public static string ReplaceFirst(this string @this, string oldValue, string newValue)
        {
            int startindex = @this.IndexOf(oldValue);

            if (startindex == -1)
            {
                return @this;
            }

            return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
        }

        /// <summary>
        ///     A string extension method that replace first number of occurences.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="number">Number of.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the numbers of occurences of old value replace by new value.</returns>
        public static string ReplaceFirst(this string @this, int number, string oldValue, string newValue)
        {
            List<string> list = @this.Split(oldValue).ToList();
            int old = number + 1;
            IEnumerable<string> listStart = list.Take(old);
            IEnumerable<string> listEnd = list.Skip(old);

            return string.Join(newValue, listStart) + (listEnd.Any() ? oldValue : "") + string.Join(oldValue, listEnd);
        }
        #endregion String Replace

        #region Split Strings
        /// <summary>
        ///     Returns an enumerable collection of the specified type containing the substrings in this instance that are
        ///     delimited by elements of a specified Char array
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separator">
        ///     An array of Unicode characters that delimit the substrings in this instance, an empty array containing no
        ///     delimiters, or null.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the element to return in the collection, this type must implement IConvertible.
        /// </typeparam>
        /// <returns>
        ///     An enumerable collection whose elements contain the substrings in this instance that are delimited by one or more
        ///     characters in separator.
        /// </returns>
        public static IEnumerable<T> SplitTo<T>(this string str, params char[] separator) where T : IConvertible
        {
            return str.Split(separator, StringSplitOptions.None).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        /// <summary>
        ///     Returns an enumerable collection of the specified type containing the substrings in this instance that are
        ///     delimited by elements of a specified Char array
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="options">StringSplitOptions <see cref="StringSplitOptions" /></param>
        /// <param name="separator">
        ///     An array of Unicode characters that delimit the substrings in this instance, an empty array containing no
        ///     delimiters, or null.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the element to return in the collection, this type must implement IConvertible.
        /// </typeparam>
        /// <returns>
        ///     An enumerable collection whose elements contain the substrings in this instance that are delimited by one or more
        ///     characters in separator.
        /// </returns>
        public static IEnumerable<T> SplitTo<T>(this string str, StringSplitOptions options, params char[] separator)
            where T : IConvertible
        {
            return str.Split(separator, options).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }


        public static IEnumerable<string> SplitEx(this string @this, char separator)
        {
            if (@this.Contains(separator.ToString()) == false)
            {
                return null;
            }

            return @this.Split(new char[] { separator });
        }

        public static IEnumerable<string> SplitEx(this string @this, char[] separator)
        {
            if (@this.Contains(separator.ToString()) == false)
            {
                return null;
            }

            return @this.Split(separator);
        }

        public static IList<string> SplitToList(this string @this, char separator, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            if (@this.Contains(separator.ToString()) == false)
            {
                return null;
            }

            return @this.Split(new char[] { separator }, stringSplitOptions);
        }

        public static IList<string> SplitToList(this string @this, char[] separator)
        {
            if (@this.Contains(separator.ToString()) == false)
            {
                return null;
            }

            return @this.Split(separator);
        }

        public static IEnumerable<string> SplitAndKeep(this string @this, char[] delims)
        {
            int start = 0, index;
            while ((index = @this.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                {
                    yield return @this.Substring(start, index - start);
                }

                yield return @this.Substring(index, 1);
                start = index + 1;
            }

            if (start < @this.Length)
            {
                yield return @this.Substring(start);
            }
        }

        public static string SplitByLenght(this string @this, int lenght, string separator = "\\r\\n")
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }
            else
            {
                var regex = new Regex($".{{{lenght}}}");
                string result = regex.Replace(@this, "$&" + separator);

                return result;
            }
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by a specified Unicode character.
        /// </summary>
        /// <param name="element">The string to work with.</param>
        /// <param name="separator">An Unicode character that delimit the substrings in this string.</param>
        /// <param name="options">System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or System.StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by the separator.</returns>
        public static string[] Split(this string @this, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return @this.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited a specified string.
        /// </summary>
        /// <param name="this">The string to work with.</param>
        /// <param name="separator">An array of strings that delimit the substrings in this string.</param>
        /// <param name="options">System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or System.StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by the separator.</returns>
        public static string[] Split(this string @this, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return @this.Split(new[] { separator }, options);
        }
        #endregion Split Strings

        #region Remove Strings
        /// <summary>
        /// Removes any special characters in the input string not provided
        /// in the allowed special character list.
        ///
        /// Sometimes it is required to remove some special characters like
        /// carriage return, or new line which can be considered as invalid
        /// characters, especially while file processing. This method removes any
        /// special characters in the input string which is not included in the
        /// allowed special character list.
        /// </summary>
        /// <param name="input">Input string to process</param>
        /// <param name="allowedCharacters">list of allowed special characters </param>
        /// <returns>
        /// The original string with special charactersremoved.
        /// </returns>
        /// <example>
        ///
        /// Remove carriage return from the input string:
        ///
        ///     var processedString = RemoveSpecialCharacters(
        ///         "Hello! This is string to process. \r\n", @""",-{}.! "
        ///     );
        ///
        /// </example>
        public static string RemoveSpecialCharacters(this string @this, string allowedCharacters)
        {
            char[] buffer = new char[@this.Length];
            int index = 0;

            char[] allowedSpecialCharacters = allowedCharacters.ToCharArray();

            foreach (char c in @this.Where(c => char.IsLetterOrDigit(c) || allowedSpecialCharacters.Any(x => x == c)))
            {
                buffer[index] = c;
                index++;
            }

            return new string(buffer, 0, index);
        }

        public static string RemoveLastChar(this string @this, char separator = ';')
        {
            string result = string.Empty;
            if (@this.Contains(separator) == true)
            {
                result = @this.Substring(0, @this.Length - 1).Trim();
            }
            else
            {
                result = @this;
            }

            return result;
        }

        public static string RemoveSpaces(this string @this)
        {
            return @this.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Removes all space.
        /// </summary>
        /// <param name="pText">The p text.</param>
        /// <returns></returns>
        public static string RemoveAllSpace(this string @this)
        {
            const string PATTERN = @"[ ]+";

            return Regex.Replace(@this.Trim(), PATTERN, string.Empty);
        }

        public static string RemoveAllSpace(this char @this)
        {
            const string PATTERN = @"[ ]+";

            return Regex.Replace(@this.ToString(), PATTERN, string.Empty);
        }

        /// <summary>
        /// Removes all space.
        /// </summary>
        /// <param name="pText">The p text.</param>
        /// <returns></returns>
        public static string RemoveLinebreak(this string @this)
        {
            const string PATTERN = @"\r\n?|\n";

            return Regex.Replace(@this, PATTERN, string.Empty);
        }

        public static string RemoveChar(this string @this, char character)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            return @this.Replace(character.ToString(), string.Empty);
        }

        public static string RemoveWhitespace(this string @this)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            result = new string(@this.ToCharArray()
                .Where(c => Char.IsWhiteSpace(c) == false)
                .ToArray());

            return result;
        }

        public static string RemoveUnicodeChar(this string @this)
        {
            const string pattern = @"\u0000|\u0001|\u0002|\u0003|\u0004|\u0005|\u0006|\u0007";

            if (string.IsNullOrEmpty(@this) == true)
            {
                return string.Empty;
            }

            string result = Regex.Replace(@this, pattern, string.Empty, RegexOptions.Compiled);

            return result;
        }

        #endregion Remove Strings

        /// <summary>
        /// Does paragraph contain word - search is performed in a case-insensitive manner using the Invariant culture
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool CaseInsensitiveContains(this string paragraph, string word)
        {
            return CaseInsensitiveContains(paragraph, word, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Does paragraph contain word - search is performed in a case-insensitive manner using the specified culture
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="word"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool CaseInsensitiveContains(this string paragraph, string word, CultureInfo culture)
        {
            return culture.CompareInfo.IndexOf(paragraph, word, CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        /// Return all possible permutation for string
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> GetPermutations(this string text)
        {
            return GetPermutations(string.Empty, text);
        }

        /// <summary>
        /// Return all possible permutation for string start at specific character
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="start">The start.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> GetPermutations(this string text, string start)
        {
            if (text.Length <= 1)
                yield return start + text;
            else
            {
                for (var i = 0; i < text.Length; i++)
                {
                    text = text[i] + text.Substring(0, i) + text.Substring(i + 1);
                    foreach (var s in GetPermutations(start + text[0], text.Substring(1)))
                    {
                        yield return s;
                    }
                }
            }
        }

        // <summary>
        /// Parses an string into an decimal. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="numberFormat">The number format.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ParseDecimal(this string @this, decimal defaultValue, IFormatProvider numberFormat)
        {
            decimal val;
            if (!decimal.TryParse(@this, NumberStyles.Any, numberFormat, out val))
            {
                return defaultValue;
            }
            return val;
        }

        /// <summary>
        /// Parses the hexadecimal character.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="ArgumentException">Invalid Hex String{c}</exception>
        public static int ParseHexChar(this char @this)
        {
            if ((@this >= '0') && (@this <= '9'))
            {
                return @this - '0';
            }

            if ((@this >= 'A') && (@this <= 'F'))
            {
                return @this - 'A' + 10;
            }

            if ((@this >= 'a') && (@this <= 'f'))
            {
                return @this - 'a' + 10;
            }

            throw new ArgumentException($"Invalid Hex String{@this}");
        }

        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="numberFormat">The number format.</param>
        /// <returns>System.Int32.</returns>
        public static int ParseInt(this string input, int defaultValue, IFormatProvider numberFormat)
        {
            int val;
            if (!int.TryParse(input, NumberStyles.Any, numberFormat, out val))
            {
                return defaultValue;
            }
            return val;
        }

        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public static int ParseInt(this string input, int defaultValue)
        {
            return ParseInt(input, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
        }
    }
}
