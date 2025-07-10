//-----------------------------------------------------------------------
// <copyright file="FluentBool.cs" company="Lifeprojects.de">
//     Class: FluentBool
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.05.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur bool Behandlung auf Basis einer
// FluentAPI (FluentBool) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System.Diagnostics;
    using System.Globalization;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentBool : FluentBool<FluentBool>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentBool"/> class.
        /// </summary>
        public FluentBool(bool value) : base(value)
        {
        }
    }

    public class FluentBool<TAssertions> : ReferenceTypeAssertions<bool, TAssertions> where TAssertions : FluentBool<TAssertions>
    {
        public FluentBool(bool value) : base(value)
        {
            this.BoolValue = value;
        }

        private bool BoolValue { get; set; }

        /// <summary>
        /// Konvertiert den Wert dieser Instanz in seine äquivalente String-Darstellung
        /// <br>Sprachabhängig in Deutsch oder englisch (entweder "Ja/Yes" oder "Nein/No").</br>
        /// </summary>
        /// <param name="">bool Value</param>
        /// <returns>string</returns>
        public string ToYesNoString()
        {
            if (CultureInfo.CurrentCulture.Name == "de-DE")
            {
                return this.BoolValue ? "Ja" : "Nein";
            }
            else
            {
                return this.BoolValue ? "Yes" : "No";
            }
        }

        /// <summary>
        /// Konvertiert den Wert dieser Instanz in seine äquivalente String-Darstellung
        /// <br>Sprachabhängig in Deutsch oder englisch (entweder "Ja/Yes" oder "Nein/No")</br>
        /// <br>unter Berücksichtigung der übergeben Cultur.</br>
        /// </summary>
        /// <param name="this">bool Value</param>
        /// <param name="cultureInfo">Current CultureInfo</param>
        /// <returns></returns>
        public string ToYesNoString(CultureInfo cultureInfo)
        {
            if (cultureInfo.Name == "de-DE")
            {
                return this.BoolValue ? "Ja" : "Nein";
            }
            else
            {
                return this.BoolValue ? "Yes" : "No";
            }
        }

        /// <summary>
        /// Gib einen bool-Wert als Integer 0/1 zurück.
        /// </summary>
        /// <param name="this">bool Value</param>
        /// <returns><b>True</b> = 1, <b>False</b> = 0</returns>
        public int ToInt()
        {
            return this.BoolValue ? 1 : 0;
        }
    }
}
