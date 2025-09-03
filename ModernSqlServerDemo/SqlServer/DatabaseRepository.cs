namespace ModernSqlServerDemo.SqlServer
{
    using Microsoft.Data.SqlClient;

    public class DatabaseRepository : SqlClientDBContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoDataRepository"/> class.
        /// </summary>
        public DatabaseRepository() : base(Program.DatabaseConnection)
        {
            this.DBConnection = base.Connection;
        }

        public SqlConnection DBConnection { get; private set; }

        public void CreateTable()
        {
            if (this.ExistTable("TAB_Contact") == false)
            {
                base.Create(CreateTableInDB);
            }
        }

        private static void CreateTableInDB(SqlConnection sqlConnection)
        {
            List<string> sqlTextBuilder = new List<string>();
            SqlTransaction transaction = sqlConnection.BeginTransaction();

            try
            {
                sqlTextBuilder.Add("DROP TABLE IF EXISTS [TAB_Contact]");
                sqlTextBuilder.Add("CREATE TABLE [TAB_Contact] ([Id] uniqueidentifier,[Vorname] nvarchar(50), [Nachname] nvarchar(50),[Geburtstag] datetime,[Alter] int,[Gehalt] numeric(10,2), [Beschreibung] nvarchar(MAX),[Photo] VARBINARY(MAX), [Active] bit,[CreatedBy] nvarchar(50), [CreatedOn] datetime, [ModifiedBy] nvarchar(50), [ModifiedOn] datetime,[Timestamp] nvarchar(50), PRIMARY KEY (Id))");
                sqlTextBuilder.Add("CREATE INDEX [IDX_TAB_Contact_Nachname] ON [TAB_Contact] ([Nachname])");

                foreach (string sqlStatemanet in sqlTextBuilder)
                {
                    sqlConnection.RecordSet<int>(sqlStatemanet, transaction).Execute();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string errorText = ex.Message;
                throw;
            }
        }
    }
}
