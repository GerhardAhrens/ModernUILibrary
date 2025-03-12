/*
 * <copyright file="IntExtension.cs" company="Lifeprojects.de">
 *     Class: IntExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Int Types
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
    using System.Net;

    public static class IntExtension
    {
        /// <summary>
        /// Prüft ob eine Zahl zwischen min und max liegt
        /// </summary>
        /// <param name="this">Zu prüfende Zahl</param>
        /// <param name="minValue">Kleinster Wert</param>
        /// <param name="maxValue">Größter Wert</param>
        /// <returns>True wenn die zu prüfende Zahl zwischen min und max liegt</returns>
        public static bool Between(this int @this, int minValue, int maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        /// <summary>
        /// Prüft ob eine Zahl in einer Liste von Zahlen vorhanden ist.
        /// </summary>
        /// <param name="this">Zu prüfende Zahl</param>
        /// <param name="values">Liste von Zahlen</param>
        /// <returns>Gibt True zurück wenn die Zahl in der Liste von Zahlen vorhanden ist.</returns>
        public static bool In(this int @this, params int[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        /// <summary>
        /// Prüft ob eine Zahl nicht in einer Liste von Zahlen vorhanden ist.
        /// </summary>
        /// <param name="this">Zu prüfende Zahl</param>
        /// <param name="values">Liste von Zahlen</param>
        /// <returns>Gibt True zurück wenn die Zahl in der Liste von Zahlen nicht vorhanden ist.</returns>
        public static bool NotIn(this int @this, params int[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        /// <summary>
        /// Prüft ob eine Zahl zwischen min und max liegt
        /// </summary>
        /// <param name="this">Zu prüfende Zahl</param>
        /// <param name="minValue">Kleinster Wert</param>
        /// <param name="maxValue">Größter Wert</param>
        /// <returns>True wenn die zu prüfende Zahl zwischen min und max liegt</returns>
        public static bool InRange(this int @this, int minValue, int maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="factorNumer"></param>
        /// <returns></returns>
        public static bool FactorOf(this int @this, int factorNumer)
        {
            return factorNumer % @this == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static bool IsMultipleOf(this int @this, int factor)
        {
            return @this % factor == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsEven(this int @this)
        {
            return @this % 2 == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsOdd(this int @this)
        {
            return @this % 2 != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsPrime(this int @this)
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
        public static IEnumerable<int> Fibonacci(this int @this)
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static int HostToNetworkOrder(this int host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static int NetworkToHostOrder(this int network)
        {
            return IPAddress.NetworkToHostOrder(network);
        }

        /// <summary>
        /// Gibt aus einer Zahl <= 0 false, und bei > 0 true zurück
        /// </summary>
        /// <param name="this">Zahl</param>
        /// <returns>Kleiner gleich 0 false und bei größer 0 = true</returns>
        public static bool ToBool(this int @this)
        {
            return @this <= 0 ? false : true;
        }

        /// <summary>
        /// Überprüfung, ob ein Index im Bereich des angegebenen Arrays liegt.
        /// </summary>
        /// <param name="@this">Index</param>
        /// <param name="arrayToCheck">Array das geprüft werden soll</param>
        /// <returns>True wenn das Element mit dem Index gefunden wurde</returns>
        public static bool IsIndexInArray(this int @this, Array arrayToCheck)
        {
            return @this.GetArrayIndex().InRange(arrayToCheck.GetLowerBound(0), arrayToCheck.GetUpperBound(0));
        }

        /// <summary>
        /// Um Array-Index von einem gegebenen basierend auf einer Zahl zu erhalten.
        /// </summary>
        /// <param name="@this">Position im Array</param>
        /// <returns>Array Element der übergebenen Position</returns>
        public static int GetArrayIndex(this int @this)
        {
            return @this == 0 ? 0 : @this - 1;
        }

        /// <summary>
        /// Gibt die Anzahl von Byte las lesbarer Text zurück
        /// </summary>
        /// <param name="this">Eine Zahl die als Dateigröße</param>
        /// <returns>Eine Zahl die als Dateigröße mit Einheit zurückgibt</returns>
        public static string ToByteText(this int @this)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (@this == 0)
            {
                return $"0 {suf[0]}";
            }

            long bytes = Math.Abs(@this);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(@this) * num).ToString() + suf[place];
        }

        /// <summary>
        /// Rounds the value up to the nearest multiple of toNearest. For example:
        /// - 150 to the nearest 10 would return 150
        /// - 151 to the nearest 10 would return 160
        /// </summary>
        /// <param name="???"></param>
        /// <param name="toNearestMultipleOf"></param>
        /// <returns></returns>
        public static int RoundUp(this int @this, int toNearestMultipleOf)
            => @this % toNearestMultipleOf == 0
                ? @this
                : (toNearestMultipleOf - @this % toNearestMultipleOf) + @this;

        /// <summary>
        /// Rounds the value down to the nearest multiple of toNearest. For example:
        /// - 150 to the nearest 10 would return 150
        /// - 151 to the nearest 10 would return 150
        /// </summary>
        /// <param name="???"></param>
        /// <param name="toNearestMultipleOf"></param>
        /// <returns></returns>
        public static int RoundDown(this int @this, int toNearestMultipleOf)  => @this - @this % toNearestMultipleOf;

        public static TimeSpan Millisecond(this int @this)
        {
            if (@this != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return TimeSpan.FromMilliseconds(1);
        }

        public static TimeSpan Milliseconds(this int @this)
        {
            return TimeSpan.FromMilliseconds(@this);
        }

        public static TimeSpan Second(this int @this)
        {
            if (@this != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return TimeSpan.FromSeconds(1);
        }

        public static TimeSpan Seconds(this int @this)
        {
            return TimeSpan.FromSeconds(@this);
        }

        public static TimeSpan Minute(this int @this)
        {
            if (@this != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return TimeSpan.FromMinutes(1);
        }

        public static TimeSpan Minutes(this int @this)
        {
            return TimeSpan.FromMinutes(@this);
        }

        public static TimeSpan Hour(this int @this)
        {
            if (@this != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return TimeSpan.FromHours(1);
        }

        public static TimeSpan Hours(this int @this)
        {
            return TimeSpan.FromHours(@this);
        }

        public static TimeSpan Day(this int @this)
        {
            if (@this != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return TimeSpan.FromDays(@this);
        }

        public static TimeSpan Days(this int @this)
        {
            return TimeSpan.FromDays(@this);
        }

        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Leap_year for avg days in yr.
        /// </remarks>
        public static TimeSpan Years(this int @this)
        {
            return TimeSpan.FromDays(@this * 365.2425d);
        }

        public static bool IsBetween(this int @this, int lowerBound, int upperBound)
        {
            return (@this > lowerBound) && (@this < upperBound);
        }

        public static bool IsBetweenInclusive(this int @this, int lowerBound, int upperBound)
        {
            return (@this >= lowerBound) && (@this <= upperBound);
        }

        public static bool IsGreaterThan(this int @this, int bound)
        {
            return @this > bound;
        }

        public static bool IsPositive(this int @this)
        {
            return @this >= 0;
        }

        public static bool IsLessThan(this int @this, int bound)
        {
            return @this < bound;
        }
    }
}
