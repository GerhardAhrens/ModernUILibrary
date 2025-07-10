/*
 * <copyright file="WMIException.cs" company="Lifeprojects.de">
 *     Class: WMIException
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Custom Exception Class for WMI Query
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

namespace ModernConsole.Exception
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    using ModernConsole.Extension;

    [SupportedOSPlatform("windows")]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [Serializable]
    public class ReadOnlyCollectionException : BaseException
    {
        private const string MESSAGE = "Die Collection ist als ReadOnly festgelegt";

        public ReadOnlyCollectionException() : base(string.Empty)
        {
            this.Data.Add("Msg", MESSAGE);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public ReadOnlyCollectionException(string pMessage) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public ReadOnlyCollectionException(string pMessage, string query, Exception pInnerException) : base(pMessage, pInnerException)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public override IDictionary CustomMessage()
        {
            if (this.Data.IsNullOrEmpty() == true)
            {
                this.Data.Add("Msg", MESSAGE);

                return this.Data;
            }
            else
            {
                return this.Data;
            }
        }
    }
}