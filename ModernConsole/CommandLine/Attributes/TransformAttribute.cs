/*
 * <copyright file="TransformAttribute.cs" company="Lifeprojects.de">
 *     Class: TransformAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Transform Attribute Klasse
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
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class TransformAttribute : Attribute
    {
        private TransformDelegate delegateInstance;
        private Type targetType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType">Parent Type of delegate function</param>
        /// <param name="delegateName">Name of delegate function</param>
        public TransformAttribute(Type targetType, string delegateName)
        {
            this.targetType = targetType;

            this.delegateInstance = (string data) => targetType.GetMethod(delegateName).Invoke(System.Activator.CreateInstance(targetType), new[] { data });
        }

        public Delegate Execute
        {
            get
            {
                return this.delegateInstance;
            }
        }

        public Type TargetType
        {
            get
            {
                return this.targetType;
            }
        }
    }
}
