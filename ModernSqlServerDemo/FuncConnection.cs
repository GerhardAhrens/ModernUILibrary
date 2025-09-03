namespace ModernSqlServerDemo
{
    using System;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Runtime.Versioning;
    using System.Windows.Documents;

    using ModernBaseLibrary.Extension;

    using ModernConsole.Message;

    using ModernSqlServerDemo.SqlServer;

    [SupportedOSPlatform("windows")]
    public class FuncConnection
    {

        public static string DatabasePath { get; set; }

        public void CreateConnection(string databasePath)
        {
            MConsole.ClearScreen();

            try
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    SqlConnection conn = repository.DBConnection;
                    if (conn != null)
                    {
                        MConsole.WriteInfoLine($"{conn.ClientConnectionId}; {conn.State}; {conn.ServerVersion}");
                    }
                }
            }
            catch (Exception ex)
            {
                MConsole.Alert(ex.Message, "Datenbank");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void CreateTable(string databasePath)
        {
            MConsole.ClearScreen();

            try
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    repository.CreateTable();
                }
            }
            catch (Exception ex)
            {
                MConsole.Alert(ex.Message, "Datenbank");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }


        public void ExistDatabase(string databasePath)
        {
            MConsole.ClearScreen();

            try
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    bool existdatabase = repository.ExistDatabase();
                    if (existdatabase == true)
                    {
                        MConsole.Info("Datenbank 'Zollabwicklung' ist vorhanden","Datenbank");
                    }
                    else
                    {
                        MConsole.Alert("Datenbank nicht gefunden", "Datenbank");
                    }
                }
            }
            catch (Exception ex)
            {
                MConsole.Alert(ex.Message, "Datenbank");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void DatabaseInfo(string databasePath)
        {
            MConsole.ClearScreen();

            try
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    string version = repository.Version();
                    DateTime lastAccess = repository.LastWriteTime();
                    long size = repository.Length();
                    MConsole.WriteInfoLine($"Version : {version}");
                    MConsole.WriteInfoLine($"Größe : {size} MB");
                    MConsole.WriteInfoLine($"Letzter Zugriff : {lastAccess.ToString("dd.MM.yyyy HH:mm:ss")}");
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
                }

                MConsole.WriteLine();
            }
            catch (Exception ex)
            {
                MConsole.Alert(ex.Message, "Datenbank");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void DatabaseMetaInfo(string databasePath)
        {
            MConsole.ClearScreen();

            try
            {
                using (DatabaseRepository repository = new DatabaseRepository())
                {
                    List<Tuple<string, string, object, Type>> metaList = repository.MetadataInformation();
                    foreach (Tuple<string, string, object, Type> item in metaList)
                    {
                        MConsole.WriteInfoLine($"Name:{item.Item1}; Value: {item.Item3}");
                    }
                }

                MConsole.WriteLine();
            }
            catch (Exception ex)
            {
                MConsole.Alert(ex.Message, "Datenbank");
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}