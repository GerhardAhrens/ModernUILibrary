/*
 * <copyright file="EnumeratedTypeExtensions.cs" company="Lifeprojects.de">
 *     Class: EnumeratedTypeExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Collection Types
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
    public static class EnumeratedTypeExtensions
    {
        /// <summary>
        /// Die Methode gibt den generischen Typ einer Liste zurück
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetEnumeratedType(this Type type)
        {
            Type elType = type.GetElementType();
            if (null != elType)
            {
                return elType;
            }

            Type[] elTypes = type.GetGenericArguments();
            if (elTypes.Length > 0)
            {
                return elTypes[0];
            }

            return null;
        }
    }
}
