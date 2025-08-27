
namespace ModernSQLiteDemo.SQLite
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Text;

    public class DatabaseRepository : SQLiteDBContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoDataRepository"/> class.
        /// </summary>
        public DatabaseRepository() : base(Program.DatabasePath)
        {
            this.DBConnection = base.Connection;
        }

        public SQLiteConnection DBConnection { get; private set; }

        public void CreateTable()
        {
            if (this.Exist() == false)
            {
                base.Create(CreateTableInDB);
            }
        }

        private static void CreateTableInDB(SQLiteConnection sqliteConnection)
        {
            List<string> sqlTextBuilder = new List<string>();
            SQLiteTransaction transaction = sqliteConnection.BeginTransaction();

            try
            {
                sqlTextBuilder.Add("DROP TABLE IF EXISTS [TAB_Contact]");
                sqlTextBuilder.Add("CREATE TABLE IF NOT EXISTS [TAB_Contact] ([Id] VARCHAR(36), [ShortName] VARCHAR(50),[FullName] VARCHAR(250),[EMail] VARCHAR(50),[Description] VARCHAR(250),[Photo] blob, [Active] bool,[CreatedBy] nvarchar(50), [CreatedOn] datetime, [ModifiedBy] nvarchar(50), [ModifiedOn] datetime,[Timestamp] text, PRIMARY KEY (Id))");
                sqlTextBuilder.Add("CREATE INDEX [IDX_TAB_Contact_ShortName] ON [TAB_Contact] ([ShortName])");

                foreach (string sqlStatemanet in sqlTextBuilder)
                {
                    sqliteConnection.RecordSet<int>(sqlStatemanet, transaction).Execute();
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
