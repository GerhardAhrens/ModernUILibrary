namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public class PropertyDescriptorComparerDC<T> : IComparer<T>
    {
        private const int ASCENDING = 1;
        private const int DESCENDING = -1;

        private readonly int sortDirection;
        private readonly PropertyDescriptor propertyDescriptor;
        private readonly IComparer comparer;

        public PropertyDescriptorComparerDC(PropertyDescriptor propertyDescriptor, ListSortDirection sortDirection)
        {
            this.propertyDescriptor = propertyDescriptor;
            comparer = getComparerFromDescriptor();

            this.sortDirection = sortDirection == ListSortDirection.Ascending ? ASCENDING : DESCENDING;
        }

        private IComparer getComparerFromDescriptor()
        {
            Type comparerType = typeof(Comparer<>);
            Type comparerForPropertyType = comparerType.MakeGenericType(propertyDescriptor.PropertyType);

            return (IComparer)comparerForPropertyType.InvokeMember("Default",
                                                                     BindingFlags.GetProperty |
                                                                     BindingFlags.Public |
                                                                     BindingFlags.Static,
                                                                     null, null, null);
        }

        public int Compare(T x, T y)
        {
            object xValue = propertyDescriptor.GetValue(x);
            object yValue = propertyDescriptor.GetValue(y);

            return sortDirection * comparer.Compare(xValue, yValue);
        }
    }
}
