/*
 * <copyright file="StringExtension.cs" company="Lifeprojects.de">
 *     Class: StringExtension
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>26.05.2024 18:25:49</date>
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

namespace System
{
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static partial class StringExtension
    {
        /// <summary>
        /// Wiederholt ein Zeichen 'n' mal
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
    }
}
