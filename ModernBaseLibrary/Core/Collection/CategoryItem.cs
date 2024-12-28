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
 * Definition of CategoryItem Class for Category
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

    /// <summary>
    /// Beinhaltet die Werte für die Klasse CategoryDictionary
    /// </summary>
    public class CategoryItem : Tuple<string, object, Type>
    {
        public CategoryItem(string item1, object item2, Type item3) : base(item1, item2, item3)
        {
            this.Value = item1;
            this.ValueX = item2;
            this.ValueXType = item3;
        }

        public string Value { get; private set; }

        public object ValueX { get; private set; }

        public Type ValueXType { get; private set; }
    }
}