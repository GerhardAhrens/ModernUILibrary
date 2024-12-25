/*
 * <copyright file="IListExtension.cs" company="Lifeprojects.de">
 *     Class: IListExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
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

namespace ModernBaseLibrary.Extension
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class IListExtension
    {
        public static bool IsNullOrEmpty(this IList @this)
        {
            return (@this == null || @this.Count < 1);
        }

        public static List<T> Concate<T>(this IList<T> @this, IList<T> second, bool withOutDuplicates = false)
        {
            @this.IsArgumentNull("First IList<T>");
            second.IsArgumentNull("Second IList<T>");

            if (withOutDuplicates == false)
            {
                return @this.Concat(second).ToList();
            }
            else
            {
                return @this.Union(second).ToList();
            }
        }

        public static T GetItem<T>(this IList<T> @this, int index)
        {
            try
            {
                if (index < @this.Count)
                {
                    T item = @this[index];
                    return item;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                throw new IndexOutOfRangeException($"The index {index} is out of range.");
            }
        }

        public static T GetItemAndRemove<T>(this IList<T> @this, int index)
        {
            try
            {
                if (index < @this.Count)
                {
                    T item = @this[index];
                    @this.Remove(item);
                    return item;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                throw new IndexOutOfRangeException($"The index {index} is out of range.");
            }
        }
    }
}
