namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;

    [Serializable()]
    public class DataCollection<T> : BindingList<T>
    {
        private const int NO_ITEM_INDEX = -1;

        [NonSerialized()]
        private ListSortDirection listSortDirection;

        [NonSerialized()]
        private PropertyDescriptor propertyDescriptor;

        [NonSerialized()]
        private bool isSorted;

        protected override bool IsSortedCore
        {
            get
            {
                return this.isSorted;
            }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return this.listSortDirection;
            }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                return this.propertyDescriptor;
            }
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        public DataCollection(IEnumerable<T> enumeration) : base(new List<T>(enumeration))
        {
        }

        protected override void ApplySortCore(PropertyDescriptor propertyDesciptor, ListSortDirection sortDirection)
        {
            this.isSorted = true;
            this.listSortDirection = sortDirection;
            this.propertyDescriptor = propertyDesciptor;

            var comparer = this.createComparer(propertyDesciptor, sortDirection);

            sort(comparer);
        }

        protected virtual IComparer<T> createComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            return new PropertyDescriptorComparerDC<T>(property, direction);
        }

        private void sort(IComparer<T> comparer)
        {
            ((List<T>)Items).Sort(comparer);
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, NO_ITEM_INDEX));
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;

            for (int itemIndex = 0; itemIndex < count; itemIndex++)
            {
                T item = this[itemIndex];
                var itemValue = property.GetValue(item);
                if (itemValue.Equals(key))
                {
                    return itemIndex;
                }
            }

            return NO_ITEM_INDEX;
        }

        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.listSortDirection = base.SortDirectionCore;
            this.propertyDescriptor = base.SortPropertyCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, NO_ITEM_INDEX));
        }

        public void AddRange(IEnumerable<T> itemsToAdd)
        {
            foreach (T item in itemsToAdd)
            {
                this.Add(item);
            }
        }

        public ListCollectionView View(Predicate<object> methode)
        {
            ListCollectionView collectionView = new ListCollectionView(this);
            collectionView.Filter = methode;

            return collectionView;
        }

        public int FindIndex(string property, object key)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find(property, true);

            if (prop == null)
            {
                return -1;
            }
            else
            {
                return FindCore(prop, key);
            }
        }

        public void ResetModified()
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find("IsModified", true);
            if (prop != null)
            {
                var filtedItems = this.Where(p => ((IBaseModelDC)p).IsModified == true);
                foreach  (T item in filtedItems)
                {
                    var itemValue = prop.GetValue(item);
                    if (itemValue.Equals(true))
                    {
                        prop.SetValue(item, false);
                    }
                }
            }
        }
    }
}