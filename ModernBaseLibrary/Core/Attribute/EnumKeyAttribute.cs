/*
 * <copyright file="EnumKeyAttribute.cs" company="Lifeprojects.de">
 *     Class: EnumKeyAttribute
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.02.2023 19:59:34</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Attribute für Enum um weitere Festlegungen zu einem Enum-Item machen zu können
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

namespace System
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnumKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumKeyAttribute"/> class.
        /// </summary>
        public EnumKeyAttribute(string guid)
        {
            this.Guid = new Guid(guid);
        }

        public EnumKeyAttribute(string key, string description)
        {
            this.EnumKey = key;
            this.Description = description;
        }

        public Guid Guid { get; private set; } = Guid.Empty;

        public string EnumKey { get; private set; }

        public string Description { get; private set; }
    }
}
