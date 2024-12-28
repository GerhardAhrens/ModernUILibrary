namespace ModernBaseLibrary.Collection
{
    using System;

    /// <summary>
    /// This class represent one node from RB tree.
    /// </summary>
    internal class TreeCollectionNode<TKey, TValue> where TKey : IComparable<TKey>
    {

        #region Properties

        /// <summary>
        /// Get or set Key of node.
        /// </summary>
        public TKey Key
        {
            get;
            set;
        }


        /// <summary>
        /// Get or set value of node.
        /// </summary>
        public TValue Value
        {
            get;
            set;
        }


        /// <summary>
        /// Get or set color of node.
        /// </summary>
        public TreeCollectionColor Color
        {
            get;
            set;
        }


        /// <summary>
        /// Get or set left child of node.
        /// </summary>
        public TreeCollectionNode<TKey, TValue> LeftNode
        {
            get;
            set;
        }


        /// <summary>
        /// Get or ser right child of node.
        /// </summary>
        public TreeCollectionNode<TKey, TValue> RightNode
        {
            get;
            set;
        }


        /// <summary>
        /// Get or set parent of node.
        /// </summary>
        public TreeCollectionNode<TKey, TValue> Parent
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string that represents the current node.
        /// </summary>
        /// <returns>A string that represents the current node.</returns>
        public override string ToString()
        {
            return string.Format("Key: {0}; Value: {1}", Key, Value);
        }

        #endregion
    }

    /// <summary>
    /// Enum represent node color
    /// </summary>
    internal enum TreeCollectionColor
    {
        Red,
        Black,
    }
}
