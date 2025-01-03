/*
 * <copyright file="DataGridLoc.cs" company="Lifeprojects.de">
 *     Class: DataGridLoc
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>02.01.2025</date>
 * <Project>ModernUILibrary</Project>
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

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public enum DataGridLanguage
    {
        English = 0,
        German,
    }

    public static class DataGridLoc
    {
        private static readonly int Language;

        private static readonly string[] CultureNames = { "en-US", "de-DE" };

        private static readonly Dictionary<string, string[]> Translation = new Dictionary<string, string[]>
        {
            {
                "All", new[]
                {
                    "(Select all)",
                    "(Wählen Sie Alle)",
                }
            },
            {
                "Empty", new[]
                {
                    "(Blank)",
                    "(Leer)",
                }
            },
            {
                "Clear",
                new[]
                {
                    "Clear filter \"{0}\"",
                    "Filter löschen \"{0}\"",
                }
            },
            {
                "Search", new[]
                {
                    "Search (contains)",
                    "Suche (enthält)",
                }
            },
            {
                "Ok", new[]
                {
                    "Ok",
                    "Ok",
                }
            },
            {
                "Cancel", new[]
                {
                    "Cancel",
                    "Abbrechen",
                }
            },
            {
                "Status", new[]
                {
                    "record(s) {0:### ### ###} / Selected {1:### ### ###}",
                    "Anzahl {0:### ### ###} / Markiert {1:### ### ###}",
                }
            },
            {
                "ElapsedTime", new[]
                {
                    "Elapsed time {0:mm}:{0:ss}.{0:ff}",
                    "Verstrichene Zeit {0:mm}:{0:ss}.{0:ff}",
                }
            }
        };

        static DataGridLoc()
        {
            // change language here
            Language = (int)DataGridLanguage.German;
            Culture = new CultureInfo(CultureNames[Language]);
        }

        public static CultureInfo Culture { get; }
        public static string CultureName => CultureNames[Language];
        public static string LanguageName => Enum.GetName(typeof(DataGridLanguage), Language);

        public static string All => Translation["All"][Language];
        public static string Cancel => Translation["Cancel"][Language];
        public static string Ok => Translation["Ok"][Language];
        public static string Clear => Translation["Clear"][Language];
        public static string Empty => Translation["Empty"][Language];
        public static string Search => Translation["Search"][Language];
        public static string Status => Translation["Status"][Language];
        public static string ElapsedTime => Translation["ElapsedTime"][Language];
    }
}