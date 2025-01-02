/*
 * <copyright file="DataGridFilterCommon.cs" company="Lifeprojects.de">
 *     Class: DataGridFilterCommon
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>02.01.2025</date>
 * <Project>ModernUILibrary</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
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

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class DataGridFilterCommon : NotifyPropertyDG
    {
        public DataGridFilterCommon()
        {
            PreviouslyFilteredItems = new HashSet<object>(EqualityComparer<object>.Default);
        }

        public string FieldName { get; set; }
        public Type FieldType { get; set; }
        public bool IsFiltered { get; set; }
        public HashSet<object> PreviouslyFilteredItems { get; set; }

        // Treeview
        public List<DataGridFilterItem> Tree { get; set; }

        /// <summary>
        ///     Recursive call for check/uncheck all items in tree
        /// </summary>
        /// <param name="state"></param>
        /// <param name="updateChildren"></param>
        /// <param name="updateParent"></param>
        private void SetIsChecked(DataGridFilterItem item, bool? state, bool updateChildren, bool updateParent)
        {
            try
            {
                if (state == item.IsDateChecked)
                {
                    return;
                }

                item.SetDateState = state;

                // select all / unselect all (recursive call)
                if (item.Level == 0)
                {
                    Tree.Where(t => t.Level != 0).ToList().ForEach(c => { SetIsChecked(c, state, true, true); });
                }

                if (updateChildren && item.IsDateChecked.HasValue)
                {
                    item.Children?.ForEach(c => { SetIsChecked(c, state, true, false); });
                }

                if (updateParent && item.Parent != null)
                {
                    this.VerifyCheckedState(item.Parent);
                }

                item.OnPropertyChanged("IsDateChecked");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SetDateState : {ex.Message}");
            }
        }

        /// <summary>
        /// Update the item tree when the state of the IsDateChecked property changes
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnUpdateTree(object o, bool? e)
        {
            if (o == null)
            {
                return;
            }

            var item = (DataGridFilterItem)o;
            this.SetIsChecked(item, e, true, true);
        }

        /// <summary>
        ///     check or uncheck parents or children
        /// </summary>
        private void VerifyCheckedState(DataGridFilterItem item)
        {
            bool? state = null;

            for (var i = 0; i < item.Children?.Count; ++i)
            {
                var current = item.Children[i].IsDateChecked;

                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(item, state, false, true);
        }

        /// <summary>
        ///     Add the filter to the predicate dictionary
        /// </summary>
        public void AddFilter(Dictionary<string, Predicate<object>> criteria)
        {
            if (this.IsFiltered)
            {
                return;
            }

            bool Predicate(object o)
            {
                var value = o.GetType().GetProperty(this.FieldName)?.GetValue(o, null);
                return !PreviouslyFilteredItems.Contains(value);
            }

            criteria.Add(this.FieldName, Predicate);

            this.IsFiltered = true;
        }

        /// <summary>
        ///     Check if any tree item is checked (can apply filter)
        /// </summary>
        /// <returns></returns>
        public bool AnyDateIsChecked()
        {
            // any year is != false
            return Tree?.Skip(1).Any(t => t.IsDateChecked != false) ?? false;
        }

        /// <summary>
        ///     Build the item tree
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="currentFilter"></param>
        /// <param name="uncheckPrevious"></param>
        /// <returns></returns>
        public IEnumerable<DataGridFilterItem> BuildTree(IEnumerable<object> dates, bool uncheckPrevious = false)
        {
            if (dates == null)
            {
                return null;
            }

            try
            {
                var dateTimes = dates.ToList();

                this.Tree = new List<DataGridFilterItem>
                {
                    new DataGridFilterItem
                    {
                        Label = DataGridLoc.All, CurrentFilter = this, Content = 0, Level = 0, SetDateState = true
                    }
                };

                // event subscription
                this.Tree.First().OnIsCheckedDate += this.OnUpdateTree;

                // iterate over all items that are not null
                // INFO:
                // SetDateState  : does not raise OnIsCheckedDate event
                // IsDateChecked : raise OnIsCheckedDate event
                // (see the FilterItem class for more informations)

                foreach (var y in from date in dateTimes.Where(d => d != null)
                                  .Select(d => (DateTime)d).OrderBy(o => o.Year)
                                  group date by date.Year
                    into year
                                  select new DataGridFilterItem
                                  {
                                      // YEAR
                                      Level = 1,
                                      CurrentFilter = this,
                                      Content = year.Key,
                                      Label = year.First().ToString("yyyy"),
                                      SetDateState = true,

                                      Children = (from date in year
                                                  group date by date.Month
                                          into month
                                                  select new DataGridFilterItem
                                                  {
                                                      // MOUNTH
                                                      Level = 2,
                                                      CurrentFilter = this,
                                                      Content = month.Key,
                                                      Label = month.First().ToString("MMMM", DataGridLoc.Culture),
                                                      SetDateState = true,

                                                      Children = (from day in month
                                                                  select new DataGridFilterItem
                                                                  {
                                                                      // DAY
                                                                      Level = 3,
                                                                      CurrentFilter = this,
                                                                      Content = day.Day,
                                                                      Label = day.ToString("dd", DataGridLoc.Culture),
                                                                      SetDateState = true,

                                                                      Children = new List<DataGridFilterItem>()
                                                                  }).ToList()
                                                  }).ToList()
                                  })
                {
                    y.OnIsCheckedDate += OnUpdateTree;

                    y.Children.ForEach(m =>
                    {
                        m.Parent = y;
                        m.OnIsCheckedDate += OnUpdateTree;

                        m.Children.ForEach(d =>
                        {
                            d.Parent = m;
                            d.OnIsCheckedDate += OnUpdateTree;

                            if (PreviouslyFilteredItems != null && uncheckPrevious)
                                d.IsDateChecked = PreviouslyFilteredItems
                                        .Any(u => u != null && u.Equals(new DateTime((int)y.Content, (int)m.Content,
                                            (int)d.Content))) == false;
                        });
                    });

                    this.Tree.Add(y);
                }

                // last empty item
                if (dateTimes.Any(d => d == null))
                    Tree.Add(
                        new DataGridFilterItem
                        {
                            Label = DataGridLoc.Empty, // translation
                            CurrentFilter = this,
                            Content = -1,
                            Level = -1,
                            SetDateState = !PreviouslyFilteredItems?.Any(u => u == null) == true,
                            Children = new List<DataGridFilterItem>()
                        }
                    );

                // event subscription if an empty element exists
                if (this.Tree.LastOrDefault(c => c.Level == -1) != null)
                {
                    this.Tree.Last(c => c.Level == -1).OnIsCheckedDate += OnUpdateTree;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BuildTree : {ex.Message}");
            }

            return this.Tree;
        }

        /// <summary>
        ///     Get all the items from the tree (checked or unchecked)
        /// </summary>
        /// <returns></returns>
        public List<DataGridFilterItem> GetAllItemsTree()
        {
            var filterCommon = new List<DataGridFilterItem>();

            try
            {
                foreach (var y in Tree.Skip(1))
                    if ((int)y.Content > 0)
                        filterCommon.AddRange(
                            from m in y.Children
                            from d in m.Children
                            select new DataGridFilterItem
                            {
                                Content = new DateTime((int)y.Content, (int)m.Content, (int)d.Content),
                                IsChecked = d.IsDateChecked ?? false
                            });
                    else
                        filterCommon.Add(new DataGridFilterItem
                        {
                            Content = null,
                            IsChecked = y.IsDateChecked ?? false
                        });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllItemsTree : {ex.Message}");
            }

            return filterCommon;
        }
    }
}