namespace ModernBaseLibrary.Collection
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Eine KeyedCollection, die Ereignisse auslöst, wenn Elemente hinzugefügt, entfernt oder geändert werden.
    /// </summary>
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged, IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        public ObservableKeyedCollection()
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        /// <param name="dictionaryCreationThreshold">The number of elements that the collection can hold without creating a lookup dictionary.</param>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="items">The items that are copied to the new collection.</param>
        public ObservableKeyedCollection(IEnumerable<TItem> items)
        {
            if (items == null)
                return;
            int index = 0;
            foreach (var item in items)
            {
                base.InsertItem(index, item);
                index++;
            }
        }

        #endregion

        private bool deferNotify = false;

        /// <summary>
        /// Ersetzt das Element am angegebenen Index durch das angegebene Element.
        /// </summary>
        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        /// <summary>
        /// Fügt ein Element am angegebenen Index ein.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Fügt der Sammlung mehrere Elemente hinzu und löst ein einzelnes CollectionChanged-Ereignis aus.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
            {
                return;
            }

            var added = false;
            try
            {
                deferNotify = true;
                foreach (var item in items)
                {
                    Add(item);
                    added = true;
                }
            }
            finally
            {
                deferNotify = false;
            }

            if (!added)
            {
                return;
            }

            /* UIElement unterstützt nicht das Ändern mehrerer Elemente auf einmal, also verwenden Sie die Reset-Aktion. */
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Entfernt mehrere Elemente aus dem Wörterbuch und löst ein einzelnes CollectionChanged-Ereignis aus.
        /// </summary>
        public void RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                return;
            }

            var removed = false;
            try
            {
                deferNotify = true;
                foreach (var key in keys)
                {
                    if (Remove(key))
                    {
                        removed = true;
                    }
                }
            }
            finally
            {
                deferNotify = false;
            }

            if (!removed)
            {
                return;
            }

            /* UIElement unterstützt nicht das Ändern mehrerer Elemente auf einmal, also verwenden Sie die Reset-Aktion. */
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Entfernt mehrere Elemente aus dem Wörterbuch und löst ein einzelnes CollectionChanged-Ereignis aus.
        /// </summary>
        /// <param name="items">The the items to remove.</param>
        public void RemoveRange(IEnumerable<TItem> items)
        {
            if (items == null)
            {
                return;
            }

            var removed = false;
            try
            {
                deferNotify = true;
                foreach (var item in items)
                {
                    if (Remove(item))
                    {
                        removed = true;
                    }
                }
            }
            finally
            {
                deferNotify = false;
            }

            if (!removed)
            {
                return;
            }

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Entfernt alle Elemente aus der Liste.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Entfernt alle Elemente aus der Liste.
        /// </summary>
        /// <param name="index">Der auf Null basierende Index des zu entfernenden Elements.</param>
        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the OnCollectionChanged event.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.deferNotify)
            {
                return;
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Tritt auf, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Tritt auf, wenn ein Element hinzugefügt, entfernt, geändert oder verschoben wird oder wenn die gesamte Liste aktualisiert wird.
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();
            if (reader.IsEmptyElement)
            {
                return;
            }

            var ser = new XmlSerializer(typeof(TItem));
            var nodeType = reader.MoveToContent();

            if (nodeType == XmlNodeType.None)
            {
                return;
            }

            while (nodeType != XmlNodeType.EndElement)
            {
                var item = (TItem)ser.Deserialize(reader);
                if (item == null)
                {
                    continue;
                }

                Add(item);
                nodeType = reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var ser = new XmlSerializer(typeof(TItem));
            foreach (TItem item in this)
            {
                ser.Serialize(writer, item);
            }
        }

        #endregion
    }
}
