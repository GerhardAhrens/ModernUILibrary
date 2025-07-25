﻿/*
 * <copyright file="ExecuteMethodeHandlerAttribute.cs" company="Lifeprojects.de">
 *     Class: ExecuteMethodeHandlerAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>29.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class Attribute for ExecuteMethodeHandlerAttribute
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

namespace ModernConsole.Pattern
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExecuteMethodeHandlerAttribute : Attribute
    {
        public ExecuteMethodeHandlerAttribute(string handlerValue, string descriptionValue = "")
        {
            this.CommandHandler = handlerValue;
            this.Description = descriptionValue;
        }

        public string CommandHandler { get; set; }

        public string Description { get; set; }
    }
}
