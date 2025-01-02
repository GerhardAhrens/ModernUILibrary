/*
 * <copyright file="TestDataGenerator.cs" company="Lifeprojects.de">
 *     Class: TestDataGenerator
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.01.2025 12:03:44</date>
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


namespace ModernBaseLibrary.Core.Cryptography.TestDataGenerator
{
    using System;

    public static class TestDataGenerator
    {
        private static readonly Random Rnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataGenerator"/> class.
        /// </summary>
        static TestDataGenerator()
        {
            Rnd = new Random();
        }
    }
}
