//-----------------------------------------------------------------------
// <copyright file="ObservableSortedDictionary.cs" company="Lifeprojects.de">
//     Class: ObservableSortedDictionary
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>06.09.2020</date>
//
// <summary>
//  Definition of ObservableSortedDictionary Class
//  http://drwpf.com/blog/2007/09/16/can-i-bind-my-itemscontrol-to-a-dictionary/
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;


    [Serializable]
    public class ObservableSortedDictionary<TKey, TValue> : ObservableDictionary<TKey, TValue>, ISerializable, IDeserializationCallback
    {
        private IComparer<DictionaryEntry> _comparer;

        [NonSerialized]
        private readonly SerializationInfo _siInfo = null;

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer)
            : base()
        {
            _comparer = comparer;
        }

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _comparer = comparer;
        }

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IEqualityComparer<TKey> equalityComparer)
            : base(equalityComparer)
        {
            _comparer = comparer;
        }

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey> equalityComparer)
            : base(dictionary, equalityComparer)
        {
            _comparer = comparer;
        }

        protected ObservableSortedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _siInfo = info;
        }

        protected override bool AddEntry(TKey key, TValue value)
        {
            DictionaryEntry entry = new DictionaryEntry(key, value);
            int index = GetInsertionIndexForEntry(entry);
            _keyedEntryCollection.Insert(index, entry);
            return true;
        }

        protected virtual int GetInsertionIndexForEntry(DictionaryEntry newEntry)
        {
            return BinaryFindInsertionIndex(0, Count - 1, newEntry);
        }

        protected override bool SetEntry(TKey key, TValue value)
        {
            bool keyExists = _keyedEntryCollection.Contains(key);

            // if identical key/value pair already exists, nothing to do
            if (keyExists && value.Equals((TValue)_keyedEntryCollection[key].Value))
                return false;

            // otherwise, remove the existing entry
            if (keyExists)
                _keyedEntryCollection.Remove(key);

            // add the new entry
            DictionaryEntry entry = new DictionaryEntry(key, value);
            int index = GetInsertionIndexForEntry(entry);
            _keyedEntryCollection.Insert(index, entry);

            return true;
        }

        private int BinaryFindInsertionIndex(int first, int last, DictionaryEntry entry)
        {
            if (last < first)
                return first;
            else
            {
                int mid = first + (int)((last - first) / 2);
                int result = _comparer.Compare(_keyedEntryCollection[mid], entry);
                if (result == 0)
                    return mid;
                else if (result < 0)
                    return BinaryFindInsertionIndex(mid + 1, last, entry);
                else
                    return BinaryFindInsertionIndex(first, mid - 1, entry);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            /*
            if (!_comparer.GetType().IsSerializable)
            {
                throw new NotSupportedException("The supplied Comparer is not serializable.");
            }
            */

            base.GetObjectData(info, context);
            info.AddValue("_comparer", _comparer);
        }

        public override void OnDeserialization(object sender)
        {
            if (_siInfo != null)
            {
                _comparer = (IComparer<DictionaryEntry>)_siInfo.GetValue("_comparer", typeof(IComparer<DictionaryEntry>));
            }
            base.OnDeserialization(sender);
        }
    }
}