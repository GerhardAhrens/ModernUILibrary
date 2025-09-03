/*
 * <copyright file="SqlClientDBContext.cs" company="Lifeprojects.de">
 *     Class: SqlClientDBContext
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.09.2025</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
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

namespace Microsoft.Data.SqlClient
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Text;
    using System.Xml;

    using ModernBaseLibrary.Barcode;
    using ModernBaseLibrary.Core;

    public class SqlClientDBContext : IDisposable
    {
        private bool classIsDisposed;

        public SqlClientDBContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlClientDBContext"/> class.
        /// </summary>
        public SqlClientDBContext(string databaseSource)
        {
            this.DatabaseSource = databaseSource;
            this.ConnectString = ConnectStringToText(this.DatabaseSource);
            this.CreateConnection(ConnectString);
        }

        public string ConnectString { get; private set; }

        public SqlConnection Connection { get; private set; }

        public ConnectionState ConnectionState { get; private set; }

        private string DatabaseSource { get; set; }
        private string DatabaseName { get; set; }

        public void Create(Action<SqlConnection> actionMethod)
        {
            try
            {
                using (SqlConnection sqliteConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqliteConnection.State != ConnectionState.Open)
                    {
                        sqliteConnection.Open();
                        this.ConnectionState = sqliteConnection.State;
                    }

                    if (actionMethod != null)
                    {
                        actionMethod?.Invoke(sqliteConnection);
                    }

                    sqliteConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIfColumnExists(string tableName, string columnName)
        {
            try
            {
                using (SqlConnection sqliteConnection = new SqlConnection(this.ConnectString))
                {
                    sqliteConnection.Open();
                    this.ConnectionState = sqliteConnection.State;
                    var cmd = sqliteConnection.CreateCommand();
                    cmd.CommandText = $"select * from information_schema.columns where table_name = '{tableName}' AND columnName = {columnName}";
                    SqlDataReader reader = cmd.ExecuteReader();
                    int nameIndex = reader.GetOrdinal("COLUMN_NAME");
                    while (reader.Read())
                    {
                        if (reader.GetString(nameIndex).Equals(columnName,StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    sqliteConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return false;
        }

        public DataTable AllTableSize()
        {
            DataTable dataTables = null;
            StringBuilder sqlText = new StringBuilder();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(this.ConnectString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                        if (dbConnection.State == ConnectionState.Open)
                        {
                            var cmd = dbConnection.CreateCommand();

                            sqlText.Append("SELECT sys.databases.name,");
                            sqlText.Append("CONVERT(VARCHAR, SUM(size)*8/1024) + ' MB' AS [TableSizeText],");
                            sqlText.Append("(SUM(size)*8/1024) AS [TableSize]").Append(" ");
                            sqlText.Append("FROM sys.databases").Append(" ");
                            sqlText.Append("JOIN sys.master_files").Append(" ");
                            sqlText.Append("ON sys.databases.database_id = sys.master_files.database_id").Append(" ");
                            sqlText.Append("GROUP BY sys.databases.name").Append(" ");
                            sqlText.Append("ORDER BY sys.databases.name;");
                            cmd.CommandText = sqlText.ToString();
                            SqlDataReader reader = cmd.ExecuteReader();
                            dataTables.Load(reader);
                        }
                    }

                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return dataTables;
        }

        /// <summary>
        /// Lesen alle Columns/Tabelle der aktuellen SQLite Datenbank
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetSchema(SqlServerTableSchema schemaTyp = SqlServerTableSchema.Columns)
        {
            DataTable dataTableSchema = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(this.ConnectString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                        if (dbConnection.State == ConnectionState.Open)
                        {
                            dataTableSchema = new DataTable(schemaTyp.ToString());
                            dataTableSchema = dbConnection.GetSchema(schemaTyp.ToString());
                        }
                    }

                    dbConnection.Close();
                }

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return dataTableSchema;
        }

        public DataTable Tables()
        {
            DataTable tables = null;

            try
            {
                using (SqlConnection sqliteConnection = new SqlConnection(this.ConnectString))
                {
                    tables = new DataTable("Schema");

                    if (sqliteConnection.State != ConnectionState.Open)
                    {
                        sqliteConnection.Open();
                        if (sqliteConnection.State == ConnectionState.Open)
                        {
                            this.ConnectionState = sqliteConnection.State;

                            tables = sqliteConnection.GetSchema(SqlServerTableSchema.Columns.ToString());
                            tables.Columns.Remove("TABLE_CATALOG");
                            tables.Columns.Remove("TABLE_SCHEMA");
                            tables.Columns.Remove("IS_SPARSE");
                            tables.Columns.Remove("IS_COLUMN_SET");
                            tables.Columns.Remove("IS_FILESTREAM");
                            tables.Columns.Remove("COLLATION_CATALOG");
                            tables.Columns.Remove("CHARACTER_SET_NAME");
                            tables.Columns.Remove("CHARACTER_MAXIMUM_LENGTH");
                            tables.Columns.Remove("CHARACTER_SET_CATALOG");
                            tables.Columns.Remove("CHARACTER_SET_SCHEMA");
                        }
                    }

                    sqliteConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return tables;
        }

        public List<Tuple<string, string, object, Type>> MetadataInformation()
        {
            long databaseSize = 0;
            DateTime lastAccess = new DateTime(1900, 1, 1);
            List<Tuple<string, string, object, Type>> meta = new List<Tuple<string, string, object, Type>>();
            using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    DataTable dataTables = new DataTable();
                    StringBuilder sqlText = new StringBuilder();

                    sqlConnection.Open();

                    var cmd = sqlConnection.CreateCommand();

                    sqlText.Append("SELECT sys.databases.name,");
                    sqlText.Append("CONVERT(VARCHAR, SUM(size)*8/1024) + ' MB' AS [TableSizeText],");
                    sqlText.Append("(SUM(size)*8/1024) AS [TableSize]").Append(" ");
                    sqlText.Append("FROM sys.databases").Append(" ");
                    sqlText.Append("JOIN sys.master_files").Append(" ");
                    sqlText.Append("ON sys.databases.database_id = sys.master_files.database_id").Append(" ");
                    sqlText.Append("GROUP BY sys.databases.name").Append(" ");
                    sqlText.Append("ORDER BY sys.databases.name;");
                    cmd.CommandText = sqlText.ToString();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dataTables.Load(reader);

                    if (dataTables != null && dataTables.Rows.Count > 0)
                    {
                        DataRow row = dataTables.AsEnumerable().Where(w => w.Field<string>("Name") == this.DatabaseName).FirstOrDefault<DataRow>();
                        databaseSize = row.Field<int>("TableSize");
                    }

                    string cmdText = "select max(obj.modify_date) AS last_modify_date from sys.objects as obj";
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                    {
                        lastAccess = Convert.ToDateTime(sqlCmd.ExecuteScalar());
                    }

                }

                meta.Add(new Tuple<string, string, object, Type>("DataSource", "SqlConnection", sqlConnection.DataSource, typeof(string)));
                meta.Add(new Tuple<string, string, object, Type>("ConnectionTimeout", "SqlConnection", sqlConnection.ConnectionTimeout, typeof(int)));
                meta.Add(new Tuple<string, string, object, Type>("ServerVersion", "SqlConnection", sqlConnection.ServerVersion, typeof(string)));
                meta.Add(new Tuple<string, string, object, Type>("Length", "Database", databaseSize, typeof(long)));
                meta.Add(new Tuple<string, string, object, Type>("LastWriteTime", "Database", lastAccess.ToString("dd.MM.yyyy HH:mm:ss"), typeof(string)));

                sqlConnection.Close();
            }

            return meta;
        }

        public string Version()
        {
            string result = string.Empty;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                        this.ConnectionState = sqlConnection.State;
                        result = sqlConnection.ServerVersion;
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        #region Funktionen zur Datenbank
        public bool ExistDatabase()
        {
            bool result = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();

                        string cmdText = @"IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases where name=@db) SELECT 1 ELSE SELECT 0";
                        using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                        {
                            sqlCmd.Parameters.Add("@db", SqlDbType.NVarChar).Value = this.DatabaseName;
                            int nRet = Convert.ToInt32(sqlCmd.ExecuteScalar());
                            result = (nRet > 0);
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public bool ExistTable(string tableName)
        {
            bool result = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();

                        string cmdText = $"select case when exists((select * from information_schema.tables where table_name = @TableName)) then 1 else 0 end";
                        using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                        {
                            sqlCmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = tableName;
                            int nRet = Convert.ToInt32(sqlCmd.ExecuteScalar());
                            result = (nRet > 0);
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        /// <summary>
        /// Größe der Datenbank in MB
        /// </summary>
        /// <returns>Größe der Datenbank</returns>
        public long Length()
        {
            long dataBaseSize = 0;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(this.ConnectString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        DataTable dataTables = new DataTable();
                        StringBuilder sqlText = new StringBuilder();

                        dbConnection.Open();
                        if (dbConnection.State == ConnectionState.Open)
                        {
                            var cmd = dbConnection.CreateCommand();

                            sqlText.Append("SELECT sys.databases.name,");
                            sqlText.Append("CONVERT(VARCHAR, SUM(size)*8/1024) + ' MB' AS [TableSizeText],");
                            sqlText.Append("(SUM(size)*8/1024) AS [TableSize]").Append(" ");
                            sqlText.Append("FROM sys.databases").Append(" ");
                            sqlText.Append("JOIN sys.master_files").Append(" ");
                            sqlText.Append("ON sys.databases.database_id = sys.master_files.database_id").Append(" ");
                            sqlText.Append("GROUP BY sys.databases.name").Append(" ");
                            sqlText.Append("ORDER BY sys.databases.name;");
                            cmd.CommandText = sqlText.ToString();
                            SqlDataReader reader = cmd.ExecuteReader();
                            dataTables.Load(reader);

                            if (dataTables != null && dataTables.Rows.Count > 0)
                            {
                                DataRow row = dataTables.AsEnumerable().Where(w => w.Field<string>("Name") == this.DatabaseName).FirstOrDefault<DataRow>();
                                dataBaseSize = row.Field<int>("TableSize");
                            }
                        }
                    }

                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return dataBaseSize;
        }

        public DateTime LastWriteTime()
        {
            DateTime result = new DateTime(1900, 1, 1);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();

                        string cmdText = "select max(obj.modify_date) AS last_modify_date from sys.objects as obj";
                        using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                        {
                            result = Convert.ToDateTime(sqlCmd.ExecuteScalar());
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        #endregion Funktionen zur Datenbank

        private void CreateConnection(string connectionString)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                if (conn != null && conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    this.Connection = conn;
                    this.ConnectionState = conn.State;
                    this.DatabaseName = conn.Database;
                }
            }
            catch (SqlException ex)
            {
                string errorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private static string ConnectStringToText(string databasePath,int defaultTimeout = 15)
        {
            try
            {
                SqlConnectionStringBuilder conString = new SqlConnectionStringBuilder(databasePath);
                conString.ConnectTimeout = defaultTimeout;

                return conString.ToString();
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        #region Implement Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing == true)
                {
                    Connection = null;
                }
            }

            this.classIsDisposed = true;
        }

        #endregion Implement Dispose
    }

    public enum SqlServerTableSchema
    {
        None = 0,
        MetaDataCollections = 1,
        DataSourceInformation = 2,
        DataTypes = 3,
        ReservedWords = 4,
        Catalogs = 5,
        Columns = 6,
        Indexes = 7,
        IndexColumns = 8,
        Tables = 9,
        Views = 10,
        ViewColumns = 11,
        ForeignKeys = 12,
        Triggers = 13
    }
}
