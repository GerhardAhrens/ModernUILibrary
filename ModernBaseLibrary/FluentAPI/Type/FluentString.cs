//-----------------------------------------------------------------------
// <copyright file="StringFluent.cs" company="Lifeprojects.de">
//     Class: StringFluent
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur String Behandlung auf Basis einer
// FluentAPI (StringFluentExtension) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentString : FluentString<FluentString>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentString"/> class.
        /// </summary>
        public FluentString(string value) : base(value)
        {
        }
    }

    public class FluentString<TAssertions> : ReferenceTypeAssertions<string, TAssertions> where TAssertions : FluentString<TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentString{TAssertions}"/> class.
        /// </summary>
        public FluentString(string value) : base(value)
        {
            this.StringValue = value;
        }

        private string StringValue { get; set; }

        /// <summary>
        /// Gibt die Länge des String zurück
        /// </summary>
        /// <returns>Länge des Strings als Int</returns>
        public int Length()
        {
            return this.StringValue.Length;
        }

        /// <summary>
        /// Gibt vom einem HTML Color Code einen Color Typ zurück
        /// </summary>
        /// <returns>HTML Color Code</returns>
        public Color FromHtmlColor()
        {
            return ColorTranslator.FromHtml(this.StringValue);
        }

        #region Is-Methodes
        /// <summary>
        /// Prüft ob die String-Länge mit dem übergebenen Wert übereinstimmt
        /// </summary>
        /// <param name="expected">Stringlänge gegen die geprüft werden soll</param>
        /// <returns>True = Stringlänge stimmt überein, False = Sttringlänge stimmt nict überein.</returns>
        public bool IsLength(int expected)
        {
            return this.StringValue.Length == expected;
        }

        /// <summary>
        /// Prüft ob der String mit dem übergebenen Muster übereinstimmt.
        /// </summary>
        /// <param name="wildcardPattern">Muster zurm prüfen des Strings</param>
        /// <param name="ignoreCase">True = Groß- und Kleinschreibung wird ignoriert,<br/>False = Groß- und Kleinschreibung wird berücksichtigt</param>
        /// <returns>True = String entspricht dem Muster, False = String entspricht nicht dem Muster</returns>
        public bool IsMatch(string wildcardPattern, bool ignoreCase = true)
        {
            RegexOptions options = ignoreCase == true ? RegexOptions.IgnoreCase : RegexOptions.None;
            string pattern = "^" + Regex.Escape(wildcardPattern).Replace("\\*", ".*", StringComparison.Ordinal).Replace("\\?", ".", StringComparison.Ordinal) + "$";
            return Regex.IsMatch(this.StringValue, pattern, options | RegexOptions.Singleline);
        }

        /// <summary>
        /// Prüft ob der String mit dem übergebenen Muster nicht übereinstimmt.
        /// </summary>
        /// <param name="wildcardPattern">Muster zurm prüfen des Strings</param>
        /// <param name="ignoreCase">True = Groß- und Kleinschreibung wird ignoriert, False = Groß- und Kleinschreibung wird berücksichtigt</param>
        /// <returns>False = String entspricht dem Muster, True = String entspricht nicht dem Muster</returns>
        public bool IsNotMatch(string wildcardPattern, bool ignoreCase = true)
        {
            return this.IsMatch(wildcardPattern, ignoreCase) == false;
        }

        #endregion Is-Methodes

        #region To-Methodes
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

            if (trueStrings.Contains(this.StringValue.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (falseStrings.Contains(this.StringValue.ToString(), StringComparer.OrdinalIgnoreCase))
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

        public Color ToColorFromHexString()
        {
            if (!Regex.IsMatch(this.StringValue, @"[#]([0-9]|[a-f]|[A-F]){6}\b"))
            {
                throw new ArgumentException($"Falsches Format für die Color-Darstelung im Hex-Format = '{this.StringValue}'");
            }

            int red = int.Parse(this.StringValue.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(this.StringValue.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(this.StringValue.Substring(5, 2), NumberStyles.HexNumber);
            return Color.FromArgb(255, red, green, blue);
        }

        public DirectoryInfo ToDirectoryInfo()
        {
            return new DirectoryInfo(this.StringValue);
        }

        #endregion To-Methodes
    }
}
