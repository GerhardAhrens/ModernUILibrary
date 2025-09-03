/*
 * <copyright file="DataViewAttribute.cs" company="Lifeprojects.de">
 *     Class: DataViewAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>24.02.2021</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class Attribute for DataViewAttribute
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

namespace ModernSqlServer.Generator
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public class DataViewAttribute : Attribute
    {
        public DataViewAttribute([CallerMemberName] string viewName = "")
        {
            if (string.IsNullOrEmpty(viewName) == false)
            {
                ViewName = viewName;
            }
            else
            {
                ViewName = string.Empty;
            }
        }

        public string ViewName
        {
            get;
            set;
        }
    }
}
