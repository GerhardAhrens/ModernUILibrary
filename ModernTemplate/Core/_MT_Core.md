# Verzeichnis Core

Im Verzeichnis *Core* werden alle Funktionalitäten abgelegt, die in der gesamten Anwendung verwendet werden können.

<span style="color:red">**Hinweis:**</span> Das "\\Core" Verzeichnis kann um eigenen zusätzlichen Verzeichnisse und Klassen erweitert werden.

## <a name="CoreLogging"></a>Logging
Es wird eine Logging-Funktion zur Verfügung gestellt, deren Funktionalität durch die abzuleitende Klasse *AbstractOutHandler* verändert bzw. anggepasst werden kann. Im Standard erfolgt das Logging in eine datei über die Klasse *LogFileOutHandler*.
```csharp
App.Logger.Error(ex, errorText);
```
oder
```csharp
App.Logger.Warning($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
```
Die Stufe des Errorlevel kann konfiguriert werden und wird erst nach dem Neustart der Anwendung wirksam. Die Log-Datei wird nach *ProgramData\\anwendung\\Log\\* geschrieben.


## <a name="CoreSettings"></a>Settings
Unabhängig der *app.config* steht die Basisklasse *SmartSettingsBase* zur Erstellung und Bearbeitung einer Konfiguration zur Verfügung. Diese Konfigurationsdatei wird unter dem Verzeichnis *ProgramData\\<AppName>* als **JSON Datei** gespeichert.
Der Vorteil dieser Möglichkeit ist zum einen die einfachen Verwendung beim Lesen und Schreiben, aber auch die Typ-Sicherheit.
So kann eine einfache Klasse mit Properties erstellt werden, die von *SmartSettingsBase* ableitet.

```csharp
public class ApplicationSettings : SmartSettingsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
    /// </summary>
    public ApplicationSettings() : base(null,App.SHORTNAME) { }

    public string DatenbankConnection { get; set; }

    public string LastUser { get; set; }

    public DateTime LastAccess { get; set; }

    public bool ExitApplicationQuestion { get; set; }

    public bool SaveLastWindowsPosition { get; set; }

    public int SetLoggingLevel { get; set; }
}
```

Auf Grund der einfachen Struktur ist eine Anpaasung auch bei späteren Erweiterungen möglich.
```json
{
  "DatenbankConnection": null,
  "LastUser": null,
  "LastAccess": "0001-01-01T00:00:00",
  "ExitApplicationQuestion": true,
  "SaveLastWindowsPosition": false,
  "SetLoggingLevel": 0
}
```

Die Basisklasse stellt eine Reihe von Methoden zum lesen und Schreiben zur Verfügung.

```csharp
private void InitializeSettings()
{
    using (ApplicationSettings settings = new ApplicationSettings())
    {
        settings.Load();
        if (settings.IsExitSettings() == false)
        {
            settings.ExitApplicationQuestion = App.ExitApplicationQuestion;
            settings.SaveLastWindowsPosition = App.SaveLastWindowsPosition;
            settings.SetLoggingLevel = App.SetLoggingLevel;
            settings.Save();
        }
        else
        {
            App.ExitApplicationQuestion = settings.ExitApplicationQuestion;
            App.SaveLastWindowsPosition = settings.SaveLastWindowsPosition;
            App.SetLoggingLevel = settings.SetLoggingLevel;
        }
    }
}
```

## <a name="CoreValidation"></a>Eingabe Validierung

Die Prüfung von Eingaben erfolgt in zwei Schritten. 
- Erstellen und Registrieren einer Prüfregel
- Prüfung z.B. während der Eingabe und darstellen (als Popup) wenn ein Fehler vorhanden ist.

Im Verzeichnis **Core\\ValidationRules** befindet sich die statische Klasse *InputValidation*, in der bereits verschiedene Prüfregeln abgelegt sind.
Wie im Architekturkonzept festgelegt, arbeitet auch die Validierung mit einem *DataRow*.
```csharp
public class InputValidation<TDataRow> where TDataRow : class
{}
```
Die Klasse selbst verwendet eine Fluent-API und arbeitet typisiert. Das Ergebnis wird über ein Result-Objekt zurückgegeben.
```csharp
public Result<string> NotEmpty(string fieldName, string displayName = "")
{
    string result = string.Empty;
    bool resultValidError = false;
    string propertyValue = (string)((DataRow)validation.ThisObject).GetAs<string>(fieldName);

    displayName = string.IsNullOrEmpty(displayName) == true ? fieldName : displayName;

    if (string.IsNullOrEmpty(propertyValue) == true)
    {
        result = $"Das Feld '{displayName}' darf nicht leer sein.";
        resultValidError = true;
    }

    return Result<string>.SuccessResult(result, resultValidError);
}
```

Die Validierungsregel wird einem Dictionary hinzugefügt und ausgewertet.
```csharp
this.ValidationRules.Add("ShortName", () =>
{
    return InputValidation<DataRow>.This(this.CurrentRow).NotEmpty("ShortName", "Benutzername");
});
```

