//-----------------------------------------------------------------------
// <copyright file="MultiValueDictionary.cs" company="Lifeprojects.de">
//     Class: MultiValueDictionary
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.02.2019</date>
//
// <summary>Definition of MultiValueDictionary Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class MultiValueDictionary<TKey> : IDictionary<TKey, Tuple<string,string>>, IEnumerable
    {
        private readonly IDictionary<TKey, Tuple<string, string>> internCollection;

        /// <summary>
        /// Created new MultiValueDictionary<TKey> with Generic Key
        /// </summary>
        public MultiValueDictionary()
        {
            this.internCollection = new Dictionary<TKey, Tuple<string, string>>();
        }

        /// <summary>
        /// Created new MultiValueDictionary<TKey> with Generic Key
        /// </summary>
        /// <param name="key">Generic Key</param>
        /// <param name="value1">Value 1, String</param>
        /// <param name="value2">Value 2, String</param>
        public MultiValueDictionary(TKey key, string value1, string value2) : base()
        {
            this.internCollection = new Dictionary<TKey, Tuple<string, string>>();
            if (this.internCollection != null)
            {
                this.Add(key, value1, value1);
            }
        }

        public virtual int Count
        {
            get
            {
                return this.internCollection.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get { return this.internCollection.IsReadOnly; }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.internCollection.Keys;
            }
        }

        public ICollection<Tuple<string, string>> Values
        {
            get
            {
                return this.internCollection.Values;
            }
        }

        public Tuple<string, string> this[TKey key]
        {
            get
            {
                return this.internCollection[key];
            }

            set
            {
                value = this.internCollection[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Add new entry on MultiValueDictionary
        /// </summary>
        /// <param name="key">Generic Key</param>
        /// <param name="value1">Value 1, String</param>
        /// <param name="value2">Value 2, String</param>
        public void Add(TKey key, string value1, string value2)
        {
            if (this.internCollection != null && this.internCollection.ContainsKey(key) == false)
            {
                this.internCollection.Add(key, new Tuple<string, string>(value1, value2));
            }
        }

        public void Add(TKey key, Tuple<string, string> value)
        {
            if (this.internCollection != null && this.internCollection.ContainsKey(key) == false)
            {
                this.internCollection.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, Tuple<string, string>> item)
        {
            if (this.internCollection != null && this.internCollection.Contains(item) == false)
            {
                this.internCollection.Add(item);
            }
        }

        public virtual void Clear()
        {
            this.internCollection.Clear();
        }

        public virtual bool Contains(KeyValuePair<TKey, Tuple<string, string>> item)
        {
            return this.internCollection.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.internCollection.ContainsKey(key);
        }

        public bool ContainsValue(Tuple<string, string> value)
        {
            return this.internCollection.Values.Contains(value);
        }

        public virtual void CopyTo(KeyValuePair<TKey, Tuple<string, string>>[] array, int arrayIndex)
        {
            this.internCollection.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<TKey, Tuple<string, string>> item)
        {
            return this.internCollection.Remove(item.Key);
        }
        public virtual IEnumerator<KeyValuePair<TKey, Tuple<string, string>>> GetEnumerator()
        {
            return this.internCollection.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out Tuple<string, string> value)
        {
            return this.internCollection.TryGetValue(key, out value);
        }

        public bool Remove(TKey key)
        {
            return this.internCollection.Remove(key);
        }
    }
}
