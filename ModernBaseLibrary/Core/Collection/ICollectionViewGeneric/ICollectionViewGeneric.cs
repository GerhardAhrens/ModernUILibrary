//-----------------------------------------------------------------------
// <copyright file="ICollectionViewGeneric.cs" company="Lifeprojects.de">
//     Class: ICollectionViewGeneric
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>15.04.2025 08:18:28</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using ModernBaseLibrary.Extension;

    public interface ICollectionView<T> : IEnumerable<T>, ICollectionView
    {
        IEnumerable<T> SourceCollectionGeneric { get; }
        int CountRow { get; }
        int CountFilter { get; }
    }

    public class CollectionViewGeneric<T> : ICollectionView<T>
    {
        private readonly ICollectionView _collectionView;

        public CollectionViewGeneric(ICollectionView generic)
        {
            this.CountRow = ((System.Windows.Data.ListCollectionView)generic).Count;
            this._collectionView = generic;
        }

        private class MyEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator _enumerator;
            public MyEnumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            public T Current { get { return (T)_enumerator.Current; } }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyEnumerator(this._collectionView.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._collectionView.GetEnumerator();
        }

        public bool Contains(object item)
        {
            return this._collectionView.Contains(item);
        }

        public void Refresh()
        {
            this._collectionView.Refresh();
        }

        public IDisposable DeferRefresh()
        {
            return this._collectionView.DeferRefresh();
        }

        public bool MoveCurrentToFirst()
        {
            return this._collectionView.MoveCurrentToFirst();
        }

        public bool MoveCurrentToLast()
        {
            return this._collectionView.MoveCurrentToLast();
        }

        public bool MoveCurrentToNext()
        {
            return this._collectionView.MoveCurrentToNext();
        }

        public bool MoveCurrentToPrevious()
        {
            return this._collectionView.MoveCurrentToPrevious();
        }

        public bool MoveCurrentTo(object item)
        {
            return this._collectionView.MoveCurrentTo(item);
        }

        public bool MoveCurrentToPosition(int position)
        {
            return this._collectionView.MoveCurrentToPosition(position);
        }

        public int CountRow { get; private set; }

        public int CountFilter { get; private set; }

        public CultureInfo Culture
        {
            get { return this._collectionView.Culture; }
            set { this._collectionView.Culture = value; }
        }

        public IEnumerable SourceCollection
        {
            get 
            {
                return this._collectionView.SourceCollection; 
            }
        }

        public Predicate<object> Filter
        {
            get { return this._collectionView.Filter; }
            set 
            { 
                this._collectionView.Filter = value;
                this.CountFilter = ((System.Windows.Data.ListCollectionView)_collectionView).Count;
            }
        }

        public bool CanFilter
        {
            get { return this._collectionView.CanFilter; }
        }

        public SortDescriptionCollection SortDescriptions
        {
            get { return this._collectionView.SortDescriptions; }
        }

        public bool CanSort
        {
            get { return this._collectionView.CanSort; }
        }

        public bool CanGroup
        {
            get { return this._collectionView.CanGroup; }
        }

        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return this._collectionView.GroupDescriptions; }
        }

        public ReadOnlyObservableCollection<object> Groups
        {
            get { return this._collectionView.Groups; }
        }

        public bool IsEmpty { get { return this._collectionView.IsEmpty; } }

        public object CurrentItem { get { return this._collectionView.CurrentItem; } }

        public int CurrentPosition { get { return this._collectionView.CurrentPosition; } }

        public bool IsCurrentAfterLast { get { return this._collectionView.IsCurrentAfterLast; } }

        public bool IsCurrentBeforeFirst { get { return this._collectionView.IsCurrentBeforeFirst; } }

        public event CurrentChangingEventHandler CurrentChanging
        {
            add
            {
                lock (this.objectLock)
                {
                    this._collectionView.CurrentChanging += value;
                }
            }
            remove
            {
                lock (this.objectLock)
                {
                    this._collectionView.CurrentChanging -= value;
                }
            }
        }

        object objectLock = new object();
        public event EventHandler CurrentChanged
        {
            add
            {
                lock (this.objectLock)
                {
                    this._collectionView.CurrentChanged += value;
                }
            }
            remove
            {
                lock (this.objectLock)
                {
                    this._collectionView.CurrentChanged -= value;
                }
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (this.objectLock)
                {
                    this._collectionView.CollectionChanged += value;
                }
            }
            remove
            {
                lock (this.objectLock)
                {
                    this._collectionView.CollectionChanged -= value;
                }
            }
        }

        public IEnumerable<T> SourceCollectionGeneric
        {
            get { return this._collectionView.Cast<T>(); }
        }
    }
}
