//-----------------------------------------------------------------------
// <copyright file="FunctionalExtensions.cs" company="Lifeprojects.de">
//     Class: FunctionalExtensions
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.03.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    public static class FunctionalExtensions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var e in source)
            {
                action(e);
                yield return e;
            }
        }

        public static int Do<T>(this IEnumerable<T> list, Action<int, T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var index = 0;

            foreach (var elem in list)
            {
                action(index++, elem);
            }

            return index;
        }

        public static int Do<T>(this List<T> list, Action<int, T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var index = 0;

            foreach (var elem in list)
            {
                action(index++, elem);
            }

            return index;
        }

        public static int Do<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<int,TKey, TValue> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            int index = 0;

            foreach (var kvp in dictionary)
            {
                action(index++,kvp.Key, kvp.Value);
            }

            return index;
        }

        public static void CountSum<T>(this IEnumerable<T> items, Action<int> pPerfomAction)
        {
            pPerfomAction(items.Count());
        }

        public static void Use<T>(this T item, Action<T> action) where T : IDisposable
        {
            using (item)
            {
                action(item);
            }
        }

        public static IEnumerable<U> Convert<T, U>(this IEnumerable<T> items, Func<T, U> conversion)
        {
            foreach (T item in items)
            {
                yield return conversion(item);
            }
        }

        public static IEnumerable<KeyValuePair<T, Int32>> GetCounts<T>(this IEnumerable<T> items, Func<T, T, Boolean> pEqualityComparerMethod)
        {
            Dictionary<T, Int32> result = new Dictionary<T, int>(new EqualityComparer<T>(pEqualityComparerMethod));

            foreach (T item in items)
            {
                if (result.ContainsKey(item))
                {
                    result[item]++;
                }
                else
                {
                    result.Add(item, 1);
                }
            }

            return result;
        }

        public static T WhereFirstOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).FirstOrDefault();
        }

        public static T WhereFirstOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).FirstOrDefault();
        }

        public static T WhereLastOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).LastOrDefault();
        }

        public static T WhereLastOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).LastOrDefault();
        }

        public static PropertyInfo GetPropertyInfo<TType, TReturn>(this Expression<Func<TType, TReturn>> property)
        {
            LambdaExpression lambda = property;

            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            return (PropertyInfo)memberExpression.Member;
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and a given delimiter.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, (sb, e) => sb.Append(e));
        }

        private static string ToDelimitedStringImpl<T>(IEnumerable<T> source, string delimiter, Func<StringBuilder, T, StringBuilder> append)
        {
            var sb = new StringBuilder();
            var i = 0;

            foreach (var value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                append(sb, value);
            }

            return sb.ToString();
        }

        public static string GetPropertyName<TType, TReturn>(this Expression<Func<TType, TReturn>> property) => property.GetPropertyInfo().Name;

        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            public EqualityComparer(Func<T, T, Boolean> pEqualityComparerMethod)
            {
                EqualityComparerMethod = pEqualityComparerMethod;
            }

            public Func<T, T, Boolean> EqualityComparerMethod { get; set; }

            public bool Equals(T x, T y)
            {
                return EqualityComparerMethod(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}