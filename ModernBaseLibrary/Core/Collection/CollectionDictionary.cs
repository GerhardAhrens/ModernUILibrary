//-----------------------------------------------------------------------
// <copyright file="CollectionDictionary.cs" company="Lifeprojects.de">
//     Class: CollectionDictionary
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2017</date>
//
// <summary>Definition of CollectionDictionary Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System.Collections.Generic;

    public class CollectionDictionary<TKey, TValue> : Dictionary<TKey, ICollection<TValue>>
    {
        private readonly TValue[] empty = new TValue[0];

        public CollectionDictionary()
        {
        }

        public CollectionDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.LoadFromDictionary(dictionary);
        }

        public CollectionDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public CollectionDictionary(int capacity) : base(capacity)
        {
        }

        public CollectionDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(comparer)
        {
            this.LoadFromDictionary(dictionary);
        }

        public CollectionDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public int ValueCount
        {
            get
            {
                int count = 0;
                foreach (var kvp in this)
                {
                    count += kvp.Value.Count;
                }

                return count;
            }
        }

        public void Add(TKey key, TValue value)
        {
            ICollection<TValue> collection;
            if (this.TryGetValue(key, out collection) == false)
            {
                collection = new List<TValue>();
                base.Add(key, collection);
            }

            collection.Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            ICollection<TValue> collection;
            if (this.TryGetValue(key, out collection) == false)
            {
                collection = new List<TValue>();
                base.Add(key, collection);
            }

            foreach (TValue value in values)
            {
                collection.Add(value);
            }
        }

        public void RemoveValue(TKey key, TValue value)
        {
            ICollection<TValue> collection;
            if (this.TryGetValue(key, out collection))
            {
                collection.Remove(value);
                if (collection.Count == 0)
                {
                    this.Remove(key);
                }
            }
        }

        public bool ContainsValue(TValue value)
        {
            foreach (var kvp in this)
            {
                if (kvp.Value.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        public ICollection<TValue> GetValuesOrEmpty(TKey key)
        {
            ICollection<TValue> values;
            if (this.TryGetValue(key, out values))
            {
                return values;
            }

            return this.empty;
        }

        private void LoadFromDictionary(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var kvp in dictionary)
            {
                var list = new List<TValue>();
                list.Add(kvp.Value);
                this.Add(kvp.Key, list);
            }
        }
    }
}