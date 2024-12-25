/*
 * <copyright file="ValueListItem.cs" company="Lifeprojects.de">
 *     Class: ValueListItem
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>28.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * ValueListItem class for EnumHelper
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
    using System.Diagnostics;

    [DebuggerDisplay("Key={Key};Value={Value]")]
    [Serializable]
    public class ValueListItem
    {
        public int Key { get; set; }

        public string Value { get; set; }
    }
}
