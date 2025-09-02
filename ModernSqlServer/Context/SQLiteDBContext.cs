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
    using System.IO;

    public class SqlClientDBContext : IDisposable
    {
        private bool classIsDisposed;

        public SqlClientDBContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlClientDBContext"/> class.
        /// </summary>
        public SqlClientDBContext(string databaseName)
        {
            if (File.Exists(databaseName) == true)
            {
                this.DatabaseFullName = databaseName;
                this.ConnectString = ConnectStringToText(this.DatabaseFullName);
                this.CreateConnection(ConnectString);
            }
            else
            {
                this.DatabaseFullName = databaseName;
                this.ConnectString = ConnectStringToText(this.DatabaseFullName);
            }
        }

        public string ConnectString { get; private set; }

        public SqlConnection Connection { get; private set; }

        public ConnectionState ConnectionState { get; private set; }

        private string DatabaseFullName { get; set; }

        public void Create(Action<SqlConnection> actionMethod)
        {
            try
            {
                if (File.Exists(this.DatabaseFullName) == false)
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
                else
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
                    cmd.CommandText = $"PRAGMA table_info({tableName})";

                    SqlDataReader reader = cmd.ExecuteReader();
                    int nameIndex = reader.GetOrdinal("Name");
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
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        /// <summary>
        /// Lesen alle Columns/Tabelle der aktuellen SQLite Datenbank
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetSchema(SQLiteTableSchema schemaTyp = SQLiteTableSchema.Columns)
        {
            DataTable dataTableSchema = null;

            try
            {
                using (SqlConnection sqliteConnection = new SqlConnection(this.ConnectString))
                {
                    if (sqliteConnection.State != ConnectionState.Open)
                    {
                        sqliteConnection.Open();
                        if (sqliteConnection.State == ConnectionState.Open)
                        {
                            dataTableSchema = new DataTable(schemaTyp.ToString());
                            dataTableSchema = sqliteConnection.GetSchema(schemaTyp.ToString());
                        }
                    }

                    sqliteConnection.Close();
                }

            }
            catch (Exception)
            {
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

                            tables = sqliteConnection.GetSchema(SQLiteTableSchema.Columns.ToString());
                            tables.Columns.Remove("TABLE_CATALOG");
                            tables.Columns.Remove("TABLE_SCHEMA");
                            tables.Columns.Remove("COLUMN_GUID");
                            tables.Columns.Remove("COLUMN_PROPID");
                            tables.Columns.Remove("COLUMN_HASDEFAULT");
                            tables.Columns.Remove("COLUMN_DEFAULT");
                            tables.Columns.Remove("COLUMN_FLAGS");
                            tables.Columns.Remove("TYPE_GUID");
                            tables.Columns.Remove("CHARACTER_MAXIMUM_LENGTH");
                            tables.Columns.Remove("CHARACTER_SET_CATALOG");
                            tables.Columns.Remove("CHARACTER_SET_SCHEMA");
                            tables.Columns.Remove("CHARACTER_SET_NAME");
                            tables.Columns.Remove("COLLATION_CATALOG");
                            tables.Columns.Remove("COLLATION_NAME");
                            tables.Columns.Remove("DOMAIN_CATALOG");
                            tables.Columns.Remove("DOMAIN_NAME");
                        }
                    }

                    sqliteConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return tables;
        }

        public List<Tuple<string, string, object, Type>> MetadataInformation()
        {
            List<Tuple<string, string, object, Type>> meta = new List<Tuple<string, string, object, Type>>();
            using (SqlConnection sqlConnection = new SqlConnection(this.ConnectString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand("vacuum", sqlConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                meta.Add(new Tuple<string, string, object, Type>("DataSource", "SqlConnection", sqlConnection.DataSource, typeof(string)));
                meta.Add(new Tuple<string, string, object, Type>("ConnectionTimeout", "SqlConnection", sqlConnection.ConnectionTimeout, typeof(int)));
                meta.Add(new Tuple<string, string, object, Type>("ServerVersion", "SqlConnection", sqlConnection.ServerVersion, typeof(string)));

                sqlConnection.Close();
            }

            return meta;
        }

        public string Version()
        {
            string result = string.Empty;

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

            return result;
        }

        #region Funktionen zur Datenbank
        public bool Exist()
        {
            if (File.Exists(this.DatabaseFullName) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long Length()
        {
            long result = 0;

            try
            {
                if (File.Exists(this.DatabaseFullName) == true)
                {
                    FileInfo fi = new FileInfo(this.DatabaseFullName);
                    result = fi.Length;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public DateTime LastWriteTime()
        {
            DateTime result = new DateTime(1900, 1, 1);

            if (File.Exists(this.DatabaseFullName) == true)
            {
                FileInfo fi = new FileInfo(this.DatabaseFullName);
                result = fi.LastWriteTime;
            }

            return result;
        }

        public DateTime CreationTime()
        {
            DateTime result = new DateTime(1900, 1, 1);

            if (File.Exists(this.DatabaseFullName) == true)
            {
                FileInfo fi = new FileInfo(this.DatabaseFullName);
                result = fi.CreationTime;
            }

            return result;
        }
        #endregion Funktionen zur Datenbank

        #region Datenbanksicherung

        public void Backup(string targetBackup = "")
        {
            try
            {
                if (File.Exists(this.DatabaseFullName) == true)
                {
                    FileInfo fi = new FileInfo(this.DatabaseFullName);
                    if (string.IsNullOrEmpty(targetBackup) == true)
                    {
                        targetBackup = $"{Path.GetDirectoryName(this.DatabaseFullName)}\\{Path.GetFileNameWithoutExtension(this.DatabaseFullName)}_{DateTime.Now.ToString("yyyyMMdd")}{Path.GetExtension(this.DatabaseFullName)}";
                    }

                    var result = this.CopyFileAsync(this.DatabaseFullName, targetBackup);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using (Stream source = File.Open(sourcePath, FileMode.Open))
            {
                using (Stream destination = File.Create(destinationPath))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }

        #endregion Datenbanksicherung

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
            SqlConnectionStringBuilder conString = new SqlConnectionStringBuilder(databasePath);
            conString.ConnectTimeout = defaultTimeout;

            return conString.ToString();
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

    public enum SQLiteTableSchema
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
