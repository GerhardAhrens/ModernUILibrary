/*
 * <copyright file="ConsoleMenuOption.cs" company="Lifeprojects.de">
 *     Class: ConsoleMenuOption
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Erstellen von Menu Item zur Klasse ConsoleMenu
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

namespace ModernBaseLibrary.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [DebuggerDisplay("Menuname={Menuname}; Hotkey={Hotkey}; MenutextColor={MenutextColor}")]
    public class ConsoleMenuOption
    {
        public ConsoleMenuOption(string menuName, Action callAction, ConsoleKey hotkey, ConsoleColor menutextColor)
        {
            menuName.IsArgumentNullOrEmpty("Das Argument 'menuName' darf nicht leer oder null sein.");
            callAction.IsArgumentNull("Das Argument 'callAction' darf nicht null sein.");
            hotkey.IsArgumentInEnum("Für das Argument 'hotkey' muss eine Enum-Wert angegeben werden.");
            menutextColor.IsArgumentInEnum("Für das Argument 'menutextColor' muss eine Enum-Wert angegeben werden.");

            this.Menuname = menuName;
            this.CallAction = callAction;
            this.Hotkey = hotkey;
            this.MenutextColor = menutextColor;
        }

        public ConsoleMenuOption(string name, Action callAction, ConsoleKey hotkey) : this(name, callAction, hotkey,ConsoleColor.White)
        {
        }

        public ConsoleMenuOption(string menuName, Action callAction) : this(menuName, callAction, ConsoleKey.NoName, ConsoleColor.White)
        {
            menuName.IsArgumentNullOrEmpty("Das Argument 'menuName' darf nicht leer oder null sein.");
            callAction.IsArgumentNull("Das Argument 'callAction' darf nicht null sein.");

            this.Hotkey = this.GetHotkey(menuName);
        }

        public ConsoleMenuOption(string menuName, Action callAction, ConsoleColor menutextColor) : this(menuName, callAction, ConsoleKey.NoName, menutextColor)
        {
            menuName.IsArgumentNullOrEmpty("Das Argument 'menuName' darf nicht leer oder null sein.");
            callAction.IsArgumentNull("Das Argument 'callAction' darf nicht null sein.");

            this.Hotkey = this.GetHotkey(menuName);
        }

        public string Menuname { get; }

        public Action CallAction { get; }

        public ConsoleKey Hotkey { get; }

        public ConsoleColor MenutextColor { get; }

        private ConsoleKey GetHotkey(string menuName)
        {
            ConsoleKey result = ConsoleKey.NoName;

            try
            {
                if (menuName.Contains("[") == true && menuName.Contains("]") == true)
                {
                    IEnumerable<string> hotkeys = menuName.BetweenToken("[", "]", false);
                    foreach (string testOfHotkey in hotkeys)
                    {
                        if (testOfHotkey.Length == 1)
                        {
                            char value = Convert.ToChar(testOfHotkey);
                            if (Char.IsLetterOrDigit(value) == true)
                            {
                                if (value.IsDigit() == true)
                                {
                                    result = $"D{value}".ToEnum<ConsoleKey>();
                                }
                                else
                                {
                                    result = value.ToEnum<ConsoleKey>();
                                }
                            }
                        }
                    }
                }
                else
                {
                    result = ConsoleKey.NoName;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
