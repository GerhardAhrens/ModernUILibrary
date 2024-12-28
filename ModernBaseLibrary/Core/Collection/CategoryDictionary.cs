/*
 * <copyright file="CategoryDictionary.cs" company="Lifeprojects.de">
 *     Class: CategoryDictionary
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse CategoryDictionary bildet einen Key mit zwei Values, und einen Typ ab. Der zweite Value ist vom Type Object
 * für einen Cast des zweiten Value. Die weitere Funktionalität wird über IDictionary abgebildet.
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Die Klasse CategoryDictionary bildet einen Key mit zwei Values, und einen Typ ab. Der zweite Value ist vom Type Object
    /// für einen Cast des zweiten Value.
    /// </summary>
    [DebuggerStepThrough]
    public class CategoryDictionary : IDictionary<string, CategoryItem>
    {
        private readonly IDictionary<string, CategoryItem> internCollection;

        public CategoryDictionary()
        {
            this.internCollection = new Dictionary<string, CategoryItem>();
        }

        public CategoryDictionary(string key, string category, object value, Type typ) : base()
        {
            this.internCollection = new Dictionary<string, CategoryItem>();
            if (this.internCollection != null)
            {
                this.Add(key, category, value, typ);
            }
        }

        public virtual int Count
        {
            get
            {
                return this.internCollection.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get { return this.internCollection.IsReadOnly; }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.internCollection.Keys;
            }
        }

        public ICollection<CategoryItem> Values
        {
            get
            {
                return this.internCollection.Values;
            }
        }

        public CategoryItem this[string key]
        {
            get
            {
                return this.internCollection[key];
            }

            set
            {
                value = this.internCollection[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(string key, string category, object value, Type typ)
        {
            if (this.internCollection != null && this.internCollection.ContainsKey(key) == false)
            {
                this.internCollection.Add(key, new CategoryItem(category, value, typ));
            }
        }

        public void Add(string key, CategoryItem value)
        {
            if (this.internCollection != null && this.internCollection.ContainsKey(key) == false)
            {
                this.internCollection.Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, CategoryItem> item)
        {
            if (this.internCollection != null && this.internCollection.Contains(item) == false)
            {
                this.internCollection.Add(item);
            }
        }

        public virtual void Clear()
        {
            this.internCollection.Clear();
        }

        public virtual bool Contains(KeyValuePair<string, CategoryItem> item)
        {
            return this.internCollection.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.internCollection.ContainsKey(key);
        }

        public virtual void CopyTo(KeyValuePair<string, CategoryItem>[] array, int arrayIndex)
        {
            this.internCollection.CopyTo(array, arrayIndex);
        }


        public virtual bool Remove(KeyValuePair<string, CategoryItem> item)
        {
            return this.internCollection.Remove(item.Key);
        }

        public virtual IEnumerator<KeyValuePair<string, CategoryItem>> GetEnumerator()
        {
            return this.internCollection.GetEnumerator();
        }

        public bool TryGetValue(string key, out CategoryItem value)
        {
            return this.internCollection.TryGetValue(key, out value);
        }

        public bool Remove(string key)
        {
            return this.internCollection.Remove(key);
        }
    }
}
