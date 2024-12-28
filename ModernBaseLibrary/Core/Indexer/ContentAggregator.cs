//-----------------------------------------------------------------------
// <copyright file="ContentAggregator.cs" company="Lifeprojects.de">
//     Class: ContentAggregator
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.02.2023</date>
//
// <summary>
// Die Klasse verwaltet eine Liste beliebiger Werte.
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ContentAggregator<TValue> where TValue: class
    {
        private readonly ConcurrentDictionary<string, TValue> contentCollection;
        public ContentAggregator()
        {
            contentCollection = new ConcurrentDictionary<string, TValue>();
        }

        public ContentAggregator(IEnumerable<KeyValuePair<string, TValue>> contentCollection) : this()
        {
            foreach (KeyValuePair<string, TValue> item in contentCollection)
            {
                this.AddOrSet(item.Key, item.Value);
            }
        }

        ~ContentAggregator()
        {
            contentCollection.Clear();
       
        }

        public TValue this[string key] => this.Get(key);

        public bool HasAnyContainer => this.contentCollection.Any();

        public void AddOrSet(string key, TValue valueContent)
        {
            if (this.contentCollection.Any(k => k.Key == key))
            {
                this.contentCollection[key] = valueContent;
            }
            else
            {
                this.contentCollection.AddOrUpdate(key, valueContent, (exkey, excmd) => valueContent);
            }
        }

        public TValue Get(string key)
        {
            lock (this.contentCollection)
            {
                if (this.contentCollection.Any(k => k.Key == key))
                {
                    return this.contentCollection[key];
                }
                else
                {
                    return default;
                }
            }
        }

        public void Remove(string key)
        {
            lock (this.contentCollection)
            {
                if (this.contentCollection.Any(k => k.Key == key))
                {
                    TValue oldValue = null;
                    this.contentCollection.TryRemove(key, out oldValue);
                }
            }
        }

        public void RemoveAll()
        {
            if (this.contentCollection != null)
            {
                lock (this.contentCollection)
                {
                    this.contentCollection.Clear();
                }
            }
        }

        public bool Exists(string key)
        {
            if (contentCollection == null)
            {
                return false;
            }

            lock (this.contentCollection)
            {
                return this.contentCollection.Any(k => k.Key == key);
            }
        }

        public int Count()
        {
            int count = -1;
            if (this.contentCollection != null)
            {
                lock (this.contentCollection)
                {
                    count = this.contentCollection.Count();
                }
            }
            return (count);
        }
    }
}