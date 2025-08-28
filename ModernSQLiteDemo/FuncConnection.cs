namespace ModernSQLiteDemo
{
    using System;
    using System.Data;
    using System.Runtime.Versioning;
    using System.Windows.Documents;

    using ModernBaseLibrary.Extension;

    using ModernConsole.Message;

    using ModernSQLiteDemo.SQLite;

    [SupportedOSPlatform("windows")]
    public class FuncConnection
    {

        public static string DatabasePath { get; set; }

        public void CreateDatabase(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true)
            {
                if (File.Exists(databasePath) == false)
                {
                    using (DatabaseRepository repository = new DatabaseRepository())
                    {
                        repository.CreateTable();
                    }
                }
            }
            else
            {
                MConsole.Alert($"Die Datenbankdatei '{databasePath}' ist nicht vorhanden.","Keine Datei");
            }

            if (File.Exists(databasePath) == true)
            {
                MConsole.Info($"Die Datenbankdatei '{databasePath}' ist vorhanden.", "Datenbankdatei");

                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    string version = repository.Version();
                    long size = repository.Length();
                    MConsole.WriteInfoLine($"Version : {version}");
                    MConsole.WriteInfoLine($"Größe : {size.ToByteText()}");
                }
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void DatabaseInfo(string databasePath)
        {
            if (File.Exists(databasePath) == true)
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    string version = repository.Version();
                    long size = repository.Length();
                    MConsole.WriteInfoLine($"Version : {version}");
                    MConsole.WriteInfoLine($"Größe : {size.ToByteText()}");
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Datenbanktabellen");
                    DataTable tabellen = repository.Tables();
                    DataView view = new DataView(tabellen);
                    DataTable distinctValues = view.ToTable(true, "TABLE_NAME");

                    foreach (DataRow item in distinctValues.Rows)
                    {
                        if (item.GetAs<string>("TABLE_NAME").StartsWith("TAB_") == true)
                        {
                            MConsole.WriteInfoLine(item.GetAs<string>("TABLE_NAME"));
                        }
                    }

                    MConsole.WriteLine();
                }
            }
            else
            {
                MConsole.Alert($"Die Datenbankdatei '{databasePath}' ist nicht vorhanden.", "Keine Datei");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}