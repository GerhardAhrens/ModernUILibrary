/*
 * <copyright file="NavigationListExtension.cs" company="Lifeprojects.de">
 *     Class: NavigationListExtension
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>03.02.2023</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für NavigationList<TContent>
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public static class NavigationCollectionExtension
    {
        public static bool IsNullOrEmpty<TContent>(this NavigationCollection<TContent> @this)
        {
            return @this == null || @this.Any() == false;
        }

        public static bool IsNotNullOrEmpty<T>(this NavigationCollection<T> @this)
        {
            return @this != null && @this.Any() == true;
        }

        public static NavigationCollection<T> ToNavigationCollection<T>(this IEnumerable<T> @this)
        {
            NavigationCollection<T> newList = new NavigationCollection<T>();
            newList.Source = @this;
            if (@this.IsNullOrEmpty() == false)
            {
                foreach (T item in @this)
                {
                    newList.Add(item);
                }
            }

            return newList;
        }

    }
}
