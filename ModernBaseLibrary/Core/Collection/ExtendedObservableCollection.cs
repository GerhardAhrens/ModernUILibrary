//-----------------------------------------------------------------------
// <copyright file="ExtendedObservableCollection.cs" company="Lifeprojects.de">
//     Class: ExtendedObservableCollection
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.05.2022</date>
//
// <summary>
// ExtendedObservableCollection Klasse
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class ExtendedObservableCollectionEx<T> : ObservableCollection<T>
    {
        #region Private Members

        /// <summary>
        /// The suppress notification flag.
        /// </summary>
        private bool suppressSingleAddNotification;

        #endregion

        #region Properties

        #endregion

        #region Constructor(s) and Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class.
        /// </summary>
        public ExtendedObservableCollectionEx()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="initData">The initialize data.</param>
        public ExtendedObservableCollectionEx(IEnumerable<T> initData) : base(initData)
        {
        }

        #endregion Constructor(s) and Destructor

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.suppressSingleAddNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        public new void Clear()
        {
            this.ClearItems();
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            try
            {
                this.suppressSingleAddNotification = true;
                base.ClearItems();
            }
            finally
            {
                this.suppressSingleAddNotification = false;
            }
        }

        /// <summary>
        /// Adds the data range to the collection without single add notifications.
        /// </summary>
        /// <remarks>
        /// The suppression of the single notification leads to a better performance.
        /// </remarks>
        /// <param name="dataList">The data list to be added.</param>
        public void AddRange(IEnumerable<T> dataList)
        {
            if (dataList == null)
            {
                return;
            }

            this.suppressSingleAddNotification = true;

            foreach (T item in dataList)
            {
                this.Add(item);
            }
            this.suppressSingleAddNotification = false;

            // notify collection changed only at the end of the action
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion Methods
    }
}