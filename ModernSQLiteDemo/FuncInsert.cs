namespace ModernSQLiteDemo
{
    using System;
    using System.Data;
    using System.DirectoryServices;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Cryptography;
    using ModernBaseLibrary.Extension;

    using ModernSQLiteDemo.SQLite;

    [SupportedOSPlatform("windows")]
    public class FuncInsert
    {

        public static string DatabasePath { get; set; }

        public void InsertValues(string databasePath)
        {
            const int MAXROW = 100;
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true)
            {
                if (File.Exists(databasePath) == true)
                {
                    MConsole.WriteLine();
                    MConsole.ProgressTitleColor = ConsoleColor.Yellow;
                    MConsole.ProgressBackColor = ConsoleColor.DarkGreen;
                    MConsole.ProgressTotal = MAXROW;
                    MConsole.ProgressTitle = "Insert Demodaten";

                    using (ContactRepository repository = new ContactRepository())
                    {
                        for (int i = 0; i < MAXROW; i++)
                        {
                            DataRow rowNew = repository.NewDataRow();
                            rowNew.SetField<Guid>("Id", Guid.NewGuid());
                            rowNew.SetField<string>("Vorname", new RandomDataContent().Letters(20));
                            rowNew.SetField<string>("Nachname", new RandomDataContent().Letters(20));
                            DateTime geburtsTag = new RandomDataContent().Dates(new DateTime(1960, 1, 1), DateTime.Now);
                            rowNew.SetField<DateTime>("Geburtstag", geburtsTag);
                            rowNew.SetField<int>("Alter", geburtsTag.GetAge());
                            rowNew.SetField<decimal>("Gehalt", new RandomDataContent().NumbersDecimal(1000.0M, 9999.99M, 2));
                            rowNew.SetField<string>("Beschreibung", new RandomDataContent().AlphabetAndNumeric(100));
                            rowNew.SetField<byte[]>("Photo", new byte[0]);
                            rowNew.SetField<bool>("Active", new RandomDataContent().Boolean());
                            rowNew.SetField<string>("CreatedBy", $"{Environment.UserDomainName}\\{Environment.UserName}");
                            rowNew.SetField<DateTime>("CreatedOn", DateTime.Now);
                            repository.Add(rowNew);

                            MConsole.ProgressValue = i;
                        }
                    }

                    MConsole.ProgressValue = 0;
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Es wurden {MAXROW} Datensätze erstellt.");
                }
            }
            else
            {
                MConsole.Alert($"Die Datenbankdatei '{databasePath}' ist nicht vorhanden.","Keine Datei");
            }


            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void InsertByteArray(string databasePath, string blobPath)
        {
            const int MAXROW = 100;
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true)
            {
                if (File.Exists(databasePath) == true)
                {
                    MConsole.WriteLine();
                    MConsole.ProgressTitleColor = ConsoleColor.Yellow;
                    MConsole.ProgressBackColor = ConsoleColor.DarkGreen;
                    MConsole.ProgressTotal = MAXROW;
                    MConsole.ProgressTitle = "Insert Demodaten";

                    using (ContactRepository repository = new ContactRepository())
                    {
                        for (int i = 0; i < MAXROW; i++)
                        {
                            int rndFile = new RandomDataContent().NumbersInt(0, 6);
                            string blobFile = CreateBlobFiles()[rndFile];
                            byte[] blob = File.ReadAllBytes(Path.Combine(blobPath, blobFile));

                            DataRow rowNew = repository.NewDataRow();
                            rowNew.SetField<Guid>("Id", Guid.NewGuid());
                            rowNew.SetField<string>("Vorname", blobFile);
                            rowNew.SetField<string>("Nachname", $"Größe: {blob.Length}");
                            rowNew.SetField<DateTime>("Geburtstag", new DateTime(1960, 1, 1));
                            rowNew.SetField<int>("Alter", 99);
                            rowNew.SetField<decimal>("Gehalt", 1.99M);
                            rowNew.SetField<string>("Beschreibung", string.Empty);
                            rowNew.SetField<byte[]>("Photo", blob);
                            rowNew.SetField<bool>("Active", true);
                            rowNew.SetField<string>("CreatedBy", $"{Environment.UserDomainName}\\{Environment.UserName}");
                            rowNew.SetField<DateTime>("CreatedOn", DateTime.Now);
                            repository.Add(rowNew);

                            MConsole.ProgressValue = i;
                        }
                    }

                    MConsole.ProgressValue = 0;
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Es wurden {MAXROW} Datensätze erstellt.");
                }
            }
            else
            {
                MConsole.Alert($"Die Datenbankdatei '{databasePath}' ist nicht vorhanden.", "Keine Datei");
            }


            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        private List<string> CreateBlobFiles()
        {
            return new List<string>() { "Donald-Erschrocken.gif", "GuteNacht.gif", "Snoopy-Fall.gif", "snoopy-happy-thanksgiving.gif", "snoopy-peanuts.gif", "Snoopy-Play.gif", "wake-up-dog-barking.gif" };
        }
    }
}