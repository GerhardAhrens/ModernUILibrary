//-----------------------------------------------------------------------
// <copyright file="TableColumnAttribute.cs" company="Lifeprojects.de">
//     Class: TableColumnAttribute
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>Class for TableColumnAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TableColumnAttribute : Attribute
    {
        public TableColumnAttribute(SQLiteDataType columnTyp, int length, int afterComma, [CallerMemberName] string columnName = "")
        {
            this.ColumnName = columnName;
            this.ColumnType = columnTyp;
            this.Length = length;
            this.AfterComma = afterComma;
        }

        public TableColumnAttribute(SQLiteDataType columnTyp, int length, [CallerMemberName] string columnName = "")
        {
            this.ColumnName = columnName;
            this.ColumnType = columnTyp;
            this.Length = length;
            this.AfterComma = 0;
        }

        public TableColumnAttribute(SQLiteDataType columnTyp, [CallerMemberName] string columnName = "")
        {
            this.ColumnName = columnName;
            this.ColumnType = columnTyp;
            if (columnTyp == SQLiteDataType.VarChar)
            {
                this.Length = 50;
            }

            this.AfterComma = 0;
        }

        public TableColumnAttribute([CallerMemberName] string columnName = "")
        {
            this.ColumnName = columnName;
            this.ColumnType = SQLiteDataType.Text;
            this.Length = 0;
        }

        public string ColumnName { get; set; }

        public int Length { get; set; }

        public int AfterComma { get; set; }

        public SQLiteDataType ColumnType { get; set; }
    }
}