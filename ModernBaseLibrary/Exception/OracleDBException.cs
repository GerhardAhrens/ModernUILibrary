/*
 * <copyright file="OracleDBException.cs" company="Lifeprojects.de">
 *     Class: OracleDBException
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>06.07.2023</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Custom Exception Class for Oracle Database
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
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [Serializable]
    public class OracleDBException : BaseException
    {
        private const string MESSAGE = "Es ist ein Fehler beim Zugriff auf eine Oracle Database aufgetreten.";

        public OracleDBException() : base(string.Empty)
        {
            this.Data.Add("Msg", MESSAGE);
            this.Data.Add("OracleDB", "Unbekannte");
            this.ErrorLevel = ErrorLevel.Error;
        }

        public OracleDBException(string pMessage) : base(pMessage)
        {
            this.ErrorMessage = pMessage;
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public OracleDBException(string pMessage, string oracleHost) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.Data.Add("OracleDB", oracleHost);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public OracleDBException(string pMessage, Exception pInnerException) : base(pMessage, pInnerException)
        {
            this.ErrorMessage = pMessage;
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public string ErrorMessage { get; private set; }

        public string OraError()
        {
            string oraError = "ORA99999";
            if (string.IsNullOrEmpty(this.ErrorMessage) == false && this.ErrorMessage.Contains(":"))
            {
                oraError = this.ErrorMessage.Split(':')[0];
            }

            return oraError;
        }

        public string OraMessage()
        {
            string oraMessage = "ORA-Errortext";
            if (string.IsNullOrEmpty(this.ErrorMessage) == false && this.ErrorMessage.Contains(":"))
            {
                oraMessage = this.ErrorMessage.Split(':')[1];
            }

            return oraMessage;
        }

        public override IDictionary CustomMessage()
        {
            if (this.Data.IsNullOrEmpty() == true)
            {
                this.Data.Add("Msg", MESSAGE);
                this.Data.Add("OracleDB", "Unbekannte");

                return this.Data;
            }
            else
            {
                return this.Data;
            }
        }
    }
}