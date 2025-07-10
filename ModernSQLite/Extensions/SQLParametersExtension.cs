//-----------------------------------------------------------------------
// <copyright file="SQLParametersExtension.cs" company="Lifeprojects.de">
//     Class: SQLParametersExtension
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.05.2019</date>
//
// <summary>SQLParametersExtension Class for SQL Server Database</summary>
//-----------------------------------------------------------------------

namespace System.Data.SQLite
{
    using System.Collections.Generic;

    public static class SQLParametersExtension
    {
        /// <summary>
        /// Übernimmmt eine Dictionary(string,object)() als SQLiteParameterCollection
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        public static void AddRangeWithValue(this SQLiteParameterCollection @this, Dictionary<string, object> values)
        {
            foreach (var keyValuePair in values)
            {
                @this.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }

            @this.ClearParameters();
        }

        /// <summary>
        /// Setzt alle Null-Werte auf DBNull.Value
        /// </summary>
        /// <param name="this"></param>
        public static void ClearParameters(this SQLiteParameterCollection @this)
        {
            foreach (SQLiteParameter parameter in @this)
            {
                if (parameter.Value == DBNull.Value || parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }

    }
}
