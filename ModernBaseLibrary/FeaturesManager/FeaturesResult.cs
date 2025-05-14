/*
 * <copyright file="FeaturesResult.cs" company="Lifeprojects.de">
 *     Class: FeaturesResult
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>16.02.2023 20:54:06</date>
 * <Project>CurrentProject</Project>
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

namespace ModernBaseLibrary.Features
{
    using System;
    using System.Windows;

    public class FeaturesResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesResult"/> class.
        /// </summary>
        public FeaturesResult(Guid key, string description)
        {
            this.Key = key;
            this.Description = description;
        }

        public Guid Key { get; private set; }

        public string Description { get; private set; }
    }
}
