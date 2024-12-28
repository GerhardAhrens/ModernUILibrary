//-----------------------------------------------------------------------
// <copyright file="NavigationList.cs" company="Lifeprojects.de">
//     Class: DefaultClass1
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.02.2023 13:06:35</date>
//
// <summary>
// Collection-Klasse in der man sich mit Next und Previous durch die Liste bewegen kann.
// Zusätzlich stehen weitere Methoden zur Navigation in der Liste zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows.Data;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [Serializable]
    public class NavigationCollection<TSource> : List<TSource>
    {
        private ICollectionView source;

        public NavigationCollection(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.Collection = CollectionViewSource.GetDefaultView(source);
        }

        public NavigationCollection()
        {
        }

        public IEnumerable<TSource> Source
        {
            set { this.source = CollectionViewSource.GetDefaultView(value); }
        }


        public ICollectionView Collection { get; private set; }

        public bool IsBegin { get { return this.Collection.IsCurrentBeforeFirst; } }

        public bool IsEnd { get { return this.Collection.IsCurrentAfterLast; } }

        public int CurrentPosition { get { return this.Collection.CurrentPosition; } }

        public TSource CurrentItem { get { return this.Collection.CurrentItem.To<TSource>(); } }

        public TSource ToNext()
        {
            bool end = this.Collection.MoveCurrentToNext();
            if (end != false)
            {
                return this.Collection.CurrentItem.To<TSource>();
            }

            return default;
        }

        public TSource ToPrevious()
        {
            bool begin = this.Collection.MoveCurrentToPrevious();
            if (begin != false)
            {
                return this.Collection.CurrentItem.To<TSource>();
            }

            return default;
        }

        public TSource First()
        {
            this.Collection.MoveCurrentToFirst();
            return this.Collection.CurrentItem.To<TSource>();
        }

        public TSource Last()
        {
            this.Collection.MoveCurrentToLast();
            return this.Collection.CurrentItem.To<TSource>();
        }

        /// <summary>
        /// Gib einen Bereich als IEnumerable<T> zurück
        /// </summary>
        /// <param name="predicate">Parameter</param>
        /// <returns>Ergebnis als IEnumerable</returns>
        public IEnumerable<TSource> RangeOf(Func<TSource, bool> predicate)
        {
            IEnumerable<TSource> result = this.Collection.Cast<TSource>().Where(predicate);
            return result;
        }

        /// <summary>
        /// Anzahl der Element, die  auf die Parameter zutreffen
        /// </summary>
        /// <param name="predicate">Parameter</param>
        /// <returns></returns>
        public int CountOf(Func<TSource, bool> predicate)
        {
            int result = this.Collection.Cast<TSource>().Count(predicate);
            return result;
        }

        /// <summary>
        /// Mindestens ein Element trifft auf die Parameter zu
        /// </summary>
        /// <param name="predicate">Parameter</param>
        /// <returns>True, wenn mindestens ein Item auf die Parameter zutrifft</returns>
        public bool AnyOf(Func<TSource, bool> predicate)
        {
            bool result = this.Collection.Cast<TSource>().Any(predicate);
            return result;
        }

        public IEnumerable<TSource> NextOf(Func<TSource, bool> predicate)
        {
            TSource pos = this.Collection.CurrentItem.To<TSource>();
            foreach (TSource item in this.Collection)
            {
                bool itemCondition = predicate(item);
                if (itemCondition == true)
                {
                    yield return item;
                }
            }

            this.Collection.MoveCurrentTo(pos);
        }

        public IEnumerable<TSource> PreviousOf(Func<TSource, bool> predicate)
        {
            TSource pos = this.Collection.CurrentItem.To<TSource>();
            foreach (TSource item in this.Collection.Cast<TSource>().Reverse())
            {
                bool itemCondition = predicate(item);
                if (itemCondition == true)
                {
                    yield return item;
                }
            }

            this.Collection.MoveCurrentTo(pos);
        }

        public Predicate<object> Filter(Predicate<object> predicate)
        {
            return this.Collection.Filter = predicate;
        }
    }
}
