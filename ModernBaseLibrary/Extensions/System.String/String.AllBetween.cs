/*
 * <copyright file="String.AllBetween.cs" company="Lifeprojects.de">
 *     Class: StringExtension
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>15.09.2023</date>
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static partial class StringExtension
    {
        /// <summary>
        /// Extracts all parts from the input string between the first encounter of a couple of enclosure character
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="enclosureCharacter">The character to use for checking enclosure</param>
        /// <returns>
        /// A sequence of substring starting at the first position after the first encountered enclosure character until the last position before the second enclosure character
        /// </returns>
        public static IEnumerable<string> AllBetween(this string input, char enclosureCharacter)
        {
            return AllBetween(input, enclosureCharacter, enclosureCharacter);
        }

        /// <summary>
        /// Extracts all parts from the input string between the first encounter of a the first enclosure character until the first encounter of the second enclosure character
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="firstEnclosureCharacter">The first character to use for checking enclosure</param>
        /// <param name="secondEnclosureCharacter">The second character to use for checking enclosure</param>
        /// <returns>
        /// A sequence of substring starting at the first position after the first encountered enclosure character until the last position before the second enclosure character
        /// </returns>
        public static IEnumerable<string> AllBetween(this string input, char firstEnclosureCharacter, char secondEnclosureCharacter)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");

            return AllBetweenCore(input, firstEnclosureCharacter, secondEnclosureCharacter);
        }

        private static IEnumerable<string> AllBetweenCore(this string input, char firstEnclosureCharacter, char secondEnclosureCharacter)
        {
            int firstEnclosureCharacterIndex = input.IndexOf(firstEnclosureCharacter);
            while (firstEnclosureCharacterIndex != -1 && firstEnclosureCharacterIndex < input.Length - 1)
            {
                int firstAdjustedIndex = firstEnclosureCharacterIndex + 1;
                int secondEnclosureCharacterIndex = input.IndexOf(secondEnclosureCharacter, firstAdjustedIndex);
                if (secondEnclosureCharacterIndex == -1)
                {
                    break;
                }
                else
                {
                    int length = secondEnclosureCharacterIndex - firstAdjustedIndex;

                    string part = input.Substring(firstAdjustedIndex, length);

                    yield return part;

                    firstEnclosureCharacterIndex = input.IndexOf(firstEnclosureCharacter, secondEnclosureCharacterIndex + 1);
                }
            }
        }

        /// <summary>
        /// Extracts all parts from the input string between the first encounter of a couple of enclosures
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="enclosure">The string used for checking enclosure</param>
        /// <returns>
        /// A sequence of substring starting at the first position after the first encountered enclosure character until the last position before the second enclosure character
        /// </returns>
        public static IEnumerable<string> AllBetween(this string input, string enclosure)
        {
            return AllBetween(input, enclosure, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the part from the input string between all encounters of a couple of outer enclosures
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="enclosure">The string to use for checking enclosure</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>
        /// A sequence of substring starting at the first position after the first encountered enclosure character until the last position before the second enclosure character
        /// </returns>
        public static IEnumerable<string> AllBetween(this string input, string enclosure, StringComparison comparisonType)
        {
            return AllBetween(input, enclosure, enclosure, comparisonType);
        }

        /// <summary>
        /// Extracts the part from the input string between all encounters of a couple of outer enclosures
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="firstEnclosure">The first string to use for checking enclosure</param>
        /// <param name="secondEnclosure">The second string to use for checking enclosure</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>
        /// A sequence of substring starting at the first position after the first encountered enclosure character until the last position before the second enclosure character
        /// </returns>
        public static IEnumerable<string> AllBetween(this string input, string firstEnclosure, string secondEnclosure, StringComparison comparisonType)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");
            if (firstEnclosure == null)
                throw new ArgumentNullException("firstEnclosure");
            if (secondEnclosure == null)
                throw new ArgumentNullException("secondEnclosure");

            return AllBetweenImpl(input, firstEnclosure, secondEnclosure, comparisonType);
        }

        private static IEnumerable<string> AllBetweenImpl(this string input, string firstEnclosure, string secondEnclosure, StringComparison comparisonType)
        {
            int firstEnclosureIndex = input.IndexOf(firstEnclosure, comparisonType);
            while (firstEnclosureIndex != -1 && firstEnclosureIndex + firstEnclosure.Length < input.Length)
            {
                int firstAdjustedIndex = firstEnclosureIndex + firstEnclosure.Length;
                int secondEnclosureIndex = input.IndexOf(secondEnclosure, firstAdjustedIndex, comparisonType);
                if (secondEnclosureIndex == -1)
                {
                    break;
                }
                else
                {
                    int length = secondEnclosureIndex - firstAdjustedIndex;

                    string substring = input.Substring(firstAdjustedIndex, length);

                    yield return substring;

                    firstEnclosureIndex = input.IndexOf(firstEnclosure, secondEnclosureIndex + secondEnclosure.Length);
                }
            }
        }
    }
}
