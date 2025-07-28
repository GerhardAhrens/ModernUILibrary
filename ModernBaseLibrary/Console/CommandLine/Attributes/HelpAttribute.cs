/*
 * <copyright file="HelpAttribute.cs" company="Lifeprojects.de">
 *     Class: HelpAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Help Attribute Klasse
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

namespace System.Cmdl
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class HelpAttribute : Attribute
    {
        private readonly string usageText;
        
        public HelpAttribute(string usageText)
        {
            this.usageText = usageText;
        }

        public string Usage
        {
            get { return usageText; }
        }
    }
}
