/*
 * <copyright file="DataGridFilterItem.cs" company="Lifeprojects.de">
 *     Class: DataGridFilterItem
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

    public class DataGridFilterItem : NotifyPropertyDG
    {
        private bool isChecked;
        private bool? isDateChecked;
        private bool notify;

        public event EventHandler<bool?> OnIsCheckedDate;

        public List<DataGridFilterItem> Children { get; set; }

        // raw value of the item (not displayed, see Label property)
        public object Content { get; set; }

        public DataGridFilterCommon CurrentFilter { get; set; }

        public Type FieldType { get; set; }

        public int Id { get; set; }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (value == isChecked)
                {
                    return;
                }

                isChecked = value;

                if (notify)
                {
                    base.OnPropertyChanged();
                }

                // reactivate notify
                // the iteration over an ObservableCollection triggers the notification
                // of the "IsChecked" property and slows the performance of the loop
                notify = true;
            }
        }

        public bool? IsDateChecked
        {
            get => isDateChecked;
            set
            {
                // raise event to update the date tree
                // see FilterCommon class
                OnIsCheckedDate?.Invoke(this, value);
            }
        }

        // content displayed
        public string Label { get; set; }

        public int Level { get; set; }

        public DataGridFilterItem Parent { get; set; }

        // don't invoke update tree
        public bool? SetDateState
        {
            get => isDateChecked;
            set => isDateChecked = value;
        }
    }
}