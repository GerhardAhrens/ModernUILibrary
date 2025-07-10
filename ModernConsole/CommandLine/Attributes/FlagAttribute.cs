/*
 * <copyright file="FlagAttribute.cs" company="Lifeprojects.de">
 *     Class: FlagAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Flag Attribute Klasse
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

namespace ModernConsole.CommandLine
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class FlagAttribute : Attribute
    {
        private readonly string shortName;
        private readonly string name;
        public readonly bool required;
        
        public FlagAttribute(string name = null, string shortName = null, bool required = false)
        {
            this.name = name;
            this.required = required;
            this.shortName = shortName;

            if (!string.IsNullOrWhiteSpace(shortName) && shortName.Length > 1)
            {
                throw new CmdLineLengthException(1);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                this.name = this.name.ToLower();
            }
        }

        public string Name
        {
            get { return name; }
        }

        public string ShortName
        {
            get { return shortName; }
        }

        public bool Required
        {
            get { return required; }
        }
    }
}
