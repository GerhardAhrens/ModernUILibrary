/*
 * <copyright file="IDictionaryExtension.cs" company="Lifeprojects.de">
 *     Class: IDictionaryExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für IDictionary
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

namespace ModernConsole.Extension
{
    using System.Collections;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    internal static class IDictionaryExtension
    {
        public static bool IsNullOrEmpty(this IDictionary @this)
        {
            return (@this == null || @this.Count < 1);
        }
    }
}
