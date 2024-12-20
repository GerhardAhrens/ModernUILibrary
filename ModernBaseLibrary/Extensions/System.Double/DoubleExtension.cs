/*
 * <copyright file="DoubleExtension.cs" company="Lifeprojects.de">
 *     Class: DoubleExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 *Extensions Class for Decimal Types
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
    public static class DoubleExtension
    {
        public static int ToInt(this double @this)
        {
            return Convert.ToInt32(@this);
        }

        /// <summary>
        /// A Double extension method that converts the @this to a Decimal
        /// </summary>
        /// <param name="this"></param>
        /// <returns>@this as a Decimal.</returns>
        public static decimal ToDecimal(this double @this)
        {
            return Convert.ToDecimal(@this);
        }

        /// <summary>
        /// A Double extension method that converts the @this to a money.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Decimal.</returns>
        public static decimal ToMoney(this double @this)
        {
            double result = Math.Round(@this, 2);
            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// A Double extension method that converts the @this to a money.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="decimals">Count decimals</param>
        /// <returns>@this as a Decimal.</returns>
        public static decimal ToMoney(this double @this, int decimals)
        {
            double result = Math.Round(@this, decimals);
            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// Die Extension gibt bei <= 0 false, und bei > 0 true zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Kleiner gleich 0 false und bei größer 0 = true</returns>
        public static bool ToBool(this double @this)
        {
            return @this <= 0 ? false : true;
        }

        public static bool Between(this double @this, double minValue, double maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        public static bool In(this double @this, params double[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool NotIn(this double @this, params double[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        public static bool InRange(this double @this, double minValue, double maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        public static double TruncatePrecision(this double @this, int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentException($"Number of double places ({precision} is invalid!)");
            }

            var multiplied = @this * (double)(Math.Pow(10, precision));

            double skippedValue;
            if (@this >= 0)
            {
                skippedValue = Math.Floor(multiplied);
            }
            else
            {
                skippedValue = Math.Ceiling(multiplied);
            }

            return skippedValue / (double)(Math.Pow(10, precision));
        }
    }
}
