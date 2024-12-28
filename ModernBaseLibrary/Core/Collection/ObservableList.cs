//-----------------------------------------------------------------------
// <copyright file="ObservableList.cs" company="Lifeprojects.de">
//     Class: ObservableList
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>25.06.2019</date>
//
// <summary>
// Definition of ObservableList Class
// https://github.com/damieng/DamienGKit/tree/master/CSharp
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{

    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A list that can be observed for change or clear events.
    /// </summary>
    /// <typeparam name="T">Type of items in the list.</typeparam>
    public class ObservableList<T> : IList<T>
    {
        private readonly IList<T> internalList;

        public event EventHandler<ListChangedEventArgs> ListChanged = delegate { };
        public event EventHandler ListCleared = delegate { };

        public ObservableList()
        {
            internalList = new List<T>();
        }

        public ObservableList(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            internalList = list;
        }

        public ObservableList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.internalList = new List<T>(collection);
        }

        public int Count
        {
            get { return this.internalList.Count; }
        }

        public bool IsReadOnly
        {
            get { return internalList.IsReadOnly; }
        }

        public T this[int index]
        {
            get { return this.internalList[index]; }
            set
            {
                if (this.internalList[index].Equals(value)) return;

                this.internalList[index] = value;
                OnListChanged(new ListChangedEventArgs(index, value));
            }
        }

        public int IndexOf(T item)
        {
            return this.internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.internalList.Insert(index, item);
            OnListChanged(new ListChangedEventArgs(index, item));
        }

        public void RemoveAt(int index)
        {
            var item = this.internalList[index];
            this.internalList.RemoveAt(index);
            OnListChanged(new ListChangedEventArgs(index, item));
        }

        public void Add(T item)
        {
            this.internalList.Add(item);
            OnListChanged(new ListChangedEventArgs(this.internalList.IndexOf(item), item));
        }

        public void Clear()
        {
            this.internalList.Clear();
            this.OnListCleared(new EventArgs());
        }

        public bool Contains(T item)
        {
            return this.internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                var index = this.internalList.IndexOf(item);
                if (this.internalList.Remove(item))
                {
                    OnListChanged(new ListChangedEventArgs(index, item));
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.internalList).GetEnumerator();
        }

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            this.ListChanged(this, e);
        }

        protected virtual void OnListCleared(EventArgs e)
        {
            this.ListCleared(this, e);
        }

        public class ListChangedEventArgs : EventArgs
        {
            private readonly int index;
            private readonly T item;

            internal ListChangedEventArgs(int index, T item)
            {                
                this.index = index;
                this.item = item;
            }

            public int Index
            {
                get { return this.index; }
            }

            public T Item
            {
                get { return this.item; }
            }
        }
    }
}