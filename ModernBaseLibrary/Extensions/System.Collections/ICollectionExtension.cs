/*
 * <copyright file="ICollectionExtensions.cs" company="Lifeprojects.de">
 *     Class: ICollectionExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for ICollection
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
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class ICollectionExtensions
    {
        /// <summary>
        /// An ICollection&lt;T&gt; extension method that adds only if the value satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIf<T>(this ICollection<T> @this, Func<T, bool> predicate, T value)
        {
            if (predicate(value) == true)
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that add value if the ICollection doesn't contains it already.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> @this, T value)
        {
            if (!@this.Contains(value))
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        public static bool AddIfNotExists<T>(this ICollection<T> @this, [NotNull] T item)
        {
            @this = @this.IsArgumentNotReadOnly();

            if (@this.Contains(item))
            {
                return false;
            }

            @this.Add(item);
            return true;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that adds a range to 'values'.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRange<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                @this.Add(value);
            }
        }

        /// <summary>Adds the elements of the specified sequence to the end of the collection.</summary>
        public static void AddRange<TValue>(this ICollection<TValue> collection, IEnumerable<TValue> source)
        {
            foreach (TValue item in source)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that adds a collection of objects to the end of this collection only
        /// for value who satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIf<T>(this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
        {
            foreach (T value in values)
            {
                if (predicate(value) == true)
                {
                    @this.Add(value);
                }
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that adds a range of values that's not already in the ICollection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIfNotContains<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                if (!@this.Contains(value))
                {
                    @this.Add(value);
                }
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that query if '@this' contains all values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAll<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                if (!@this.Contains(value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that query if '@this' contains any value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAny<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                if (@this.Contains(value) == true)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that query if the collection is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty&lt; t&gt;, false if not.</returns>
        [Obsolete("Die Methode 'IsEmpty' ist veraltet. Zukünftig die Methode 'IsNullOrEmpty' verwenden.", false)]
        public static bool IsEmpty<T>(this ICollection<T> @this)
        {
            return @this != null && @this.Count == 0;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that query if the collection is not empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if not empty&lt; t&gt;, false if not.</returns>
        [Obsolete("Die Methode 'IsNotEmpty' ist veraltet. Zukünftig die Methode 'IsNotNullOrEmpty' verwenden.", false)]
        public static bool IsNotEmpty<T>(this ICollection<T> @this)
        {
            return @this != null && @this.Count != 0;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that queries if the collection is not (null or is empty).
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if the collection is not (null or empty), false if not.</returns>
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> @this)
        {
            return @this != null && @this.Count != 0;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that queries if the collection is null or is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or empty&lt; t&gt;, false if not.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> @this)
        {
            return @this == null || @this.Count == 0;
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes if.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveIf<T>(this ICollection<T> @this, T value, Func<T, bool> predicate)
        {
            if (predicate(value) == true)
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes if contains.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        public static void RemoveIfContains<T>(this ICollection<T> @this, T value)
        {
            if (@this.Contains(value) == true)
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes the range.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRange<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes range item that satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRangeIf<T>(this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
        {
            foreach (T value in values)
            {
                if (predicate(value) == true)
                {
                    @this.Remove(value);
                }
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes the range if contains.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRangeIfContains<T>(this ICollection<T> @this, params T[] values)
        {
            foreach (T value in values)
            {
                if (@this.Contains(value) == true)
                {
                    @this.Remove(value);
                }
            }
        }

        /// <summary>
        /// An ICollection&lt;T&gt; extension method that removes value that satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveWhere<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            List<T> list = @this.Where(predicate).ToList();
            foreach (T item in list)
            {
                @this.Remove(item);
            }
        }

        /// <summary>
        /// Replaces the elements of the collection with the the elements of the specified sequence.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection"></param>
        /// <param name="source"></param>
        public static void ReplaceAll<TValue>(this ICollection<TValue> collection, IEnumerable<TValue> source)
        {
            collection.Clear();
            foreach (TValue item in source)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Returns a read-only wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<T> AsReadonly<T>(this ICollection<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }

        private class ReadOnlyCollection<T> : IReadOnlyCollection<T>
        {

            public int Count => collection.Count;

            private readonly ICollection<T> collection;

            public ReadOnlyCollection(ICollection<T> collection)
            {
                this.collection = collection;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return collection.GetEnumerator();
            }
        }
    }
}
