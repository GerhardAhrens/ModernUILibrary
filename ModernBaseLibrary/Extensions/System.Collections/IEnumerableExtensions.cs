/*
 * <copyright file="IEnumerableExtensions.cs" company="Lifeprojects.de">
 *     Class: IEnumerableExtensions
 *     Copyright � Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 12:02:43</date>
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

namespace System.Collections.Generic
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Versioning;
    using System.Text;

    [SupportedOSPlatform("windows")]
    public static class IEnumerableExtensions
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Pr�ft ob ein IEnumerable<typeparamref name="T"/> Null oder leer ist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns>True, wenn das IEnumerable<typeparamref name="T"/> null oder leer ist</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this == null || @this.Any() == false;
        }

        /// <summary>
        /// Pr�ft ob ein IEnumerable<typeparamref name="T"/> nicht Null oder leer ist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns>True, wenn das IEnumerable<typeparamref name="T"/> nicht null oder leer ist</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return @this != null || @this.Any() == true;
        }

        /// <summary>
        /// Pr�ft ob ein IEnumerable Null oder leer ist
        /// </summary>
        /// <param name="this"></param>
        /// <returns>True, wenn das IEnumerable null oder leer ist</returns>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            if (@this != null)
            {
                return @this.GetEnumerator().MoveNext() == false;
            }

            return true;
        }

        public static int IndexOf<T>(this IEnumerable<T> @this, T pValue)
        {
            return @this.IndexOf(pValue, null);
        }

        public static int IndexOf<T>(this IEnumerable<T> @this, T pValue, IEqualityComparer<T> pComparer)
        {
            pComparer = pComparer ?? EqualityComparer<T>.Default;
            var found = @this
                .Select((a, i) => new { a, i })
                .FirstOrDefault(x => pComparer.Equals(x.a, pValue));

            return found == null ? -1 : found.i;
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var e in @this)
            {
                action(e);
            }
        }

        /// <summary>
        /// Counts the number of items in a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>System.Int64.</returns>
        internal static long FastCount<T>([NotNull] this IEnumerable<T> list)
        {
            return list.LongCount();
        }

        /// <summary>
        /// Gibt eine <see cref="List&lt;T>"/> mit eindeutigen Werten
        /// zur�ck.
        /// </summary>
        /// <typeparam name="T">
        /// Der Typ der aufzulistenden Objekte.
        /// </typeparam>
        /// <param name="@this">
        /// Die Auflistung.
        /// </param>
        /// <param name="keepOrder">
        /// Gitb an ob die Reihenfolge der Elemente beibehalten werden soll.
        /// </param>
        /// <param name="comparer">
        /// Die <see cref="IEqualityComparer&lt;T>"/>-Implementierung, die
        /// zum Vergleichen von Schl�sseln verwendet werden soll, oder null,
        /// wenn der Standard-<see cref="EqualityComparer&lt;T>"/> f�r diesen
        /// Schl�sseltyp verwendet werden soll.
        /// </param>
        /// <returns>
        /// Auflistung ohne Duplikate.
        /// </returns>
        public static List<T> GetDistinct<T>(this IEnumerable<T> @this, bool keepOrder, IEqualityComparer<T> comparer)
        {
            if (keepOrder == true)
            {
                HashSet<T> hashSet = new HashSet<T>(comparer);
                List<T> result = new List<T>();

                foreach (T item in @this)
                {
                    if (hashSet.Add(item) == true)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            else
                return new HashSet<T>(@this, comparer).ToList();
        }

        /// <summary>
        /// Gibt jedes Element mit seinem dazugeh�rigen Index zur�ck
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <returns>Liste mit dem Index und dem dazugeh�rigm Item</returns>
        public static IEnumerable<Tuple<int, T>> LoopIndex<T>(this IEnumerable<T> @this)
        {
            int index = -1;

            using (var enumerator = @this.GetEnumerator())
                while (enumerator.MoveNext())
                {
                    index++;
                    var item = enumerator.Current;

                    yield return new Tuple<int, T>(index, item);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="comparer"></param>
        /// <param name="onLeftOnly"></param>
        /// <param name="onRightOnly"></param>
        /// <param name="onBoth"></param>
        public static void CompareSortedCollections<T>(IEnumerable<T> source, IEnumerable<T> destination, IComparer<T> comparer, Action<T> onLeftOnly, Action<T> onRightOnly, Action<T, T> onBoth)
        {
            EnumerableIterator<T> sourceIterator = new EnumerableIterator<T>(source);
            EnumerableIterator<T> destinationIterator = new EnumerableIterator<T>(destination);

            while (sourceIterator.HasCurrent && destinationIterator.HasCurrent)
            {
                // While LHS < RHS, the items in LHS aren't in RHS
                while (sourceIterator.HasCurrent && (comparer.Compare(sourceIterator.Current, destinationIterator.Current) < 0))
                {
                    onLeftOnly(sourceIterator.Current);
                    sourceIterator.MoveNext();
                }

                // While RHS < LHS, the items in RHS aren't in LHS
                while (sourceIterator.HasCurrent && destinationIterator.HasCurrent && (comparer.Compare(sourceIterator.Current, destinationIterator.Current) > 0))
                {
                    onRightOnly(destinationIterator.Current);
                    destinationIterator.MoveNext();
                }

                // While LHS==RHS, the items are in both
                while (sourceIterator.HasCurrent && destinationIterator.HasCurrent && (comparer.Compare(sourceIterator.Current, destinationIterator.Current) == 0))
                {
                    onBoth(sourceIterator.Current, destinationIterator.Current);
                    sourceIterator.MoveNext();
                    destinationIterator.MoveNext();
                }
            }

            // Mop up.
            while (sourceIterator.HasCurrent)
            {
                onLeftOnly(sourceIterator.Current);
                sourceIterator.MoveNext();
            }

            while (destinationIterator.HasCurrent)
            {
                onRightOnly(destinationIterator.Current);
                destinationIterator.MoveNext();
            }
        }

        /// <summary>
        /// Die Methode gibt die Anzahl von Elementen in der Liste zur�ck
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Anzahl als Integer</returns>
        public static int Count(this IEnumerable<string> @this)
        {
            int result = 0;
            using (IEnumerator<string> enumerator = @this.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    result++;
                }
            }

            return result;
        }

        public static string Split(this IEnumerable<string> @this, string sparator = ", ")
        {
            string result = string.Empty;

            result = string.Join(sparator, @this.ToArray());

            return result;
        }

        /// <summary>
        ///     Appends an element to the end of the current collection and returns the new collection.
        /// </summary>
        /// <typeparam name="T">The enumerable data type</typeparam>
        /// <param name="source">The data values.</param>
        /// <param name="item">The element to append the current collection with.</param>
        /// <returns>
        ///     The modified collection.
        /// </returns>
        /// <example>
        ///     var integers = Enumerable.Range(0, 3);  // 0, 1, 2
        ///     integers = integers.Append(3);          // 0, 1, 2, 3
        /// </example>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            foreach (var i in source)
            {
                yield return i;
            }

            yield return item;
        }

        /// <summary>
        /// Prepends an element to the start of the current collection and returns the new collection.
        /// </summary>
        /// <typeparam name="T">The enumerable data type</typeparam>
        /// <param name="source">The data values.</param>
        /// <param name="item">The element to prepend the current collection with.</param>
        /// <returns>
        ///     The modified collection.
        /// </returns>
        /// <example>
        ///     var integers = Enumerable.Range(1, 3);  // 1, 2, 3
        ///     integers = integers.Prepend(0);         // 0, 1, 2, 3
        /// </example>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            yield return item;

            foreach (var i in source)
            {
                yield return i;
            }
        }

        /// <summary>
        /// Converts all items of a list and returns them as enumerable.
        /// </summary>
        /// <typeparam name = "TSource">The source data type</typeparam>
        /// <typeparam name = "TTarget">The target data type</typeparam>
        /// <param name = "source">The source data.</param>
        /// <returns>The converted data</returns>
        /// <example>
        /// var values = new[] { "1", "2", "3" };
        /// values.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
        /// </example>
        public static IEnumerable<TTarget> ConvertList<TSource, TTarget>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Select(value => value.ConvertTo<TTarget>());
        }

        /// <summary>
        /// Returns the minimum item based on a provided selector.
        /// </summary>
        /// <typeparam name = "TItem">The item type</typeparam>
        /// <typeparam name = "TValue">The value item</typeparam>
        /// <param name = "items">The items.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "minValue">The min value as output parameter.</param>
        /// <returns>The minimum item</returns>
        /// <example>
        /// <code>
        /// int age;
        /// var youngestPerson = persons.MinItem(p =&gt; p.Age, out age);
        /// </code>
        /// </example>
        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector, out TValue minValue)
            where TItem : class
            where TValue : IComparable
        {
            TItem minItem = null;
            minValue = default(TValue);

            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                var itemValue = selector(item);

                if ((minItem != null) && (itemValue.CompareTo(minValue) >= 0))
                {
                    continue;
                }

                minValue = itemValue;
                minItem = item;
            }

            return minItem;
        }

        /// <summary>
        /// 	Returns the maximum item based on a provided selector.
        /// </summary>
        /// <typeparam name = "TItem">The item type</typeparam>
        /// <typeparam name = "TValue">The value item</typeparam>
        /// <param name = "items">The items.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "maxValue">The max value as output parameter.</param>
        /// <returns>The maximum item</returns>
        /// <example>
        /// <code>
        /// int age;
        /// var oldestPerson = persons.MaxItem(p =&gt; p.Age, out age);
        /// </code>
        /// </example>
        public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector, out TValue maxValue)
            where TItem : class
            where TValue : IComparable
        {
            TItem maxItem = null;
            maxValue = default(TValue);

            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                var itemValue = selector(item);

                if ((maxItem != null) && (itemValue.CompareTo(maxValue) <= 0))
                {
                    continue;
                }

                maxValue = itemValue;
                maxItem = item;
            }

            return maxItem;
        }

        /// <summary>
        /// 	Returns the maximum item based on a provided selector.
        /// </summary>
        /// <typeparam name = "TItem">The item type</typeparam>
        /// <typeparam name = "TValue">The value item</typeparam>
        /// <param name = "items">The items.</param>
        /// <param name = "selector">The selector.</param>
        /// <returns>The maximum item</returns>
        /// <example>
        /// <code>
        /// var oldestPerson = persons.MaxItem(p =&gt; p.Age);
        /// </code>
        /// </example>
        public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
            where TItem : class
            where TValue : IComparable
        {
            TValue maxValue;

            return items.MaxItem(selector, out maxValue);
        }

        /// <summary>
        /// Returns the minimum item based on a provided selector.
        /// </summary>
        /// <typeparam name = "TItem">The item type</typeparam>
        /// <typeparam name = "TValue">The value item</typeparam>
        /// <param name = "items">The items.</param>
        /// <param name = "selector">The selector.</param>
        /// <returns>The minimum item</returns>
        /// <example>
        /// 	<code>
        /// 		var youngestPerson = persons.MinItem(p =&gt; p.Age);
        /// 	</code>
        /// </example>
        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
            where TItem : class
            where TValue : IComparable
        {
            TValue minValue;

            return items.MinItem(selector, out minValue);
        }

        ///<summary>
        /// Get Distinct
        ///</summary>
        ///<param name = "source"></param>
        ///<param name = "expression"></param>
        ///<typeparam name = "T"></typeparam>
        ///<typeparam name = "TKey"></typeparam>
        ///<returns></returns>
        /// <remarks>
        /// Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> expression)
        {
            return source == null ? Enumerable.Empty<T>() : source.GroupBy(expression).Select(i => i.First());
        }

        /// <summary>
        /// Gibt ein <see cref="IEnumerable&lt;T>"/> mit eindeutigen Werten
        /// zur�ck.
        /// </summary>
        /// <typeparam name="T">
        /// Der Typ der aufzulistenden Objekte.
        /// </typeparam>
        /// <param name="@this">
        /// Die Auflistung.
        /// </param>
        /// <returns>
        /// Auflistung ohne Duplikate.
        /// </returns>
        public static IEnumerable<T> GetDistinctEnumerable<T>(this IEnumerable<T> @this)
        {
            HashSet<T> hashSet = new HashSet<T>();
            foreach (T item in @this)
            {
                if (hashSet.Add(item) == true)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, object> uniqueCheckerMethod)
        {
            return source.Distinct(new GenericComparer<T>(uniqueCheckerMethod));
        }

        public static bool IsSameAs<T, TKey>(this IEnumerable<T> @this, IEnumerable<T> target, Expression<Func<T, TKey>> keySelectorExpression)
        {
            // check the object
            if (@this == null && target == null)
            {
                return true;
            }

            if (@this == null || target == null)
            {
                return false;
            }

            var sourceList = @this.ToList();
            var targetList = target.ToList();

            // check the list count :: { 1,1,1 } != { 1,1,1,1 }
            if (sourceList.Count != targetList.Count)
            {
                return false;
            }

            var keySelector = keySelectorExpression.Compile();
            var groupedSourceList = sourceList.GroupBy(keySelector).ToList();
            var groupedTargetList = targetList.GroupBy(keySelector).ToList();

            // check that the number of grouptings match :: { 1,1,2,3,4 } != { 1,1,2,3,4,5 }
            var groupCountIsSame = groupedSourceList.Count == groupedTargetList.Count;
            if (!groupCountIsSame)
            {
                return false;
            }

            // check that the count of each group in source has the same count in target :: for values { 1,1,2,3,4 } & { 1,1,1,2,3,4 }
            // key:count
            // { 1:2, 2:1, 3:1, 4:1 } != { 1:3, 2:1, 3:1, 4:1 }
            var countsMissmatch = groupedSourceList.Any(sourceGroup =>
            {
                var targetGroup = groupedTargetList.Single(y => y.Key.Equals(sourceGroup.Key));
                return sourceGroup.Count() != targetGroup.Count();
            });

            return !countsMissmatch;
        }

        /// <summary>
        /// Gruppiert die Elemente einer Sequenz gem�� einer angegebenen firstKey-Selektorfunktion und rotiert 
        /// die eindeutigen Werte aus der secondKey-Selektorfunktion in mehrere Werte in der Ausgabe 
        /// und f�hrt Aggregationen durch.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TFirstKey"></typeparam>
        /// <typeparam name="TSecondKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="@this">IEnumerable of T</param>
        /// <param name="firstKeySelector"></param>
        /// <param name="secondKeySelector"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> @this, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = @this.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return retVal;
        }

        public static string ToPrint<TSource>(this IEnumerable<TSource> @this, Func<TSource, int, bool> wherePredicate = null,
            Func<TSource, int, string> formatFunction = null,
            Func<StringBuilder, string, StringBuilder> ConcatenateFunction = null)
        {
            if (formatFunction == null)
            {
                formatFunction = (Source, Position) => $"[{Position}] {Source}";
            }

            if (ConcatenateFunction == null)
            {
                ConcatenateFunction = (Result, Value) => Result.AppendFormat(" {0}", Value);
            }

            StringBuilder retVal;
            if (wherePredicate == null)
            {
                retVal = @this
                    .Select((a, pos) => formatFunction(a, pos))
                    .Aggregate(new StringBuilder(),
                    (ReturnString, Value) => ConcatenateFunction(ReturnString, Value));
            }
            else
            {
                retVal = @this
                    .Where((InputObject, Position) => wherePredicate(InputObject, Position))
                    .Select((InputObject, Position) => formatFunction(InputObject, Position))
                    .Aggregate(new StringBuilder(),
                    (ReturnString, Value) => ConcatenateFunction(ReturnString, Value));
            }

            return retVal.ToString().Trim();
        }

        public static T Find<T>(this IEnumerable<T> @this, Func<T, bool> pPredicate)
        {
            foreach (var current in @this)
            {
                if (pPredicate(current))
                {
                    return current;
                }
            }

            return default(T);
        }

        public static IEnumerable<Item<T>> WithIndex<T>(this IEnumerable<T> @this)
        {
            Item<T> item = null;
            foreach (T value in @this)
            {
                Item<T> next = new Item<T>();
                next.Index = 0;
                next.Value = value;
                next.IsLast = false;
                if (item != null)
                {
                    next.Index = item.Index + 1;
                    yield return item;
                }

                item = next;
            }

            if (item != null)
            {
                item.IsLast = true;
                yield return item;
            }
        }

        public static string JoinWithPrefix(this IEnumerable<string> @this, char separator = ',', char prefix = '@')
        {
            string result = @this.Aggregate((a, x) =>
            {
                string joinColumn = string.Empty;

                if (a.Contains(prefix) == false)
                {
                    joinColumn = $"{prefix}{a}{separator} {prefix}{x}";
                }
                else
                {
                    joinColumn = $"{a}{separator} {prefix}{x}";
                }

                return joinColumn;
            });

            return result;
        }

        public static string JoinWithSufix(this IEnumerable<string> @this, char separator = ',', char sufix = '@')
        {
            string result = @this.Aggregate((a, x) =>
            {
                string joinColumn = string.Empty;

                if (a.Contains(sufix) == false)
                {
                    joinColumn = $"{a}{sufix}{separator} {x}{sufix}";
                }
                else
                {
                    joinColumn = $"{a}{separator} {x}{sufix}";
                }

                return joinColumn;
            });

            return result;
        }

        /// <summary>
        /// Die Methode pr�ft ob in der Liste der �bergebene Suchstring mindestens einmal gefunden wird.
        /// </summary>
        /// <param name="this">�bergebene Liste</param>
        /// <param name="findString">Suchstring</param>
        /// <param name="ignorCase">True = Ingnoriere Gro�- und Kleinschreibung<br>False = Ber�cksichtige Gro�- und Kleinschreibung</br></param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> @this, string findString, bool ignorCase = true)
        {
            bool result = false;
            using (IEnumerator<string> enumerator = @this.GetEnumerator())
            {
                foreach (var element in @this)
                {
                    if (ignorCase == true)
                    {
                        if (findString.ToLower().Contains(element.ToLower(), StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (findString.ToLower().Contains(element.ToLower(), StringComparison.CurrentCulture) == true)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }

        public static T SingleOrNew<T>(this IEnumerable<T> @this, T newValue)
        {
            T element = @this.SingleOrDefault();
            if (element == null)
            {
                return newValue;
            }

            return element;
        }

        public static T SingleOrNew<T>(this IEnumerable<T> @this, Func<T, bool> predicate, T newValue)
        {
            T element = @this.SingleOrDefault(predicate);
            if (element == null)
            {
                return newValue;
            }

            return element;
        }

        public static T ElementOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            if (@this.Any(predicate) == true)
            {
                T element = @this.SingleOrDefault(predicate);
                if (element == null)
                {
                    return default(T);
                }

                return element;
            }

            return default(T);
        }

        public static void ForEachInEnumerable<TEnumerable>(this IEnumerable<TEnumerable> @this, Action<TEnumerable> action)
        {
            foreach (var item in @this)
            {
                action(item);
            }
        }

        /// <summary>
        /// Gibt eine <see cref="List&lt;T>"/> mit eindeutigen Werten
        /// zur�ck.
        /// </summary>
        /// <typeparam name="T">
        /// Der Typ der aufzulistenden Objekte.
        /// </typeparam>
        /// <param name="collection">
        /// Die Auflistung.
        /// </param>
        /// <returns>
        /// Auflistung ohne Duplikate.
        /// </returns>
        public static List<T> GetDistinct<T>(this IEnumerable<T> collection)
        {
            return collection.GetDistinct(false);
        }

        /// <summary>
        /// Gibt eine <see cref="List&lt;T>"/> mit eindeutigen Werten
        /// zur�ck.
        /// </summary>
        /// <typeparam name="T">
        /// Der Typ der aufzulistenden Objekte.
        /// </typeparam>
        /// <param name="collection">
        /// Die Auflistung.
        /// </param>
        /// <param name="keepOrder">
        /// Gitb an ob die Reihenfolge der Elemente beibehalten werden soll.
        /// </param>
        /// <returns>
        /// Auflistung ohne Duplikate.
        /// </returns>
        public static List<T> GetDistinct<T>(this IEnumerable<T> collection, bool keepOrder)
        {
            if (keepOrder == true)
            {
                HashSet<T> hashSet = new HashSet<T>();

                List<T> result = new List<T>();

                foreach (T item in collection)
                {
                    if (hashSet.Add(item) == true)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            else
            {
                return new HashSet<T>(collection).ToList();
            }
        }

        public static IEnumerable<T> OrderBySequence<T, TProperty>(this IEnumerable<T> @this, Func<T, TProperty> property, IEnumerable<TProperty> sequence)
        {
            var sequenceList = sequence.ToList();
            var sequenceDictionary = sequenceList.ToDictionary(s => s, sequenceList.IndexOf);
            return @this.OrderBy(s => sequenceDictionary[property(s)]);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> @this, int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                throw new ArgumentException("Must be 1 or greater", page < 1 ? "page" : "pageSize");
            }

            return @this.Skip(--page * pageSize).Take(pageSize);
        }

        public static T ContainsOrDefault<T>(this IEnumerable<T> @this, T value)
        {
            return @this.Contains(value) ? value : default(T);
        }

        public static T RandomElement<T>(this ICollection<T> @this)
        {
            return @this.ElementAt(Random.Next(@this.Count));
        }

        public static TResult FirstOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            var match = source.FirstOrDefault(predicate);
            return Equals(match, default(TSource)) ? default(TResult) : selector(match);
        }

        /// <summary>
        /// Calculates aggregated hash code of all elements in a sequence.
        /// </summary>
        public static int GetSequenceHashCode<T>([NotNull] this IEnumerable<T> source, bool ignoreOrder = false)
        {
            // Calculate all hashes
            var hashes = source.Select(i => i?.GetHashCode() ?? 0);

            // If the order is irrelevant - order the list to ensure the order is always the same
            if (ignoreOrder)
                hashes = hashes.OrderBy(i => i);

            // Aggregate individual hashes
            var result = 19;
            foreach (var hash in hashes)
            {
                unchecked
                {
                    result = result * 31 + hash;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns elements with distinct keys.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> Distinct<T, TKey>([NotNull] this IEnumerable<T> @this, [NotNull] Func<T, TKey> keySelector, [NotNull] IEqualityComparer<TKey> keyComparer)
        {
            // Use a hashset to maintain uniqueness of keys
            var keyHashSet = new HashSet<TKey>(keyComparer);
            foreach (var element in @this)
            {
                if (keyHashSet.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Discards elements from a sequence that are equal to given value.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> Except<T>([NotNull] this IEnumerable<T> @this, T value, [NotNull] IEqualityComparer<T> comparer)
        {
            return @this.Where(i => !comparer.Equals(i, value));
        }

        /// <summary>
        /// Discards elements from a sequence that are equal to given value.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> Except<T>([NotNull] this IEnumerable<T> @this, T value) => @this.Except(value, EqualityComparer<T>.Default);

        /// <summary>
        /// Discards default values from a sequence.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> ExceptDefault<T>([NotNull] this IEnumerable<T> @this) => @this.Except(default!);

        /// <summary>
        /// Slices a sequence into a subsequence.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> Slice<T>([NotNull] this IEnumerable<T> @this, int startAt, int count)
        {
            // If count is zero - return empty
            if (count == 0)
                yield break;

            var i = 0;
            foreach (var element in @this)
            {
                // If the index is within range - yield element
                if (i >= startAt && i <= startAt + count - 1)
                {
                    yield return element;
                }

                // If the index is past bounds - break
                if (i >= startAt + count)
                {
                    yield break;
                }

                i++;
            }
        }

        /// <summary>
        /// Returns a specified number of contiguous elements at the end of a sequence.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> TakeLast<T>([NotNull] this IEnumerable<T> @this, int count)
        {
            // If count is 0 - return empty
            if (count == 0)
            {
                return Enumerable.Empty<T>();
            }

            // Buffer all elements
            var asReadOnlyList = @this as IReadOnlyList<T> ?? @this.ToArray();

            // If count is greater than element count - return source
            if (count >= asReadOnlyList.Count)
            {
                return asReadOnlyList;
            }

            // Otherwise - slice
            return asReadOnlyList.Slice(asReadOnlyList.Count - count, count);
        }

        /// <summary>
        /// Bypasses a specified number of contiguous elements at the end of a sequence.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> SkipLast<T>([NotNull] this IEnumerable<T> @this, int count)
        {
            // If count is 0 - return source
            if (count == 0)
            {
                return @this;
            }

            // Buffer all elements
            var asReadOnlyList = @this as IReadOnlyList<T> ?? @this.ToArray();

            // If count is greater than element count - return empty
            if (count >= asReadOnlyList.Count)
            {
                return Enumerable.Empty<T>();
            }

            // Otherwise - slice
            return asReadOnlyList.Slice(0, asReadOnlyList.Count - count);
        }

        /// <summary>
        /// Returns elements from the end of a sequence as long as a specified condition is true.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> TakeLastWhile<T>([NotNull] this IEnumerable<T> @this, [NotNull] Func<T, bool> predicate)
        {
            return @this.Reverse().TakeWhile(predicate).Reverse();
        }

        /// <summary>
        /// Bypasses elements from the end of a sequence as long as a specified condition is true.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> SkipLastWhile<T>([NotNull] this IEnumerable<T> @this, [NotNull] Func<T, bool> predicate)
        {
            return @this.Reverse().SkipWhile(predicate).Reverse();
        }

        /// <summary>
        /// Groups contiguous elements into a list based on a predicate.
        /// The predicate decides whether the next element should be added to the current group.
        /// If the predicate fails, the current group is closed and a new one, containing this element, is created.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<IReadOnlyList<T>> GroupContiguous<T>([NotNull] this IEnumerable<T> @this, [NotNull] Func<IReadOnlyList<T>, T, bool> groupPredicate)
        {
            // Create buffer
            var buffer = new List<T>();

            foreach (var element in @this)
            {
                // If buffer is not empty and group predicate failed - yield and reset buffer
                if (buffer.Any() && !groupPredicate(buffer, element))
                {
                    yield return buffer;
                    buffer = new List<T>(); // new instance to reset reference
                }

                // Add element to buffer
                buffer.Add(element);
            }

            // If buffer still has something after the source has been enumerated - yield
            if (buffer.Any())
            {
                yield return buffer;
            }
        }

        /// <summary>
        /// Groups contiguous elements into a list based on a predicate.
        /// The predicate decides whether the next element should be added to the current group.
        /// If the predicate fails, the current group is closed and a new one, containing this element, is created.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<IReadOnlyList<T>> GroupContiguous<T>([NotNull] this IEnumerable<T> @this, [NotNull] Func<IReadOnlyList<T>, bool> groupPredicate)
        {
            return @this.GroupContiguous((buffer, _) => groupPredicate(buffer));
        }

        private class GenericComparer<T> : IEqualityComparer<T>
        {
            public GenericComparer(Func<T, object> uniqueCheckerMethod)
            {
                this._uniqueCheckerMethod = uniqueCheckerMethod;
            }

            private readonly Func<T, object> _uniqueCheckerMethod;

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                return this._uniqueCheckerMethod(x).Equals(this._uniqueCheckerMethod(y));
            }

            int IEqualityComparer<T>.GetHashCode(T obj)
            {
                return this._uniqueCheckerMethod(obj).GetHashCode();
            }
        }
    }

    internal sealed class EnumerableIterator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public EnumerableIterator(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
            MoveNext();
        }

        public bool HasCurrent { get; private set; }

        public T Current
        {
            get { return _enumerator.Current; }
        }

        public void MoveNext()
        {
            HasCurrent = _enumerator.MoveNext();
        }
    }
    public sealed class Item<T>
    {
        public int Index { get; set; }

        public T Value { get; set; }

        public bool IsLast { get; set; }
    }
}
