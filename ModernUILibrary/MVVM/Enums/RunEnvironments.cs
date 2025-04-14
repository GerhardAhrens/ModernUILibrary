/*
 * <copyright file="RunEnvironments.cs" company="Lifeprojects.de">
 *     Class: RunEnvironments
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>09.02.2025 20:20:12</date>
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

namespace ModernUI.MVVM.Enums
{
    using System;
    using System.ComponentModel;

    public enum RunEnvironments : int
    {
        [Description("Keine Festlegung")]
        None = 0,
        [Description("Entwicklung")]
        Development = 1,
        [Description("UI-Test/Unit-Test")]
        Test = 2,
        [Description("Produktion")]
        Production = 3,
    }
}
