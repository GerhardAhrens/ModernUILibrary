/*
 * <copyright file="RequirementAttribute.cs" company="Lifeprojects.de">
 *     Class: RequirementAttribute
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class for RequirementAttribute
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
    using System.Linq;

    [DebuggerDisplay("Id={Id};Comment={Comment};Status={Status}")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate
        | AttributeTargets.Enum | AttributeTargets.Event |
        AttributeTargets.Interface | AttributeTargets.Method |
        AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Struct)]
    public class RequirementAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the requirement attribute
        /// </summary>
        /// <param name="id">The requirement id</param>
        public RequirementAttribute(string id)
        {
            this.Id = id;
            this.DependsOnRequirementId = string.Empty;
            this.Comment = string.Empty;
            this.Status = RequirementStatus.InWork;
            this.TestId = string.Empty;
        }

        public RequirementAttribute(string id, string comment, RequirementStatus status = RequirementStatus.InWork)
        {
            this.Id = id;
            this.DependsOnRequirementId = string.Empty;
            this.Comment = comment;
            this.Status = status;
            this.TestId = string.Empty;
        }

        /// <summary>
        /// Gets the requirement id
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the dependent requirement id
        /// </summary>
        public string DependsOnRequirementId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the developer comment
        /// </summary>
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public RequirementStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the test id.
        /// </summary>
        public string TestId
        {
            get;
            set;
        }

        public string MemberName
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{this.Id};{this.Status};{this.MemberName};{this.Comment}";
        }

        public string ToSearchFilter(bool isUpper = true)
        {
            string result = string.Empty;

            IEnumerable<object> propertyContent = from property in this.GetType().GetProperties()
                                                  select property.GetValue(this, null);
            if (propertyContent != null)
            {
                result = string.Join("|", propertyContent);
            }

            return isUpper == true ? result.ToUpper() : result;
        }

    }
}