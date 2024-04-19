/*
 * <copyright file="TabControlItem.cs" company="Lifeprojects.de">
 *     Class: TabControlItem
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>19.04.2024 18:32:45</date>
 * <Project>CurrentProject</Project>
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

namespace ModernUIDemo.Model
{
    using System.Windows.Controls;

    public class TabControlItem
    {
        private static int index = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlItem"/> class.
        /// </summary>
        public TabControlItem(string header, bool isGroupItem = false)
        {
            index++;
            this.TabItemIndex = index;
            this.ItemHeader = header;
            this.IsGroupItem = isGroupItem;
        }

        public TabControlItem(string header, UserControl tabItem, bool isGroupItem = false)
        {
            index++;
            this.TabItemIndex = index;
            this.ItemHeader = header;
            this.IsGroupItem = isGroupItem;
            this.ItemContent = tabItem;
        }

        public int TabItemIndex { get; private set; }
        public bool IsGroupItem { get; private set; }
        public string ItemHeader { get; private set; }
        public UserControl ItemContent { get; private set; }
    }
}
