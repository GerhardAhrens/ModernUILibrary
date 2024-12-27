//-----------------------------------------------------------------------
// <copyright file="CustomLinqExtensions.cs" company="Lifeprojects.de">
//     Class: CustomLinqExtensions
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Concept functions for Custom Linq Extensions
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;

    public static class CustomLinqExtensions
    {
        public static IEnumerable<Nummer> GetNumbers(this NumbersCollection @this, Predicate<Nummer> isMatch)
        {
            foreach (Nummer number in @this)
            {
                if (isMatch(number))
                {
                    yield return number;
                }
            }
        }

        public static Nummer GetNumber(this NumbersCollection @this, Predicate<Nummer> isMatch)
        {
            foreach (Nummer number in @this)
            {
                if (isMatch(number))
                {
                    return number;
                }
            }

            return null;
        }

        public static void MyForEach<Nummer>(this IEnumerable<Nummer> @this, Action<Nummer> action)
        {
            foreach (var e in @this)
            {
                action(e);
            }
        }

        public static IEnumerable<T> IsOverNull<T>(this IEnumerable<T> @this) where T : INummer

        {
            return @this.Where(p => p.IntValue > 0);
        }

        public static IEnumerable<T> MyFilter<T>(this IEnumerable<T> @this, Func<T, bool> filterCondition)
        {
            return @this.Where(filterCondition);
        }

        public static int MyCount<T>(this IEnumerable<T> @this, Func<T, bool> filterCondition)
        {
            return @this.Count(filterCondition);
        }

        public static int MyCount<T>(this IEnumerable<T> @this)
        {
            return @this.Count();
        }

        public static T MaxObject<T, TCompare>(this IEnumerable<T> @this, Func<T, TCompare> func) where TCompare : IComparable<TCompare>
        {
            T maxItem = default(T);
            TCompare maxValue = default(TCompare);
            foreach (var item in @this)
            {
                TCompare temp = func(item);
                if (maxItem == null || temp.CompareTo(maxValue) > 0)
                {
                    maxValue = temp;
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static T MinObject<T, TCompare>(this IEnumerable<T> @this, Func<T, TCompare> func) where TCompare : IComparable<TCompare>
        {
            T maxItem = default(T);
            TCompare maxValue = default(TCompare);
            foreach (var item in @this)
            {
                TCompare temp = func(item);
                if (maxItem == null || temp.CompareTo(maxValue) < 0)
                {
                    maxValue = temp;
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}