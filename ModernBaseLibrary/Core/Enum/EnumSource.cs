/*
 * <copyright file="EnumSource.cs" company="Lifeprojects.de">
 *     Class: EnumSource
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>28.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Datasource class from Enum
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

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Die Klasse stellt Methoden zur Verfügung um ein Enum als Datasource verwenden.
    /// </summary>
    /// <example>
    /// ComboBoxCtrl.DataSource = EnumHelper.DataSourceFromEnum(typeof(EntladungsTyp));
    /// ComboBoxCtrl.DisplayMember = "Value";
    /// ComboBoxCtrl.ValueMember = "Key";
    /// </example>
    [Serializable]
    [DebuggerDisplay("Key={Key};Value={Value]")]
    public class EnumSource
    {
        public int Key { get; set; }

        public string Value { get; set; }

        public static IEnumerable<ValueListItem> EnumToIEnumerable(Type type)
        {
            if (type.IsEnum == false)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "source");
            }

            var list = new List<ValueListItem>();

            Array arrayEnum = Enum.GetValues(type);
            foreach (var array in arrayEnum)
            {
                var vi = new ValueListItem { Key = (int)array, Value = array.ToString() };
                list.Add(vi);
            }

            return list;
        }

        public static List<EnumSource> EnumToList(Type type)
        {
            if (type.IsEnum == false)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "source");
            }

            var list = new List<EnumSource>();

            Array arrayEnum = Enum.GetValues(type);
            foreach (var array in arrayEnum)
            {
                var vi = new EnumSource { Key = (int)array, Value = array.ToString() };
                list.Add(vi);
            }

            return list;
        }
    }
}