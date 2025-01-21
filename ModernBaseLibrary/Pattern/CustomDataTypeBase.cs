/*
 * <copyright file="CustomDataTypeBase.cs" company="Lifeprojects.de">
 *     Class: CustomDataTypeBase
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>16.06.2023 09:36:46</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Abstract Class Implementation of the CustomDataTypeBase
 * </summary>
 *
 * <WebLink>
 * </WebLink>
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class CustomDataTypeBase
    {
        protected abstract IEnumerable<object> GetValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (CustomDataTypeBase)obj;

            return this.GetValues().SequenceEqual(other.GetValues());
        }
        public override int GetHashCode()
        {
            return GetValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        protected static bool EqualOperator(CustomDataTypeBase left, CustomDataTypeBase right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(CustomDataTypeBase left, CustomDataTypeBase right)
        {
            return !(EqualOperator(left, right));
        }

        public static bool operator ==(CustomDataTypeBase left, CustomDataTypeBase right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(CustomDataTypeBase left, CustomDataTypeBase right)
        {
            return NotEqualOperator(left, right);
        }
    }
}
