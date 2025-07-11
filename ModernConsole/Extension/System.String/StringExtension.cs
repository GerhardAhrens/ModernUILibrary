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

namespace ModernConsole.Extension
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    [SupportedOSPlatform("windows")]
    public static partial class StringExtension
    {
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
    }
}
