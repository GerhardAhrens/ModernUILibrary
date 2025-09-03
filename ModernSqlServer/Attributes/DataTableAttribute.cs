//-----------------------------------------------------------------------
// <copyright file="DataTableAttribute.cs" company="Lifeprojects.de">
//     Class: DataTableAttribute
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>04.11.2017</date>
//
// <summary>Class for DataTableAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernSqlServer.Generator
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DataTableAttribute : Attribute
    {
        public DataTableAttribute([CallerMemberName] string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName) == false)
            {
                TableName = tableName;
            }
            else
            {
                TableName = string.Empty;
            }
        }

        public string TableName
        {
            get;
            set;
        }
    }
}
