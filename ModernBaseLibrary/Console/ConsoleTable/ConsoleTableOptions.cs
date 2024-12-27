/*
 * <copyright file="ConsoleTable.cs" company="Lifeprojects.de">
 *     Class: ConsoleTable
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>28.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse stellt für ein Consolenprogramm eine Tabellenausgabe zur Verfügung
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
    using System.IO;

    public class ConsoleTableOptions
    {
        public IEnumerable<string> Columns { get; set; } = new List<string>();

        public bool EnableCount { get; set; } = true;

        /// <summary>
        /// Enable only from a list of objects
        /// </summary>
        public ConsoleTableAlignment NumberAlignment { get; set; } = ConsoleTableAlignment.Left;

        /// <summary>
        /// The <see cref="TextWriter"/> to write to. Defaults to <see cref="Console.Out"/>.
        /// </summary>
        public TextWriter OutputTo { get; set; } = Console.Out;
    }
}
