namespace ModernSqlServerDemo
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Versioning;

    using ModernConsole.Menu;
    using ModernConsole.Message;

    [SupportedOSPlatform("windows")]
    internal class Program
    {
        private const string DATABASENAME = "Zollabwicklung";
        private const string DATABASESERVER = "entnmsql-01";
        public static List<ConsoleMenuOption> menuOptions = null;

        private static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static void Main(string[] args)
        {
            DatabaseConnection = $"Data Source={DATABASESERVER};Initial Catalog={DATABASENAME};Integrated Security=true; TrustServerCertificate=true;";
            BlobPath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "DemoBLOB");

            SmartMenu.Menu("Sql Server Library Demo")
                .Add(SmartMenu.Menu("Datenbank Connection / Tabelle / Columns")
                .Add("Check Connection", () => { new FuncConnection().CreateConnection(Program.DatabaseConnection); }, 1)
                .Add("Create Table", () => { new FuncConnection().CreateTable(Program.DatabaseConnection); }, 1)
                .Add("Check of Exist Database", () => { new FuncConnection().ExistDatabase(Program.DatabaseConnection); }, 1)
                .Add("Database information", () => { new FuncConnection().DatabaseInfo(Program.DatabaseConnection); }, 1)
               .Add("Datenbank Metadaten", () => { new FuncConnection().DatabaseMetaInfo(Program.DatabaseConnection); }, 1)
                )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Insert")
               .Add("Text und Zahlen", () => { new FuncInsert().InsertValues(Program.DatabaseConnection); }, 1)
               .Add("byte[] Array (z.B. Bilder, PDF, usw.)", () => { new FuncInsert().InsertByteArray(Program.DatabaseConnection, Program.BlobPath); }, 1)
               )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Select (Lesen von Daten)")
               .Add("Select als ICollectionView", () => { new FuncSelect().SelectAsICollectionView(Program.DatabaseConnection); }, 1)
               .Add("Select als DataTable", () => { new FuncSelect().SelectAsDataTable(Program.DatabaseConnection); }, 1)
               .Add("Select als Dictionary", () => { new FuncSelect().SelectAsDictionary(Program.DatabaseConnection); }, 1)
               .Add("Select als Scalar (einzelne Column, z.B Count)", () => { new FuncSelect().SelectAsScalarCount(Program.DatabaseConnection); }, 1)
               .Add("Select als Scalar (einzelne Column, z.B Sum)", () => { new FuncSelect().SelectAsScalarSum(Program.DatabaseConnection); }, 1)
               .Add("Select DataRow by Id", () => { new FuncSelect().SelectById(Program.DatabaseConnection); }, 1)
               )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Update")
               .Add("Update Column", () => { new MenuExecute().FuncEmpty(); }, 1)
               )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Delete")
               .Add("Delete DataRow", () => { new MenuExecute().FuncEmpty(); }, 1)
               )

               /* Ohne Untermenü */
               .Add("Test", () => { new MenuExecute().FuncEmpty(); })
               .Show();
        }

        public static string DatabaseConnection { get; set; }
        public static string BlobPath { get; set; }
    }
}