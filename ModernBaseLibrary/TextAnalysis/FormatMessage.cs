/*
 * <copyright file="FormatMessage.cs" company="Lifeprojects.de">
 *     Class: FormatMessage
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>28.07.2023 17:20:09</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * <example>
 * string msgS = "Es wurden 10 Datensätze gelöscht"
 * string msgP = "Es wurde ein Datensatz gelöscht"
 * string msg = "Es [wurde/wurden] [ein/{0}] [Datensatz/Datensätze] gelöscht"
 * </example>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public static class FormatMessage
    {
        public static string Get(string msg, int args)
        {
            string result = string.Empty;

            List<string> argsSource = msg.ExtractFromString("[","]").ToList();

            if (argsSource != null && argsSource.Count > 0)
            {
                foreach (string item in argsSource)
                {
                    string changeText = string.Empty;
                    if (args == 1)
                    {
                        changeText = item.Split('/')[0];
                    }
                    else
                    {
                        changeText = item.Split('/')[1];
                    }

                    msg = msg.Replace(item, changeText);
                }

                if (args > 1)
                {
                    result = string.Format(msg.Replace("[", string.Empty).Replace("]", string.Empty), args);
                }
                else
                {
                    result = msg.Replace("[", string.Empty).Replace("]", string.Empty);
                }
            }

            return result;
        }
    }
}
