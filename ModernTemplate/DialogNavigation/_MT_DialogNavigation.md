# Verzeichnis DialogNavigation

Die *Dialog Navigation* ist eines der Kernstück des Templates. Mit Hilfe eines Mediators und einer Factory werden UserControls für Dialoge/Views geladen und dargestellt. Hierbei hat jedes CuserControl das als Dialog verwendet wird eine in sich geschlossene Struktur.

Die Funktionalität der *Dialog Navigation* besteht aus den Klassen *CommandButtons* (als Enum) und *DialogFactory* (als eigentliche Verwaltung).

In dem Enum *CommandButtons* werden alle Menüpunkte festgelegt, denen auch ein UserControl zugeordnet wird.
```csharp
public enum CommandButtons : int
{
    [Description("Keine Auswahl")]
    None = 0,
    [EnumKey("home", "Home Dialog mit Steuerung Main Menü")]
    Home = 1,
    [EnumKey("customA","Benutzer definierter Dialog - A")]
    CustomA = 2,
}
```

Rückgabe eines UserControl für den übergebenen *CommandButtons*
```csharp
public static FactoryResult Get(CommandButtons mainButton)
{
    FactoryResult resultContent = null;
    using (LoadingWaitCursor wc = new LoadingWaitCursor())
    {
        using (LoadingViewTime lvt = new LoadingViewTime())
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (Views.ContainsKey(mainButton) == true)
            {
                UserControlBase resultInstance = CreateInstanceContent(mainButton, null);
                resultContent = new FactoryResult(resultInstance);
                resultContent.WorkContent.Focusable = true;
                resultContent.WorkContent.Focus();
                resultContent.UsedTime = lvt.Result();
                resultContent.ButtonDescription = mainButton.ToDescription();
            }
        }
    }

    return resultContent;
}
```

Aufgerufen von *ChangeControl(ChangeViewEventArgs e)* im MainWindow.
```csharp
private void ChangeControl(ChangeViewEventArgs e)
{
    FactoryResult view = DialogFactory.Get(e.MenuButton, e);
}
```