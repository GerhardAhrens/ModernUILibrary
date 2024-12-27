/*
 * <copyright file="CommandManager.cs" company="Lifeprojects.de">
 *     Class: CommandManager
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Simulation einer CommandLine
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

namespace ModernBaseLibrary.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandManager
    {
        private static string TrimMatchingQuotes(string input, char quote) {
            if ((input.Length >= 2) && (input[0] == quote) && (input[input.Length - 1] == quote))
            {
                return input.Substring(1, input.Length - 2);
            }

            return input;
        }

        private static IEnumerable<string> Split(string input, Func<char, bool> fn) {
            int nextPiece = 0;

            for (int c = 0; c < input.Length; c++)
            {
                if (fn(input[c]))
                {
                    yield return input.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return input.Substring(nextPiece);
        }

        public static string[] CommandLineToArgs(string commandLine)
        {
            bool inQuotes = false;

            return Split(commandLine, (c) =>
                            {
                                if (c == '\"')
                                {
                                    inQuotes = !inQuotes;
                                }

                                return !inQuotes && c == ' ';
                            }).Select(arg => TrimMatchingQuotes(arg.Trim(), '\"'))
                            .Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
        }
    }
}
