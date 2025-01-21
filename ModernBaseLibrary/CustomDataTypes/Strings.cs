//-----------------------------------------------------------------------
// <copyright file="Strings.cs" company="Lifeprojects.de">
//     Class: Strings
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.02.2023</date>
//
// <summary>
// Class for Custom Value Type Strings
// </summary>
//-----------------------------------------------------------------------


namespace System
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Cryptography;
    using ModernBaseLibrary.Extension;

    public struct Strings : IEquatable<Strings>, IComparable<Strings>
    {
        private readonly string _value;

        public Strings(string value)
        {
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public static string Default
        {
            get { return string.Empty; }
        }

        public bool IsNullOrEmpty
        {
            get { return string.IsNullOrEmpty(this.Value); }
        }

        public int Length
        {
            get {
                if (string.IsNullOrEmpty(this.Value) == true)
                {
                    return 0;
                }
                else
                {
                    return this.Value.Length;
                }
            }
        }

        public bool IsNull
        {
            get
            {
                if (this.Value == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static implicit operator Strings(string value)
        {
            return new Strings(value);
        }

        public static implicit operator string(Strings value)
        {
            char[] charArr = value.ToString().ToCharArray();
            return new string(charArr);
        }

        public static bool operator ==(Strings left, Strings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Strings left, Strings right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return 207501131 ^ this.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if ((obj is Strings) == false)
            {
                return false;
            }

            Strings other = (Strings)obj;
            return Equals(other);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #region Extenstion Methodes 

        #region To-Methodes
        /// <summary>
        /// Die Methode erstellt aus einem String eine List<string> unter Angabe 
        /// eines Separator '\n' als Default.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="separator">Separator '\n' als Default</param>
        /// <returns>Liste mit Tokens</returns>
        public List<string> ToTokenList(char separator = '\n')
        {
            List<string> result = null;

            if (string.IsNullOrEmpty(this.Value) == false)
            {
                string[] lines = this.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result = new List<string>(lines);
            }

            return result;
        }

        /// <summary>
        /// Die Methode erstellt aus einem String eine List<string> unter Angabe einer Liste von char's als Separator
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <returns>Liste mit Tokens</returns>
        public List<string> ToTokenList(params char[] separator)
        {
            List<string> result = null;

            if (string.IsNullOrEmpty(this.Value) == false)
            {
                string[] lines = this.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result = new List<string>(lines);
            }

            return result;
        }

        /// <summary>
        /// Es wird geprüft ob der übergebene String einem Bool-Wert entspricht<br/>
        /// Gültige Werte für True: 1,y,yes,true,ja, j, wahr<br/>
        /// Gültige Werte für False: 0,n,no,false,nein,falsch<br/>
        /// Groß- und Kleinschrebung wird ignoriert<br/>
        /// </summary>
        /// <param name="ignorException">True = es wird keine Exception bei einem falschen Wert ausgelöst,<br/>False = Es wird eine InvalidCastException alsgelöst bei einem Fehler</param>
        /// <returns>Wenn der Wert einem entsprechendem Bool-Wert entspricht, wird True oder False zurückgegeben..</returns>
        public bool ToBool(bool ignorException = false)
        {
            string[] trueStrings = { "1", "y", "yes", "true", "ja", "j", "wahr" };
            string[] falseStrings = { "0", "n", "no", "false", "nein", "falsch" };

            if (trueStrings.Contains(this.Value.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (falseStrings.Contains(this.Value.ToString(), StringComparer.OrdinalIgnoreCase))
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
        /// Konvertiert einen Hex-farbstring vom Format '#FFFFFF' in das 'Color'-Format
        /// </summary>
        /// <returns>'Color'-Format vom Hex-String</returns>
        /// <exception cref="ArgumentException">Falsches Format für die Color-Darstelung im Hex-Format = '{this.Value}'</exception>
        public Color ToColorFromHexString()
        {
            if (!Regex.IsMatch(this.Value, @"[#]([0-9]|[a-f]|[A-F]){6}\b"))
            {
                throw new ArgumentException($"Falsches Format für die Color-Darstelung im Hex-Format = '{this.Value}'");
            }

            int red = int.Parse(this.Value.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(this.Value.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(this.Value.Substring(5, 2), NumberStyles.HexNumber);
            return Color.FromArgb(255, red, green, blue);
        }

        /// <summary>
        /// Gibt einen String in Großschreibung zurück
        /// </summary>
        /// <returns>String</returns>
        public Strings ToLower()
        {
            return this.Value.ToLower();
        }

        /// <summary>
        /// Gibt einen String in Kleinschreibung zurück
        /// </summary>
        /// <returns>String</returns>
        public Strings ToUpper()
        {
            return this.Value.ToUpper();
        }
        #endregion To-Methodes

        /// <summary>
        /// Gibt einen String Reverses zurück
        /// </summary>
        /// <returns>String</returns>
        public string Reverse()
        {
            if (string.IsNullOrEmpty(this.Value) == true)
            {
                return string.Empty;
            }

            return new string(this.Value.Select((c, index) => new { c, index })
                                         .OrderByDescending(x => x.index)
                                         .Select(x => x.c)
                                         .ToArray()).ToString();
        }

        /// <summary>
        /// Erfernt alle NewLine, Return, CrLf aus einem string
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>String</returns>
        public string RemoveNewLines()
        {
            return this.Value.Replace("\n", string.Empty, StringComparison.Ordinal)
                        .Replace("\r", string.Empty, StringComparison.Ordinal)
                        .Replace("\r\n", string.Empty, StringComparison.Ordinal)
                        .Replace("\\r\\n", string.Empty, StringComparison.Ordinal);
        }

        /// <summary>
        /// Wandelt einen Html Zeilenumbruch in ein NewLine um.
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>Zeilenumbruch mit NewLine</returns>
        public string BrToNl()
        {
            return this.Value.Replace("<br />", "\r\n").Replace("<br>", "\r\n");
        }

        public bool EqualsAny(params string[] args)
        {
            string @this = this.Value;
            return args.Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, @this));
        }

        public string Trim()
        {
            return this.Value.Trim();
        }

        public string Trim(params char[] args)
        {
            return this.Value.Trim(args);
        }

        public string Replace(Dictionary<string, string> args)
        {
            if (args == null || args.Count == 0 )
            {
                throw new NullReferenceException("Das übergebene Dictionary<string, string> darf nicht leer sein.");
            }

            return args.Aggregate(this.Value, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
        }

        #region Hash und Crypt
        public string ToCRC32()
        {
            if (string.IsNullOrEmpty(this.Value) == true)
            {
                return string.Empty;
            }

            byte[] byteString = Encoding.ASCII.GetBytes(this.Value);
            uint actual = Crc32.Compute(byteString);

            return actual.ToString();
        }

        public string ToCRC64()
        {
            if (string.IsNullOrEmpty(this.Value) == true)
            {
                return string.Empty;
            }

            byte[] byteString = Encoding.ASCII.GetBytes(this.Value);
            ulong actual = Crc64Iso.Compute(byteString);

            return actual.ToString();
        }

        /// <summary>
        /// Die Methode konvertiert einen String in einem MD5 Hash
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>MD5 Hash String</returns>
        public string ToMD5(bool isUpperOrLower = false)
        {
            string resultHash = string.Empty; 
            if (string.IsNullOrEmpty(this.Value) == true)
            {
                return string.Empty;
            }

            using (StringHash hash = new StringHash(this.Value))
            {
                resultHash = hash.ComputeHash(HashTyp.MD5);
            }

            return isUpperOrLower == true ? resultHash.ToUpper() : resultHash.ToLower();
        }

        /// <summary>
        /// Verifies the m d5 hash.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>Gibt true zurück, wenn der String und Hash übereinstimmt</returns>
        public bool VerifyMD5Hash(string hash)
        {
            string hashOfInput = this.Value.ToMD5();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
        #endregion Hash und Crypt

        #endregion Extenstion Methodes 

        #region Implementation of IEquatable<TextValue>

        public bool Equals(Strings other)
        {
            return this.Value == other.Value;
        }

        #endregion Implementation of IEquatable<TextValue>

        #region Implementation of IComparable<TextValue>

        public int CompareTo(Strings other)
        {
            int valueCompare = this.Value.CompareTo(other.Value);

            return valueCompare;
        }

        #endregion Implementation of IComparable<Date>

    }
}
