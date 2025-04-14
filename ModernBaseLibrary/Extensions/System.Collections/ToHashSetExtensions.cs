namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;


    public static class ToHashSetExtensions
    {
        public static HashSet<TKey> ToHashSetEx<TKey>(this IEnumerable<TKey> source) => ToHashSetEx(source, (IEqualityComparer<TKey>)null);

        public static HashSet<TKey> ToHashSetEx<TKey>(this IEnumerable<TKey> source, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            var d = new HashSet<TKey>(comparer);

            if (source is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return d;

                if (collection is TKey[] array)
                    for (var i = 0; i < array.Length; i++)
                        d.Add(array[i]);

                if (collection is List<TKey> list)
                    for (var i = 0; i < list.Count; i++)
                        d.Add(list[i]);
            }

            foreach (var element in source)
                d.Add(element);

            return d;
        }

        public static HashSet<TKey> ToHashSetEx<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>  ToHashSetEx(source, keySelector, null);

        public static HashSet<TKey> ToHashSetEx<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            if (keySelector == null)
                throw new ArgumentException(nameof(keySelector));

            var capacity = 0;
            if (source is ICollection<TSource> collection)
            {
                capacity = collection.Count;
                if (capacity == 0)
                    return new HashSet<TKey>(comparer);

                if (collection is TSource[] array)
                    return ToHashSetEx(array, keySelector, comparer);

                if (collection is List<TSource> list)
                    return ToHashSetEx(list, keySelector, comparer);
            }

            var d = new HashSet<TKey>(comparer);
            foreach (var element in source)
                d.Add(keySelector(element));

            return d;
        }
        #region Private
        private static HashSet<TKey> ToHashSetEx<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var d = new HashSet<TKey>(comparer);
            for (var i = 0; i < source.Length; i++)
                d.Add(keySelector(source[i]));

            return d;
        }
        private static HashSet<TKey> ToHashSetEx<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var d = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
                d.Add(keySelector(element));

            return d;
        }
        #endregion Private
    }
}