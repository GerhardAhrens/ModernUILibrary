//-----------------------------------------------------------------------
// <copyright file="SqlLiteral.cs" company="Lifeprojects.de">
//     Class: SqlLiteral
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SqlLiteral Class
//   http://www.sqlite.org/lang.html
//  </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public class SqlLiteral
    {
        public SqlLiteral(string value)
        {
            this.Value = value;
        }

        public SqlLiteral(SQLiteFunc sqliteFunction)
        {
            if (sqliteFunction == SQLiteFunc.Date)
            {
                this.Value = "Date('NOW')";
            }
            else
            {
                this.Value = string.Empty;
            }
        }

        public string Value { get; set; }

        public string StatementRowsAffected
        {
            get { return "SELECT @@ROWCOUNT"; }
        }

        public string GetDate
        {
            get { return "Date('NOW')"; }
        }
    }
}