/*
 * <copyright file="ConsoleMenu.cs" company="Lifeprojects.de">
 *     Class: ConsoleMenu
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Erstellen eines Menüs in einem Konsolenprogramm
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
    using System.Linq;
    using System.Runtime.Versioning;
    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public sealed class ConsoleMenu : DisposableCoreBase
    {
        public void Run(List<ConsoleMenuOption> options, ConsoleMenuOption selectedOption, ConsoleColor menuPointColor = ConsoleColor.Green)
        {
            int index = 0;
            Console.Clear();
            Console.CursorVisible = false;

            MenuPointColor = menuPointColor;

            WriteMenu(options, options[index]);
            ConsoleKeyInfo keyinfo;

            do
            {
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                else if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                else if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].CallAction.Invoke();
                    index = 0;
                }
                else
                {
                    if (options.Any(f => f.Hotkey == keyinfo.Key) == true)
                    {
                        if (options.Any(f => f.Hotkey == keyinfo.Key) == true)
                        {
                            ConsoleMenuOption menuPoint = options.First(f => f.Hotkey == keyinfo.Key);
                            menuPoint.CallAction.Invoke();
                            index = 0;
                        }
                    }
                    else
                    {
                        index = 0;
                        WriteMenu(options, options[index]);
                    }
                }
            }
            while (keyinfo.Key != ConsoleKey.X);
        }

        public static void WriteMenu(List<ConsoleMenuOption> options, ConsoleMenuOption selectedOption)
        {
            Console.Clear();

            foreach (ConsoleMenuOption option in options.AsParallel())
            {
                if (option == selectedOption)
                {
                    ConsoleHelper.Write("> ", MenuPointColor);
                }
                else
                {
                    ConsoleHelper.Write("  ",ConsoleColor.White);
                }

                ConsoleHelper.WriteLine(option.Menuname, option.MenutextColor);
            }
        }


        private static ConsoleColor MenuPointColor { get; set; } = ConsoleColor.Green;

        protected override void DisposeManagedResources()
        {
        }

        protected override void DisposeUnmanagedResources()
        {
        }
    }
}
