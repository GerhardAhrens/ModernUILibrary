//-----------------------------------------------------------------------
// <copyright file="ColumnIndexPropertyAttribute.cs" company="Lifeprojects.de">
//     Class: ColumnIndexPropertyAttribute
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>ColumnIndexPropertyAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.Property,AllowMultiple =true)]
    public class ColumnIndexAttribute : Attribute
    {
        public ColumnIndexAttribute(string columnName, string indexGroup, double order, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            this.ColumnName = columnName;
            this.IndexGroup = indexGroup;
            this.Order = order;
            this.SortDirection = sortDirection;
        }

        public ColumnIndexAttribute(string columnName, string indexGroup, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            this.ColumnName = columnName;
            this.IndexGroup = indexGroup;
            this.Order = 1;
            this.SortDirection = sortDirection;
        }

        public ColumnIndexAttribute(string columnName, double order, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            this.ColumnName = columnName;
            this.IndexGroup = string.Empty;
            this.Order = order;
            this.SortDirection = sortDirection;
        }

        public ColumnIndexAttribute(string columnName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            this.ColumnName = columnName;
            this.IndexGroup = string.Empty;
            this.Order = 1;
            this.SortDirection = sortDirection;
        }

        public string ColumnName
        {
            get;
            set;
        }

        public string IndexGroup
        {
            get;
            set;
        }

        public double Order
        {
            get;
            set;
        }

        public ListSortDirection SortDirection
        {
            get;
            set;
        }
    }
}