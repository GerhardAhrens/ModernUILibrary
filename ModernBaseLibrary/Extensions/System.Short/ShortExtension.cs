/*
 * <copyright file="ShortExtension.cs" company="Lifeprojects.de">
 *     Class: ShortExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Short Types
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

namespace ModernBaseLibrary.Extension
{
    public static class ShortExtension
    {
        public static bool Between(this short @this, short minValue, short maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        public static bool In(this short @this, params short[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool NotIn(this short @this, params short[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        public static bool InRange(this short @this, short minValue, short maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        public static bool FactorOf(this short @this, short factorNumer)
        {
            return factorNumer % @this == 0;
        }

        public static bool IsMultipleOf(this short @this, short factor)
        {
            return @this % factor == 0;
        }

        public static bool IsEven(this short @this)
        {
            return @this % 2 == 0;
        }

        public static bool IsOdd(this short @this)
        {
            return @this % 2 != 0;
        }

        /// <summary>
        /// Die Extension gibt bei <= 0 false, und bei > 0 true zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Kleiner gleich 0 false und bei größer 0 = true</returns>
        public static bool ToBool(this short @this)
        {
            return @this <= 0 ? false : true;
        }
    }
}
