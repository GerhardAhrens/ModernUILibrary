//-----------------------------------------------------------------------
// <copyright file="ILexiconCollection.cs" company="Lifeprojects.de">
//     Class: ILexiconCollection
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
    using System.Collections;
    using System.Collections.Generic;

    public interface ILexiconCollection<K,V> : ICollection<KeyValuePair<K,V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
    {
        // Properties

        int KeyCount { get; }
        ICollection<K> Keys { get; }
        ICollection<List<V>> ValueLists { get; }
        IEnumerable<V> Values { get; } 
        List<V> this[K key] { get; set; }

        // Methods
        
        void Add(K key, V value);
        void AddList(K key, List<V> valueList);
        bool ChangeValue(K key, V oldvalue, V newValue);
        bool Contains (K key, V value); 
        bool ContainsKey(K key);       
        int GetValueCount(K key);
        bool Remove(K key, V value);
        bool RemoveKey(K key);
        bool TryGetValueList(K key, out List<V> valueList);        
    }
}
