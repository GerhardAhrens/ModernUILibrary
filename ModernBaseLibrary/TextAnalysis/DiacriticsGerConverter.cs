/*
 * <copyright file="DiacriticsConverter.cs" company="Lifeprojects.de">
 *     Class: DiacriticsConverter
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>16.09.2020</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse DiacriticsConverter konvertiert deutsche Umlaute von z.B. Ä nach AE
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

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class DiacriticsConverter
    {
        private readonly Dictionary<char, string> converter = new Dictionary<char, string>() {
        {  'ä', "ae" },
        {  'Ä', "AE" },
        {  'ö', "oe" },
        {  'Ö', "OE" },
        {  'ü', "ue" },
        {  'Ü', "UE" },
        {  'ß', "ss" }};

        private readonly string value = null;
        private readonly StringBuilder stringBuilder = null;

        public DiacriticsConverter(string value)
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                this.value = value;
            }

            stringBuilder = new StringBuilder();
        }

        public string RemoveDiacritics()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string normalizedString = this.value.Normalize();

            foreach (KeyValuePair<char, string> item in this.converter)
            {
                string temp = normalizedString;
                normalizedString = temp.Replace(item.Key.ToString(), item.Value);
            }

            stringBuilder.Clear();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                normalizedString = normalizedString.Normalize(NormalizationForm.FormD);
                string c = normalizedString[i].ToString();
                if (CharUnicodeInfo.GetUnicodeCategory(Convert.ToChar(c)) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        public bool HasDiacriticsChar(bool germanOnly = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            foreach (KeyValuePair<char, string> item in this.converter)
            {
                if (this.value.Contains(item.Key.ToString()))
                {
                    return true;
                }
            }

            if (germanOnly == false)
            {
                if (value != RemoveDiacritics())
                {
                    return true;
                }
            }

            return false;
        }
    }
}