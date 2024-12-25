/*
 * <copyright file="LongExtension.cs" company="Lifeprojects.de">
 *     Class: LongExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Long Types
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
    using System.Collections.Generic;

    public static class LongExtension
    {
        public static bool Between(this long @this, long minValue, long maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        public static bool In(this long @this, params long[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool NotIn(this long @this, params long[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        public static bool InRange(this long @this, long minValue, long maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        public static bool FactorOf(this long @this, long factorNumer)
        {
            return factorNumer % @this == 0;
        }

        public static bool IsMultipleOf(this long @this, long factor)
        {
            return @this % factor == 0;
        }

        public static bool IsEven(this long @this)
        {
            return @this % 2 == 0;
        }

        public static bool IsOdd(this long @this)
        {
            return @this % 2 != 0;
        }

        public static bool IsPrime(this long @this)
        {
            if (@this == 1 || @this == 2)
            {
                return true;
            }

            if (@this % 2 == 0)
            {
                return false;
            }

            var sqrt = (int)Math.Sqrt(@this);
            for (Int64 t = 3; t <= sqrt; t = t + 2)
            {
                if (@this % t == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a Fibonacci sequence
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<long> Fibonacci(this long @this)
        {
            int a = 0, b = 1;
            for (int i = 0; i < @this; i++)
            {
                yield return a;
                int c = a + b;
                a = b;
                b = c;
            }
        }

        /// <summary>
        /// Die Extension gibt bei <= 0 false, und bei > 0 true zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Kleiner gleich 0 false und bei größer 0 = true</returns>
        public static bool ToBool(this long @this)
        {
            return @this <= 0 ? false : true;
        }

        public static string ToByteText(this long value)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (value == 0)
            {
                return $"0 {suf[0]}";
            }

            long bytes = Math.Abs(value);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(value) * num).ToString() + suf[place];
        }

        /// <summary>
        /// Gibt die formatierte Zeit aus Ticks zurück
        /// </summary>
        /// <param name="this"></param>
        /// <returns>String, Formatierte Zeit (hh:mm:ss)</returns>
        public static string ToMillisecondsFormat(this long @this)
        {
            TimeSpan duration = new TimeSpan(@this);
            return $"{duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
        }
    }
}
