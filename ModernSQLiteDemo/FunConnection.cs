namespace ModernSQLiteDemo
{
    using System;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    using ModernConsole.Message;

    using ModernSQLiteDemo.SQLite;

    [SupportedOSPlatform("windows")]
    public class FunConnection
    {

        public static string DatabasePath { get; set; }

        public void DatabaseConext(string databasePath)
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
    }
}