Die Methode *CheckInputControls* wertet die Regel aus und steuert die Darstellung
```csharp
private bool CheckInputControls(params string[] fieldNames)
{
    bool result = false;

    try
    {
        foreach (string fieldName in fieldNames)
        {
            Func<Result<string>> function = null;
            if (this.ValidationRules.TryGetValue(fieldName, out function) == true)
            {
                Result<string> ruleText = this.DoValidation(function, fieldName);
                if (string.IsNullOrEmpty(ruleText.Value) == false)
                {
                    if (this.ValidationErrors.ContainsKey(fieldName) == false)
                    {
                        NotifiactionPopup.PlacementTarget = this.BtnShowErrors;
                        NotifiactionPopup.Delay = 5;
                        Popup po = NotifiactionPopup.CreatePopup(fieldName, ruleText.Value);
                        po.IsOpen = true;
                        this.ValidationErrors.Add(fieldName, po);
                    }
                }
                else
                {
                    if (this.ValidationErrors.ContainsKey(fieldName) == true)
                    {
                        NotifiactionPopup.Remove();
                        this.ValidationErrors.Remove(fieldName);
                    }
                }
            }
        }

        if (this.ValidationErrors != null && this.ValidationErrors.Count > 0)
        {
            result = true;
        }
        else if (this.ValidationErrors != null && this.ValidationErrors.Count == 0)
        {
            NotifiactionPopup.Reset();
            result = false;
        }
    }
    catch (Exception ex)
    {
        string errorText = ex.Message;
        throw;
    }

    return result;
}
```

Zusätzlich ist eine Möglichkeit in den Popup implementiert, um durch Klick auf das jeweilige Feld zu springen auf dem ein Fehler aufgetreten ist.
Dazu muß das Event *PopupClick* registriert und im UnLoad wieder De-Registriert werden.
```csharp
 NotifiactionPopup.PopupClick += this.OnNotifiactionPopupClick;
```

## Meldungsdialoge (auch intern Notification)
Meldungen in der Anwendung bestehen aus zwei Teilen. Einem *NotificationService* für den bestimmte Meldungstypen registriert werden müssen, und die Meldung selbst die als Exctension Methode auf dem Interface *INotificationService* aufgerufen wird. Die Methoden, die die Meldungen beinhalten sind in der statischen Klasse *Core\\MessageContent.cs* gesammelt.
Registrierung der Meldungstypen.
```csharp
NotificationService.RegisterDialog<QuestionYesNo>();
NotificationService.RegisterDialog<MessageOk>();
```

Die verschiedenen Meldungstypen sind im Verzeichnis *Views\\NotificationContent* abgelegt. Diese können als Vorlage zur Implementierung weiterer Meldungstypen dienen. Auf diese Weise können nicht nur einfache **Ok** oder **Ja/Nein** Dialoge erstellt werden, sondern auch komplexere Dialoge für die Eingabe oder Auswahl von Daten.

<span style="color:red">**Wichtig:**</span> Die zu verwendenten Dialogtypen müssen im jedenfall vor der Verwendung registriert werden.

Implementierung der Meldung **ApplicationExit()**.
```csharp
public static NotificationBoxButton ApplicationExit(this INotificationService @this)
{
    (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Soll das Programm beendet werden?", MSG_FONTSIZE);
    NotificationBoxButton questionResult = NotificationBoxButton.No;

    @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
    {
        if (result == true && tag != null)
        {
            questionResult = ((Tuple<NotificationBoxButton>)tag).Item1;
        }
    });

    return questionResult;
}
```

und Aufruf der Meldung aus der Anwendung
```csharp
NotificationBoxButton result = this.Notifications.ApplicationExit();
if (result == NotificationBoxButton.Yes)
{
    this.ExitApplication(e);
}
else
{
    e.Cancel = true;
}
```

### [Erstellung eigener Notification-Typen](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/Views/NotificationContent/_MT_NotificationContent.md)

## Commandline Unterstützung
Auch wenn die Anwendung in erster Linie als Windows Anwendung konzipiert ist, kann diese trotzdem bei Bedarf mit einer Komandozeile aufgerufen werden.
In dem Template wird als Beispiel der Username verwendung um diese mit einer Kommandozeile übergeben zu können.
```text
anwendung.exe -u=snoopy
anwendung.exe -username=snoopy
```

Der Commandline-Parser bekommt die aktuelle Commnadline übergeben.
```csharp
string[] cmdArgs = CommandManager.CommandLineToArgs(Environment.CommandLine);
CommandParser parser = new CommandParser(cmdArgs);
ApplicationCmdl commandLineInfo = parser.Parse<ApplicationCmdl>();
string[] helpText = parser.GetHelpInfo<ApplicationCmdl>()?.Split('\n');
```

Das Ergebnis des Parser wird in eine festzulegende Klasse geschrieben.
Wichtig hierbei ist, das der Parameter - Flag "Longname" mit dem Propertiename übereinstimmen muss.
Im Beispiel Property **Username** und [Flag("**username**", "u")]
```csharp
public class ApplicationCmdl
{
    /// <summary>
    /// -u= oder -username=
    /// </summary>
    [Flag("username", "u")]
    [Help("Benutzername")]
    public string Username { get; set; }

    [Flag("timeout", "t")]
    [Help("Benutzername")]
    public int Timeout { get; set; }
}
```
Die Klasse kann einen beliebigen Namen und Inhalt haben, muß aber mit den darüberstehenden Attributen zusammenpassen. Auf diese Weise kann auch mit einer *typisierten* Commandline gearbeitet werden. Der Commandline-Parser identifiziert auf Grund der Commandline-Klasse die Properties und Commandline-Parameter und konvertiert den Parameter in den Typ des Properties.

## Einfache Benutzerverwaltung


