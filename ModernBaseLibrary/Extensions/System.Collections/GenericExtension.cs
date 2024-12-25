//-----------------------------------------------------------------------
// <copyright file="GenericExtension.cs" company="Lifeprojects.de">
//     Class: GenericExtension
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.1.2019</date>
//
// <summary>Extension Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GenericExtension
    {
        public static bool In<T>(this T @this, params T[] compareList)
        {
            return compareList.Contains(@this);
        }

        public static bool NotIn<T>(this T @this, params T[] compareList)
        {
            return !compareList.Contains(@this);
        }

        public static bool In<T>(this T @this, IEnumerable<T> compareList)
        {
            return compareList.Contains(@this);
        }

        public static bool NotIn<T>(this T @this, IEnumerable<T> compareList)
        {
            return !compareList.Contains(@this);
        }


        /// <summary>
        /// Die Methode 'IsBetween' prüft, ob ein Wert zwischen einem größten und kleinsten Wert liegt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">Zu prüfender Wert</param>
        /// <param name="lowest">Kleinster Wert</param>
        /// <param name="highest">Höchster wert</param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T @this, T lowest, T highest) where T : IComparable
        {
            return (Comparer<T>.Default.Compare(lowest, @this) <= 0 && Comparer<T>.Default.Compare(highest, @this) >= 0);
        }

        /// <summary>
        /// Returns true if a given value is contained in a list of values
        /// </summary>
        public static bool IsIn<T>(this T value, params T[] values) where T : IComparable
        {
            if (values == null || values.Length == 0)
            {
                return false;
            }

            return new List<T>(values).Contains(value);
        }

        public static RangeCheck IsInRange<T>(this T @this, T pBorder1, T pBorder2) where T : IComparable
        {
            return (RangeCheck)(Math.Sign(pBorder1.CompareTo(pBorder2)) == 0 ?
                Math.Abs(Math.Sign(@this.CompareTo(pBorder1))) + 1 :
                Math.Abs(Math.Sign(@this.CompareTo(pBorder1)) + Math.Sign(@this.CompareTo(pBorder2))));
        }
    }
}
