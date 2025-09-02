# Verzeichnis Repository

Im Verzeichnis *Repository* sind erstmal keine weiteren Klassen als Unterstützung vorhanden. Das liegt in der Haupsache daran, das dass Bearbeiten von Daten beliebig komplex und auch sehr unterschiedliche Datenquellen vorhanden sein können.</br>
Daher dient nun als grundlegendes Beispiel das Arbeiten mit einer [SQLite](https://sqlite.org/) Datenbank.

Hierzu gibt es eine [Hilfts-Bibliothek](https://github.com/GerhardAhrens/ModernUILibrary/tree/master/ModernSQLite) mit einer Reihe von Klassen zur Unterstützung der Arbeit mit der Datenbank. Grundsätzlich ist es ohne größeren Aufwand möglich, diese Biblitothek für weitere Datenbank zu implementitieren.
Also, konkret ein eigenes Projekt erstellen und die Klassen 1:1 übernehmen und für die neue Datenbank anzupassen.

Um die Vorteile tatsächlich auch nutzen zu können, erwähne ich hier nochmal, das Anwendungen auf Basis dieses Template erstellt werden, am effizientesten mit der Kombination DataTable und DataRow zu erstellen sind, da es so möglich ist auf Model-Klassen oder DTO-Objekte zu verzichten. Das DataTable bzw. DataRow dienen hier das Daten-Container die ebenfalls an WPF UI Elemente gebunden werden können.

Als Vorschlag dienen also folgende Kombinationen:
- ListView, DataGrid (je nach Bedarf, ICollectionView oder DataTable)
- ComboBox, ListBox (Dictionary<TKey, TValue>, ICollectionView)
- Standard IU Elemente (DataRow)

Beispiel für Standard CRUD Funktionen
```csharp
public class TagWordsRepository : SQLiteDBContext
{
    private const string TAB_NAME = "TAB_TagWords";

    public TagWordsRepository() : base(App.DatabasePath)
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

    public ICollectionView Select()
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

                this.Connection.RecordSet<int>(sql.Item1, sql.Item2).Execute();
            }
        }
        catch (Exception ex)
        {
            string errorText = ex.Message;
            throw;
        }
    }
}
```

Diese Funktioen sind für Standard-Operationen gedacht, die aber den größeren Teil einer Anwendung ausmachen. Da aber hier das Konzept offen ist, kann hier im Prinzip alles verwendet werden.

In der Anwendung sieht dann die Verwendung wie folgt aus. Lesen aller Records aus einer Tabelle
```csharp
using (TagWordsRepository repository = new TagWordsRepository())
{
    this.DialogDataView = repository.Select();
}
```

Neuer Eintrag aus einer **DataRow** erstellen.
```csharp
using (TagWordsRepository repository = new TagWordsRepository())
{
    repository.Add(this.CurrentRow);
}
```

In der Hilfssammlung für die Datenbank sind die verschiedenen Ausprägungen von Generatoren, Extensions (darunter die RecordSet Klasse).
Gerade die mächtige **RecordSet** Klasse kann eine erheblich Erleichterung sein, da diese in der Lage ist SQL Anweisungen als bestimmte Datentypen zurück zu geben. Hier entfällt in der eigentlichen Anwendung das konvertieren in den gewünschten Datentyp.

Verschiedene Ausprägungen für die Klasse **RecordSet**
Neues **DataRow** auf Basis einer Tabelle erstellen
```csharp
result = base.Connection.RecordSet<DataRow>(this.Tablename).New().Result;
```

Count einer Tabelle abfragen
```csharp
result = base.Connection.RecordSet<int>($"select count(*) from {this.Tablename}").Get().Result;
```

Datensätze aus einer Tabelle zurückgeben
```csharp
result = base.Connection.RecordSet<ICollectionView>($"SELECT * FROM {this.Tablename}").Get().Result;
```

Spezifisches DataRow einer Tabelle zurückgeben
```csharp
sqlStatement = $"SELECT * FROM {this.Tablename} WHERE Id = '{id.ToString()}'";
result = base.Connection.RecordSet<DataRow>(sqlStatement).Get().Result;
```
