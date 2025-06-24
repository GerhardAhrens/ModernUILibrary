/*
 * <copyright file="Prozentrechnung.cs" company="Lifeprojects.de">
 *     Class: Prozentrechnung
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>24.06.2025 20:34:03</date>
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

namespace ModernBaseLibrary.MathFunc
{
    public class Prozentrechnung
    {
        public static decimal Grundwert(decimal prozent, decimal prozentWert)
        {
            decimal result = 0;

            result = (prozentWert / prozent) * 100;

            return result;
        }

        public static decimal Prozent(decimal prozentWert, decimal grundWert)
        {
            decimal result = 0;

            result = (prozentWert / grundWert) * 100;

            return result;
        }

        public static decimal ProzentWert(decimal grundWert, decimal prozent)
        {
            decimal result = 0;

            result = (grundWert * prozent) / 100;

            return result;
        }
    }
}
