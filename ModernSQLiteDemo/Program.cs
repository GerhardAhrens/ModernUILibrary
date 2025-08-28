
namespace ModernSQLiteDemo
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
        private const string DATABASENAME = "DemoDaten.db";
        public static List<ConsoleMenuOption> menuOptions = null;

        private static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static void Main(string[] args)
        {
            DatabasePath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "DemoDaten", DATABASENAME);
            BlobPath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "DemoBLOB");

            SmartMenu.Menu("SQLite Library Demo")
               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Datenbank Connection / Create")
               .Add("Connection / Create", () => { new FuncConnection().CreateDatabase(Program.DatabasePath); }, 1)
               .Add("Datenbank Information", () => { new FuncConnection().DatabaseInfo(Program.DatabasePath); }, 1)
               )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Insert")
               .Add("Text und Zahlen", () => { new FuncInsert().InsertValues(Program.DatabasePath); }, 1)
               .Add("byte[] Array (z.B. Bilder, PDF, usw.)", () => { new FuncInsert().InsertByteArray(Program.DatabasePath, Program.BlobPath); }, 1)
               )

               /* Mit Untermenü */
               .Add(SmartMenu.Menu("Select")
               .Add("CommandLine - Help", () => { new MenuExecute().FuncEmpty(); }, 1)
               )

               /* Ohne Untermenü */
               .Add("Test", () => { new MenuExecute().FuncEmpty(); })
               .Show();
        }

        public static string DatabasePath { get; set; }
        public static string BlobPath { get; set; }
    }
}
