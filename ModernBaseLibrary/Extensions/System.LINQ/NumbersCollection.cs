//-----------------------------------------------------------------------
// <copyright file="NumbersCollection.cs" company="Lifeprojects.de">
//     Class: NumbersCollection
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Concept functions for Custom Linq Extensions
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows.Data;

    [SupportedOSPlatform("windows")]
    public class NumbersCollection : IEnumerable<Nummer>
    {
        private List<Nummer> _Numbers;
        private int index = 0;

        public Nummer Current
        {
            get {
                Nummer current = null;
                if (this._Numbers != null)
                {
                    if (index >= 0)
                    {
                        index = index > _Numbers.Count ? _Numbers.Count : index;
                        current = _Numbers[index];
                    }
                }

                return current;
            }
        }

        public NumbersCollection()
        {
            if (_Numbers == null)
            {
                _Numbers = new List<Nummer>();
            }
        }

        public NumbersCollection(IEnumerable<Nummer> numbers)
        {
            numbers.IsArgumentNotNull("Der Parameter darf nicht 'null' sein");

            _Numbers = numbers.ToList();
        }

        public ListCollectionView View()
        {
            ListCollectionView collectionView = new ListCollectionView(_Numbers);

            return collectionView;
        }

        public ListCollectionView View(Predicate<object> methode)
        {
            ListCollectionView collectionView = new ListCollectionView(_Numbers);
            collectionView.Filter = methode;

            return collectionView;
        }

        public void Add(Nummer number)
        {
            number.IsArgumentNotNull("Der Parameter darf nicht 'null' sein");

            if (_Numbers == null)
            {
                _Numbers = new List<Nummer>();
            }

            _Numbers.Add(number);
        }

        public void Remove(Nummer number)
        {
            number.IsArgumentNotNull("Der Parameter darf nicht 'null' sein");

            if (this._Numbers != null && number != null)
            {
                bool isRemove = this._Numbers.Remove(number);
            }
        }

        public void Remove(Predicate<Nummer> isMatch)
        {
            for (int i = 0; i < this._Numbers.Count; i++)
            {
                Nummer number = _Numbers[i];
                if (number != null)
                {
                    if (isMatch(number))
                    {
                        bool isRemove = this._Numbers.Remove(number);
                    }
                }
            }
        }

        public void Clear()
        {
            if (_Numbers != null)
            {
                _Numbers.Clear();
            }
        }

        public void MoveTo(int moveTo)
        {
            moveTo.IsArgumentInRange("moveTo", moveTo < 0, "Der Parameter darf nicht kleiner '0' sein");
            moveTo.IsArgumentInRange("moveTo", moveTo > _Numbers.Count, $"Der Parameter darf nicht größer '{_Numbers.Count}' sein");

            if (_Numbers != null && _Numbers.Count >= moveTo)
            {
                index = moveTo;
            }
        }

        public Nummer Get(Predicate<Nummer> isMatch)
        {
            foreach (Nummer number in this._Numbers)
            {
                if (isMatch(number))
                {
                    return number;
                }
            }

            return null;
        }

        public IEnumerator<Nummer> GetEnumerator()
        {
            foreach (Nummer numbers in this._Numbers)
            {
                yield return numbers;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._Numbers.GetEnumerator();
        }
    }
}