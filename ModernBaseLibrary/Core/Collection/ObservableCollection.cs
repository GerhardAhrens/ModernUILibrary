//-----------------------------------------------------------------------
// <copyright file="ObservableCollectionEx.cs" company="Lifeprojects.de">
//     Class: ObservableCollectionEx
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>25.06.2019</date>
//
// <summary>Definition of ObservableCollectionEx Class</summary>
//-----------------------------------------------------------------------

namespace System.Collections.ObjectModel
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ObservableCollectionEx()
        {
        }

        /// <summary>
        /// Constructor with list of initial items.
        /// </summary>
        /// <param name="data">Initial items.</param>
        public ObservableCollectionEx(List<T> items) : base(items)
        {
        }

        /// <summary>
        /// Constructor with list of initial items.
        /// </summary>
        /// <param name="data">Initial items.</param>
        public ObservableCollectionEx(IEnumerable<T> items) : base(items)
        {
        }

        /// <summary>
        /// Replaces a given item with a new one (first occurance).
        /// </summary>
        /// <param name="oldItem">The item to replace.</param>
        /// <param name="newItem">The new item.</param>
        public void Replace(T oldItem, T newItem)
        {
            this.CheckReentrancy();

            int foundIndex = this.Items.IndexOf(oldItem);
            if (foundIndex >= 0)
            {
                this.Items[foundIndex] = newItem;

                // Raise relevant notifications after all items are added
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Items)));
            }
        }

        /// <summary>
        /// Adds multiple items without nofiy after each single item.
        /// (better performance than loops with single Add calls).
        /// </summary>
        /// <param name="itemsToAdd">The items to add.</param>
        public void AddRange(IEnumerable<T> itemsToAdd)
        {
            this.CheckReentrancy();

            if (itemsToAdd == null || !itemsToAdd.Any())
            {
                return;
            }

            int countBeforeAdding = this.Count;

            foreach (var item in itemsToAdd)
            {
                this.Items.Add(item);
            }

            // Raise relevant notifications after all items are added
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Count)));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Items)));
        }

        /// <summary>
        /// Remove multiple items without notify after each single item.
        /// (better performance than loops with single Remove calls).
        /// </summary>
        /// <param name="itemsToRemove">The items to remove.</param>
        public void RemoveItems(IEnumerable<T> itemsToRemove)
        {
            this.CheckReentrancy();

            if (itemsToRemove == null || !itemsToRemove.Any())
            {
                return;
            }

            int countBeforeRemoving = this.Count;

            foreach (var item in itemsToRemove)
            {
                this.Items.Remove(item);
            }

            // Raise relevant notifications after all items are removed
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Count)));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Items)));
        }
    }
}
