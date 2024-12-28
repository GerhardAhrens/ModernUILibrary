namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represent data structure of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the tree.</typeparam>
    /// <typeparam name="TValue">The type of values in the tree.</typeparam>
    public interface ITreeCollection<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
    }
}
