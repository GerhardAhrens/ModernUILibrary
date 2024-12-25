//-----------------------------------------------------------------------
// <copyright file="BooleanExtension.cs" company="Lifeprojects.de">
//     Class: BooleanExtension
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.05.2021</date>
//
// <summary>
// Extension Class for bool Types
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Globalization;

    public static class BooleanExtension
    {
        /// <summary>
        /// Konvertiert den Wert dieser Instanz in seine äquivalente String-Darstellung
        /// <br>Sprachabhängig in Deutsch oder englisch (entweder "Ja/Yes" oder "Nein/No").</br>
        /// </summary>
        /// <param name="">bool Value</param>
        /// <returns>string</returns>
        public static string ToYesNoString(this bool @this)
        {
            if (CultureInfo.CurrentCulture.Name == "de-DE")
            {
                return @this ? "Ja" : "Nein";
            }
            else
            {
                return @this ? "Yes" : "No";
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
        public static string ToYesNoString(this bool @this, CultureInfo cultureInfo)
        {
            if (cultureInfo.Name == "de-DE")
            {
                return @this ? "Ja" : "Nein";
            }
            else
            {
                return @this ? "Yes" : "No";
            }
        }

        /// <summary>
        /// Gib einen bool-Wert als Integer 0/1 zurück.
        /// </summary>
        /// <param name="this">bool Value</param>
        /// <returns><b>True</b> = 1, <b>False</b> = 0</returns>
        public static int ToInt(this bool @this)
        {
            return @this ? 1 : 0;
        }
    }
}