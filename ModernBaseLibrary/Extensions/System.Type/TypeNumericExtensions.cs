/*
 * <copyright file="TypeExtensions.cs" company="Lifeprojects.de">
 *     Class: TypeExtensions
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>01.08.2025</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Data Types
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
    using System.Numerics;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static partial class TypeExtensions
    {
        public static readonly HashSet<Type> NumericTypes = new HashSet<Type>()
        {
            typeof(byte), typeof(sbyte),
            typeof(int), typeof(uint),
            typeof(UInt16), typeof(UInt32), typeof(UInt64),
            typeof(Int16), typeof(Int32), typeof(Int64),
            typeof(long), typeof(ulong),
            typeof(short), typeof(ushort),
            typeof(float),
            typeof(Single),
            typeof(double),
            typeof(Double),
            typeof(decimal),
            typeof(Decimal),
            typeof(BigInteger), typeof(Complex)
        };

        /// <summary>
        /// Prüft ob ein Typ nummerisch ist
        /// </summary>
        public static bool IsNumeric(this Type type)
        {
            // Use the underlying type for nullable numeric types
            while (!NumericTypes.Contains(type) && Type.GetTypeCode(type) == TypeCode.Object && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            return NumericTypes.Contains(type);
        }

        /// <summary>
        /// Prüft ob ein Objekt nummerisch ist
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>True if the object's type is numeric</returns>
        public static bool IsNumericType(this object obj) => obj != null && obj.GetType().IsNumeric();
    }
}