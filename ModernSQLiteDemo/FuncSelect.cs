namespace ModernSQLiteDemo
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.DirectoryServices;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Cryptography;
    using ModernBaseLibrary.Extension;

    using ModernSQLiteDemo.SQLite;

    [SupportedOSPlatform("windows")]
    public class FuncSelect
    {

        public static string DatabasePath { get; set; }

        public void SelectAsICollectionView(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                ICollectionView selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.SelectAsICollectionView();

                }

                if (selectResult != null)
                {
                    int count = selectResult.Cast<DataRow>().Count();
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Es wurden {count} Datensätze gelesen.");
                    MConsole.WriteLine();

                    selectResult.MoveCurrentToFirst();
                    DataRow dr = selectResult.CurrentItem as DataRow;
                    string drContent = dr.ItemArrayToString();
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"DataRow {drContent}");
                    MConsole.WriteLine();

                    var columns = dr.ColumnsToDictionary();
                    foreach (KeyValuePair<string, Type> column in columns)
                    {
                        MConsole.WriteInfoLine($"Column {column.Key}; {column.Value.Name}");
                    }

                    MConsole.WriteLine();
                }
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectAsDataTable(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                DataTable selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.SelectAsDataTable();

                }

                if (selectResult != null)
                {
                    int count = selectResult.Rows.Count;
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Es wurden {count} Datensätze gelesen.");
                    MConsole.WriteLine();

                    DataRow dr = selectResult.FirstRow();
                    string drContent = dr.ItemArrayToString();
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"DataRow {drContent}");
                    MConsole.WriteLine();

                    var columns = dr.ColumnsToDictionary();
                    foreach (KeyValuePair<string, Type> column in columns)
                    {
                        MConsole.WriteInfoLine($"Column {column.Key}; {column.Value.Name}");
                    }

                    MConsole.WriteLine();
                }
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectAsDictionary(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                Dictionary<Guid, string> selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.SelectAsDictionary();

                }

                if (selectResult != null)
                {
                    int count = selectResult.Count;
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"Es wurden {count} Datensätze gelesen.");
                    MConsole.WriteLine();

                    KeyValuePair<Guid,string> dr = selectResult.FirstOrDefault();
                    string drContent = dr.ToString();
                    MConsole.WriteLine();
                    MConsole.WriteInfoLine($"KeyValuePair {drContent}");
                    MConsole.WriteLine();
                }
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectAsScalarCount(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                int selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.Count();

                }

                MConsole.WriteLine();
                MConsole.WriteInfoLine($"Es wurden {selectResult} Datensätze gelesen.");
                MConsole.WriteLine();
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectAsScalarSum(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                decimal selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.SumGehalt();

                }

                MConsole.WriteLine();
                MConsole.WriteInfoLine($"Ergebnis: {selectResult.ToString("F2")}.");
                MConsole.WriteLine();
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectAsScalarByteArray(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                byte[] selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    selectResult = repository.GetPhoto();

                }

                MConsole.WriteLine();
                MConsole.WriteInfoLine($"Ergebnis: {selectResult.Length.ToByteText()}.");
                MConsole.WriteLine();
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        public void SelectById(string databasePath)
        {
            MConsole.ClearScreen();

            if (Directory.Exists(Path.GetDirectoryName(databasePath)) == true && File.Exists(databasePath) == true)
            {
                DataRow selectResult;
                using (ContactRepository repository = new ContactRepository())
                {
                    DataRow firstRow = repository.SelectByFirst();
                    selectResult = repository.SelectById(firstRow.GetAs<Guid>("Id"));

                }

                string drContent = selectResult.ItemArrayToString();
                MConsole.WriteLine();
                MConsole.WriteInfoLine($"DataRow {drContent}");
                MConsole.WriteLine();
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}