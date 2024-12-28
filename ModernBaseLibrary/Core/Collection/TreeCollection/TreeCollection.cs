namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represent data structure of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the tree.</typeparam>
    /// <typeparam name="TValue">The type of values in the tree.</typeparam>
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class TreeCollection<TKey, TValue> : ITreeCollection<TKey, TValue> where TKey : IComparable<TKey>
    {

        #region Fields

        private int _itemsCount = 0;
        private TreeCollectionNode<TKey, TValue> _treeNode = null;
        private TreeCollectionNode<TKey, TValue> _treeNodeNull = null;
        private TreeCollectionNode<TKey, TValue> _treeNodeLast = null;

        #endregion

        #region Constructors

        public TreeCollection()
        {
            _treeNodeNull = new TreeCollectionNode<TKey, TValue>();
            _treeNodeNull.LeftNode = null;
            _treeNodeNull.RightNode = null;
            _treeNodeNull.Parent = null;
            _treeNodeNull.Color = TreeCollectionColor.Black;

            _treeNode = _treeNodeNull;
            _treeNodeLast = _treeNodeNull;
        }

        #endregion

        #region Public Interfaces

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>
        /// The value associated with the specified key. If the specified key is not
        /// found, a get operation throws a System.Collections.Generic.KeyNotFoundException,
        /// and a set operation creates a new element with the specified key.
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and key is not found.
        /// </exception>
        public TValue this[TKey key]
        {
            get
            {
                Contract.Requires(key != null);
                TValue value = default(TValue);

                FindValue(key, out value);

                return value;
            }
            set
            {
                Contract.Requires(key != null);
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///  Gets the number of elements contained in the tree.
        /// </summary>
        public int Count
        {
            get { return _itemsCount; }
        }

        /// <summary>
        ///  Gets a collection containing the keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection containing the values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentException">
        /// An element with the same key already exists.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            Contract.Requires(key != null);
            Contract.Requires(value != null);

            int ret = 0;

            TreeCollectionNode<TKey, TValue> rbTreeNode = new TreeCollectionNode<TKey, TValue>();
            TreeCollectionNode<TKey, TValue> tmpRBTreeNode = _treeNode;

            // Vyhladam svojho otca kde sa pojdem vlozit.
            while (tmpRBTreeNode != _treeNodeNull)
            {
                rbTreeNode.Parent = tmpRBTreeNode;
                ret = key.CompareTo(tmpRBTreeNode.Key);

                if (ret == 0)
                {
                    throw (new ArgumentException(string.Format("An element with the same key already exists. Key: {0}", key)));
                }

                if (ret > 0)
                {
                    tmpRBTreeNode = tmpRBTreeNode.RightNode;
                }
                else
                {
                    tmpRBTreeNode = tmpRBTreeNode.LeftNode;
                }
            }

            rbTreeNode.Key = key;
            rbTreeNode.Value = value;
            rbTreeNode.LeftNode = _treeNodeNull;  // Lavy syn je null
            rbTreeNode.RightNode = _treeNodeNull; // Pravy syn je null

            // Ak niesom TopItem tak sa priradim otcovi
            if (rbTreeNode.Parent != null)
            {
                // Priradim sa otcovi bud ako pravy, alebo ako pravy podstrom
                ret = rbTreeNode.Key.CompareTo(rbTreeNode.Parent.Key);

                if (ret > 0)
                {
                    rbTreeNode.Parent.RightNode = rbTreeNode;
                }
                else
                {
                    rbTreeNode.Parent.LeftNode = rbTreeNode;
                }
            }
            else
                //inak sa prehlasim za koren
                _treeNode = rbTreeNode;

            //reorganizujem sa
            Reorganizuj(rbTreeNode);

            _treeNodeLast = rbTreeNode;

            _itemsCount++;
        }

        /// <summary>
        /// Gets a value indicating whether the tree is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="item">Key and value pair to add.</param>
        /// <exception cref="System.ArgumentException">
        /// An element with the same key already exists.
        /// </exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Contract.Requires(item.Key != null);
            Contract.Requires(item.Value != null);

            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.Dictionary<TKey,TValue>
        /// contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the three.</param>
        /// <returns>
        /// true if the tree contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            Contract.Requires(key != null);
            TValue value;

            return FindValue(key, out value);
        }

        /// <summary>
        /// Removes the value with the specified key from the tree.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully found and removed; otherwise, false.
        /// This method returns false if key is not found.
        /// </returns>
        public bool Remove(TKey key)
        {
            Contract.Requires(key != null);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the
        /// value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the tree contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            Contract.Requires(key != null);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all keys and values from the tree.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.Dictionary<TKey,TValue>
        /// contains the specified key.
        /// </summary>
        /// <param name="item">Key value pair.</param>
        /// <returns>
        /// true if the tree contains an element with the specified key and value; otherwise, false.
        /// </returns>        
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// throw new NotImplementedException();
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the tree.
        /// </summary>
        /// <param name="item">The object to remove from the tree.</param>
        /// <returns>
        /// true if item was successfully removed from the tree;
        /// otherwise, false. This method also returns false if item is not found in the original tree.
        /// </returns>
        /// <exception cref="System.NotSupportedException">
        /// The System.Collections.Generic.ICollection<T> is read-only.
        /// </exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private helpers

        private bool FindValue(TKey key, out TValue value)
        {
            int result;
            TreeCollectionNode<TKey, TValue> treeNode = _treeNode;

            while (treeNode != _treeNodeNull)
            {
                result = key.CompareTo(treeNode.Key);
                if (result == 0)
                {
                    _treeNodeLast = treeNode;
                    value = treeNode.Value;
                    return true;
                }

                if (result < 0)
                    treeNode = treeNode.LeftNode;
                else
                    treeNode = treeNode.RightNode;
            }
            value = default(TValue);

            return false;
        }


        /// <summary>
        /// Vyvažovanie stromu podľa novovloženého prvku.
        /// </summary>
        /// <param name="iRBTreeNode">Novovložený prvok.</param>
        private void Reorganizuj(TreeCollectionNode<TKey,TValue> iRBTreeNode)
        {
            TreeCollectionNode<TKey,TValue> rbTreeNode;

            // Pokial otec je cerveny (dva cervene zasebou)
            while (iRBTreeNode != _treeNode && iRBTreeNode.Parent.Color == TreeCollectionColor.Red)
            {
                // Zistim si ci moj otec je lavym, alebo pravym potomkom
                if (iRBTreeNode.Parent == iRBTreeNode.Parent.Parent.LeftNode)
                {
                    rbTreeNode = iRBTreeNode.Parent.Parent.RightNode;

                    // Ak brat mojho otca je cerveny
                    if (rbTreeNode != null && rbTreeNode.Color == TreeCollectionColor.Red)
                    {
                        // Tak prefarbi 
                        iRBTreeNode.Parent.Color = TreeCollectionColor.Black;
                        rbTreeNode.Color = TreeCollectionColor.Black;
                        iRBTreeNode.Parent.Parent.Color = TreeCollectionColor.Red;

                        // Kontroluj dalej od dedka
                        iRBTreeNode = iRBTreeNode.Parent.Parent;
                    }
                    else
                    {
                        // Inak rotuj

                        // Ak som pravym synom tak lavo prava rotacia
                        if (iRBTreeNode == iRBTreeNode.Parent.RightNode)
                        {
                            iRBTreeNode = iRBTreeNode.Parent;
                            RotujVLavo(iRBTreeNode);
                        }

                        // Inak len prava
                        iRBTreeNode.Parent.Color = TreeCollectionColor.Black;
                        iRBTreeNode.Parent.Parent.Color = TreeCollectionColor.Red;
                        RotujVPravo(iRBTreeNode.Parent.Parent);
                    }
                }
                else
                {
                    rbTreeNode = iRBTreeNode.Parent.Parent.LeftNode;

                    // Ak brat mojho otca je cerveny
                    if (rbTreeNode != null && rbTreeNode.Color == TreeCollectionColor.Red)
                    {
                        // Tak prefarbi
                        iRBTreeNode.Parent.Color = TreeCollectionColor.Black;
                        rbTreeNode.Color = TreeCollectionColor.Black;
                        iRBTreeNode.Parent.Parent.Color = TreeCollectionColor.Red;
                        iRBTreeNode = iRBTreeNode.Parent.Parent;
                    }
                    else
                    {
                        // Inak rotuj

                        // Ak som lavym synom
                        if (iRBTreeNode == iRBTreeNode.Parent.LeftNode)
                        {
                            //tak najskor rotuj v pravo
                            iRBTreeNode = iRBTreeNode.Parent;
                            RotujVPravo(iRBTreeNode);
                        }

                        // Rotujem vlavo
                        iRBTreeNode.Parent.Color = TreeCollectionColor.Black;
                        iRBTreeNode.Parent.Parent.Color = TreeCollectionColor.Red;
                        RotujVLavo(iRBTreeNode.Parent.Parent);
                    }
                }
            }
            _treeNode.Color = TreeCollectionColor.Black;
        }

        /// <summary>
        /// Pravá rotácia.
        /// </summary>
        /// <param name="iRBTreeNode">Prvok, podľa ktorého rotujem.</param>
        private void RotujVPravo(TreeCollectionNode<TKey,TValue> iRBTreeNode)
        {
            TreeCollectionNode<TKey,TValue> rbTreeNode = iRBTreeNode.LeftNode;

            iRBTreeNode.LeftNode = rbTreeNode.RightNode;

            if (rbTreeNode.RightNode != _treeNodeNull)
            {
                rbTreeNode.RightNode.Parent = iRBTreeNode;
            }

            if (rbTreeNode != _treeNodeNull)
            {
                rbTreeNode.Parent = iRBTreeNode.Parent;
            }

            if (iRBTreeNode.Parent != null)
            {
                if (iRBTreeNode == iRBTreeNode.Parent.RightNode)
                {
                    iRBTreeNode.Parent.RightNode = rbTreeNode;
                }
                else
                {
                    iRBTreeNode.Parent.LeftNode = rbTreeNode;
                }
            }
            else
            {
                _treeNode = rbTreeNode;
            }

            rbTreeNode.RightNode = iRBTreeNode;

            if (iRBTreeNode != _treeNodeNull)
            {
                iRBTreeNode.Parent = rbTreeNode;
            }
        }

        /// <summary>
        /// Ľavá rotácia podľa prvku.
        /// </summary>
        /// <param name="iRBTreeNode">Prvok, podľa ktorého rotujem.</param>
        private void RotujVLavo(TreeCollectionNode<TKey,TValue> iRBTreeNode)
        {
            // Nastavim sa
            TreeCollectionNode<TKey,TValue> rbTreeNode = iRBTreeNode.RightNode;

            // Otcovi dam ako praveho syna svoj lavy podstrom
            iRBTreeNode.RightNode = rbTreeNode.LeftNode;

            if (rbTreeNode.LeftNode != _treeNodeNull)
            {
                rbTreeNode.LeftNode.Parent = iRBTreeNode; //presmerujem otca
            }

            if (rbTreeNode != _treeNodeNull)
            {
                // Nastavim si za otca otca mojho otca
                rbTreeNode.Parent = iRBTreeNode.Parent;
            }

            if (iRBTreeNode.Parent != null)
            {
                // Nastavim sa svojnu dedkovy ako pravy, alebo lavy syn
                if (iRBTreeNode == iRBTreeNode.Parent.LeftNode)
                {
                    iRBTreeNode.Parent.LeftNode = rbTreeNode;
                }
                else
                {
                    iRBTreeNode.Parent.RightNode = rbTreeNode;
                }
            }
            else _treeNode = rbTreeNode;

            rbTreeNode.LeftNode = iRBTreeNode;

            if (iRBTreeNode != _treeNodeNull)
            {
                iRBTreeNode.Parent = rbTreeNode;
            }
        }

        #endregion
    }
}
