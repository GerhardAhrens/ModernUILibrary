//-----------------------------------------------------------------------
// <copyright file="LexiconCollection.cs" company="Lifeprojects.de">
//     Class: LexiconCollection
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.05.2022</date>
//
// <summary>
// Lexikon Collection Klasse
// https://www.c-sharpcorner.com/UploadFile/b942f9/a-dictionary-class-which-permits-duplicate-keys/
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class LexiconCollection<K, V> : IEnumerable<KeyValuePair<K, V>>, ICollection<KeyValuePair<K, V>>, ILexiconCollection<K,V>, IEnumerable, ICollection, ISerializable, IDeserializationCallback
    {
        private readonly Dictionary<K,List<V>> lexiconDict;
       
        public LexiconCollection()
        {
            this.lexiconDict = new Dictionary<K, List<V>>();
        }

        public LexiconCollection(IDictionary<K, V> dictionary)
        {
            this.lexiconDict = new Dictionary<K, List<V>>();
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                this.lexiconDict.Add(key, list);
            }
        }

        public LexiconCollection(ILexiconCollection<K, V> lexicon)
        {
            this.lexiconDict = new Dictionary<K, List<V>>();
            foreach (K key in lexicon.Keys)
            {
                this.lexiconDict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public LexiconCollection(IEqualityComparer<K> comparer)
        {
            this.lexiconDict = new Dictionary<K, List<V>>(comparer);
        }

        public LexiconCollection(int capacity)
        {
            this.lexiconDict = new Dictionary<K, List<V>>(capacity);
        }

        public LexiconCollection(IDictionary<K, V> dictionary, IEqualityComparer<K> comparer)
        {
            this.lexiconDict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                this.lexiconDict.Add(key, list);
            }
        }

        public LexiconCollection(ILexiconCollection<K, V> lexicon, IEqualityComparer<K> comparer)
        {
            this.lexiconDict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in lexicon.Keys)
            {
                this.lexiconDict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public LexiconCollection(int capacity, IEqualityComparer<K> comparer)
        {
            this.lexiconDict = new Dictionary<K, List<V>>(capacity, comparer);
        }

        protected LexiconCollection(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                return;
            }

            this.lexiconDict = (Dictionary<K, List<V>>)info.GetValue("InternalDictionary", typeof(Dictionary<K, List<V>>));
        }

        public IEqualityComparer<K> Comparer
        {
            get { return this.lexiconDict.Comparer; }
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (List<V> list in this.lexiconDict.Values)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public int KeyCount
        {

            get { return this.lexiconDict.Keys.Count; }

        }

        public ICollection<K> Keys
        {
            get { return this.lexiconDict.Keys; }
        }

        public ICollection<List<V>> ValueLists
        {
            get { return this.lexiconDict.Values; }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (K key in this.lexiconDict.Keys)
                {
                    foreach (V value in this.lexiconDict[key])
                    {
                        yield return value;
                    }
                }
            }
        }

        public List<V> this[K key]
        {
            get { return this.lexiconDict[key]; }
            set { this.lexiconDict[key] = new List<V>(value); }
        }


        public V this[K key, int index]
        {
            get
            {
                List<V> list = this.lexiconDict[key];
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentException("Index out of range for key");
                }

                return list[index];
            }

            set
            {
                if (this.lexiconDict.ContainsKey(key))
                {
                    List<V> list = this.lexiconDict[key];

                    if (index < 0 || index > list.Count)
                    {
                        throw new ArgumentException("Index out of range for key");
                    }
                    else if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                    }
                }
                else if (index == 0)
                {
                    List<V> list = new List<V>();
                    list.Add(value);
                    this.lexiconDict.Add(key, list);
                }
                else
                {
                    throw new ArgumentException("Index out of range for key");
                }
            }
        }

        public void Add(K key, V value)
        {
            if (this.lexiconDict.ContainsKey(key))
            {
                List<V> list = this.lexiconDict[key];
                list.Add(value);
            }
            else
            {
                List<V> list = new List<V>();
                list.Add(value);
                this.lexiconDict.Add(key, list);
            }
        }

        public void Add(KeyValuePair<K, V> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddList(K key, List<V> valueList)
        {
            if(this.lexiconDict.ContainsKey(key))
            {
               List<V> list = this.lexiconDict[key];
               foreach (V val in valueList)
                {
                    list.Add(val);
                }
            }
            else
            {
                this.lexiconDict.Add(key, new List<V>(valueList));
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<K, V>> keyValuePairs)
        {
            foreach (KeyValuePair<K, V> kvp in keyValuePairs)
            {
                this.Add(kvp.Key, kvp.Value);
            }
        }

        public bool ChangeValue(K key, V oldValue, V newValue)
        {
            if (this.lexiconDict.ContainsKey(key))
            {
               List<V> list = this.lexiconDict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], oldValue))
                  {
                     list[i] = newValue;
                     return true;
                  }
               }
            }

            return false;
        }

        public bool ChangeValueAt(K key, int index, V newValue)
        {
           if (this.lexiconDict.ContainsKey(key))
           {                                    
               List<V> list = this.lexiconDict[key];
                    
               if (index < 0 || index >= list.Count)
               {
                  return false;
               }                    
               else
               {
                  list[index] = newValue;
                  return true;
               }
           }

           return false;
        }  

        public void Clear()
        {
            this.lexiconDict.Clear();
        }

        public bool Contains(K key, V value)
        {
            if (this.lexiconDict.ContainsKey(key))
            {
                List<V> list = this.lexiconDict[key];
                foreach (V val in list)
                {
                    if (Object.Equals(val, value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Contains(KeyValuePair<K, V> keyValuePair)
        {
            return this.Contains(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsKey(K key)
        {
            return this.lexiconDict.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            K firstKey;
            return ContainsValue(value, out firstKey);
        }

        public bool ContainsValue(V value, out K firstKey)
        {
            foreach (K key in lexiconDict.Keys)
            {
                foreach (V val in lexiconDict[key])
                {
                    if (Object.Equals(val, value))
                    {
                        firstKey = key;       
                        return true;
                    }  
                }
            }
            firstKey = default(K); 
            return false;
        }
      
        public void CopyTo(KeyValuePair<K, V>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length < index + this.Count)
            {
                throw new ArgumentException();
            }

            int i = index;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                array[i++] = kvp;
            }
        }

        public IEnumerable<KeyValuePair<K, int>> FindKeyIndexPairs(V value)
        {
            foreach (K key in this.lexiconDict.Keys)
            {
                List<V> list = this.lexiconDict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    if (Object.Equals(list[i], value))
                    {
                       yield return new KeyValuePair<K, int>(key, i);
                    }
                }
            }
        }
               
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in this.lexiconDict.Keys)
            {
                List<V> list = this.lexiconDict[key];
                foreach (V value in list)
                {
                    yield return new KeyValuePair<K, V>(key, value);
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                return;
            }

            info.AddValue("InternalDictionary", this.lexiconDict);
        }

        public int GetValueCount(K key)
        {
            if (this.lexiconDict.ContainsKey(key))
            {
                return this.lexiconDict[key].Count;
            }

            return 0;
        }

        public int IndexOfValue(K key, V value)
        {
            if (this.lexiconDict.ContainsKey(key))
            {
               List<V> list = this.lexiconDict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], value))
                    {
                        return i;
                    }
               }
            }

            return -1;
        }
      
        public virtual void OnDeserialization(object sender)
        {
            // nothing to do
        }

        public bool Remove(K key, V value)
        {
            int count = this.GetValueCount(key);
            if (count == 0) return false;
            for (int i = 0; i < count; i++)
            {
                V val = this.lexiconDict[key][i];
                if (Object.Equals(val, value))
                {
                    if (count == 1)
                    {
                        this.lexiconDict.Remove(key);
                    }
                    else  
                    {
                        this.lexiconDict[key].RemoveAt(i);
                    }
                    return true;
                }
            }

            return false;
        }

        public bool Remove(KeyValuePair<K, V> keyValuePair)
        {
            return this.Remove(keyValuePair.Key, keyValuePair.Value);
        }

        public bool RemoveAt(K key, int index)
        {
            int count = this.GetValueCount(key);
            if (count == 0 || index < 0 || index >= count) return false;
            if (count == 1)
            {
                this.lexiconDict.Remove(key);
            }
            else
            {
                List<V> list = this.lexiconDict[key];
                list.RemoveAt(index);               
            }
            return true;
        }

        public bool RemoveKey(K key)
        {
            int count = this.GetValueCount(key);
            if (count > 0)
            {
                this.lexiconDict.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryGetValueList(K key, out List<V> valueList)
        {
            return this.lexiconDict.TryGetValue(key, out valueList);
        }
               
        public bool TryGetValueAt(K key, int index, out V value)
        {
            if (this.lexiconDict.ContainsKey(key) && index >= 0 && index < this.lexiconDict[key].Count)
            {
                value = this.lexiconDict[key][index];
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly 
        { 
            get { return false; } 
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.lexiconDict).SyncRoot; }
        }

        // Explicit Method Implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((KeyValuePair<K, V>[]) array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
