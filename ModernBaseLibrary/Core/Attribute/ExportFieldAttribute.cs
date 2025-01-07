/*
 * <copyright file="ExportFieldAttribute.cs" company="Lifeprojects.de">
 *     Class: ExportFieldAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>03.05.2017</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class Attribute for ExportFieldAttribute
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
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ExportFieldAttribute : Attribute
    {
        public ExportFieldAttribute([CallerMemberName] string fieldName = null)
        {
            this.FieldName = fieldName;
            this.DisplayName = fieldName;
            this.SortOrder = 0;
            this.Format = string.Empty;
        }

        public ExportFieldAttribute(string displayName, int sortOrder, [CallerMemberName] string fieldName = null)
        {
            this.FieldName = fieldName;
            this.DisplayName = displayName;
            this.SortOrder = sortOrder;
            this.Format = string.Empty;
        }

        public ExportFieldAttribute(int sortOrder, [CallerMemberName] string fieldName = null)
        {
            this.FieldName = fieldName;
            this.DisplayName = fieldName;
            this.SortOrder = sortOrder;
            this.Format = string.Empty;
        }

        public string FieldName { get; set; }

        public Type PropertyTyp { get; set; }

        public string DisplayName { get; set; }

        public string Format { get; set; }

        public int SortOrder { get; set; }
    }
}
