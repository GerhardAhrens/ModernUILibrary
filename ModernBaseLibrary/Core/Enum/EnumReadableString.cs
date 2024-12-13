/*
 * <copyright file="EnumReadableString.cs" company="Lifeprojects.de">
 *     Class: EnumReadableString
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class for ReadableStringAttribute
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
    using System.Reflection;

    public class EnumReadableString
    {

        public static T Parse<T>(string str, bool ignoreCase = false) where T : struct 
        {
            if (typeof(T).IsEnum == false)
            {
                throw new ArgumentException("T must be an enum", "T");
            }

            FieldInfo[] values = typeof(T).GetFields() ?? new FieldInfo[0];

            foreach (FieldInfo value in values)
            {
                object[] attributes = value.GetCustomAttributes(typeof(ReadableStringAttribute), inherit: false) ?? new object[0];
                foreach (object attr in attributes)
                {
                    string readableStr = ((ReadableStringAttribute)attr).ReadableString;
                    if (string.Compare(str, readableStr, ignoreCase) == 0)
                    {
                        return (T)value.GetValue(null);
                    }
                }
            }

            T parsedValue = default(T);
            bool matchFound = Enum.TryParse<T>(str, ignoreCase, out parsedValue);
            if (matchFound)
            {
                return parsedValue;
            }
            else
            {
                throw new ArgumentException("Unknown value " + str, "str");
            }
        }
    }
}
