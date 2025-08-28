namespace ModernSQLiteDemo.SQLite
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Text;

    using ModernBaseLibrary.Extension;

    using ModernSQLite.Generator;

    public class ContactRepository : SQLiteDBContext
    {
        private const string TAB_NAME = "TAB_Contact";
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoDataRepository"/> class.
        /// </summary>
        public ContactRepository() : base(Program.DatabasePath)
        {
            this.Tablename = TAB_NAME;
            this.DBConnection = base.Connection;
        }

        public SQLiteConnection DBConnection { get; private set; }

        private string Tablename { get; set; }

        public int Count()
        {
            int result = 0;

            try
            {
                result = base.Connection.RecordSet<int>($"select count(*) from {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }


        public ICollectionView SelectAsICollectionView()
        {
            ICollectionView result = null;

            try
            {
                result = base.Connection.RecordSet<ICollectionView>($"SELECT * FROM {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public DataTable SelectAsDataTable()
        {
            DataTable result = null;

            try
            {
                result = base.Connection.RecordSet<DataTable>($"SELECT * FROM {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public DataRow SelectById(Guid id)
        {
            DataRow result = null;
            string sqlStatement = string.Empty;

            try
            {
                sqlStatement = $"SELECT * FROM {this.Tablename} WHERE Id = '{id.ToString()}'";
                result = base.Connection.RecordSet<DataRow>(sqlStatement).Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }


        public DataRow NewDataRow()
        {
            DataRow result = null;
            string sqlText = string.Empty;

            try
            {
                result = base.Connection.RecordSet<DataRow>(this.Tablename).New().Result;

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }


        public void Add(DataRow entity)
        {
            try
            {
                using (SqlBuilderContext ctx = new SqlBuilderContext(entity))
                {
                    (string, SQLiteParameter[]) sql = ctx.GetInsert();
                    string dumpSql = ctx.GetSqlDump(sql);
                    this.Connection.RecordSet<int>(sql.Item1, sql.Item2).Execute();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Update(DataRow entity)
        {
            try
            {
                using (SqlBuilderContext ctx = new SqlBuilderContext(entity))
                {
                    ctx.CurrentUser = Environment.UserName;
                    (string, SQLiteParameter[]) sql = ctx.GetUpdate();

                    this.Connection.RecordSet<int>(sql.Item1, sql.Item2).Execute();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }


        public int Delete(DataRow entity)
        {
            int result = 0;

            try
            {
                using (SqlBuilderContext ctx = new SqlBuilderContext(entity))
                {
                    ctx.CurrentUser = Environment.UserName;
                    (string, SQLiteParameter[]) sql = ctx.GetDelete();

                    this.Connection.RecordSet<int>(sql.Item1, sql.Item2).Execute();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public int DeleteAll()
        {
            int result = 0;

            try
            {
                result = base.Connection.RecordSet<int>($"DELETE FROM {this.Tablename}").Execute().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }
    }
}
