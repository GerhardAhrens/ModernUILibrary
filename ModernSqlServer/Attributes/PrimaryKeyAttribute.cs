//-----------------------------------------------------------------------
// <copyright file="PrimaryKeyAttribute.cs" company="Lifeprojects.de">
//     Class: PrimaryKeyAttribute
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>PrimaryKeyAttribute for Property</summary>
//-----------------------------------------------------------------------

namespace ModernSqlServer.Generator
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute([CallerMemberName] string columnName = "")
        {
            this.ColumnName = columnName;
        }

        public string ColumnName { get; set; }
    }
}