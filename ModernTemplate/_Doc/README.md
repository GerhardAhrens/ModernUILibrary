# Modern Projekt Template

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

# ModernTemplate
Das Template "ModernTemplate" kann �ber das Visual Studio als eine neuen Projekt ausgew�hlt werden.

## Neues Projekt erstellen
<img src="MT_NewProject.png" style="width:750px;"/>

## Installation

Zur Installation und Verwendung der Projektvorlage **ModernTemplate.zip** mu� diese zuvor in folgendes Verzeichnis kopiert werden.

```bat
c:\Users\<username>\Documents\Visual Studio 2022\Templates\ProjectTemplates\Visual C#\ModernUI\ModernTemplate.zip

*Template Cache
c:\Users\<username\AppData\Roaming\Microsoft\VisualStudio\17.0_a0c33062\ItemTemplatesCache\Visual C#\ModernUI\
```

## Features
Das Template erstell Anwendungen werden auf Basis eine *Single Page Architektur* entwickelt. Es gibt ein Hauptwindow, in dem alle weitere Dialoge als UserControl abgebildet geladen werden. Die Steuerung zwischen den UserControls (bzw. Dialoge als UserControl) erfolgt �ber eine Class-To-Class Kommunikation die im *InsindeVM Framework* als EventAggregator abgebildet ist.</br>
Es wird intern das bekannte **MVVM-Pattern** abgebildet, alledings in einer stark vereinfachte Weise und kommt daher ohne eine ViewModel-Klasse aus.
Trotzdem ist es nach wie vor m�glich, eine externe ViewModel-Klasse zu verwenden. In den Basis-Klassen f�r Window als auch f�r UserControl sind alle Funktionalit�ten f�r das **MVVM-Pattern** abgebildet. Das Binding zwischen XAML und der dazugeh�ringen *xaml.cs* funktioniert wie auch bei einer extrenen ViewModel-Klasse.</br>
Nachteil dieser L�sung ist, komplexere Arten von Unit-Test k�nnen nicht so einfach umgesetzt werden.

### Hauptdialog
<img src="MT_MainDialog.png" style="width:750px;"/>

### Dialog-To-Dialog Kommunikation
Die Kommunikation zwischen den Dialogen (aber auch Klassen) erfolgt �ber einen Observer.

Senden von Werten
```csharp
base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
{
    Sender = this.GetType().Name,
    MenuButton = CommandButtons.Home,
    FromPage = CommandButtons.Custom
});
```

Der *Publisher* sendet eine Information an alle *Subscriber*, die das Event-Argument *ChangeViewEventArgs* registriert haben.

Empfangen und ausf�hren
```csharp
base.EventAgg.Subscribe<ChangeViewEventArgs>(this.ChangeControl);
```

Der *Subscriber* empf�ngt die Werte, aller Instanzen, die f�r das Event-Argument *ChangeViewEventArgs* bestimmt sind. Der *Subscriber* f�hrt automatisch die ihm zugewiesene Methode (im Beispiel **ChangeControl()**) mit den dazugeh�rigen Event-Arguments aus.

### Commands 

Ein Klick auf einen Button l�st �ber einen gebundenen *Command* ein Event im C# Source aus.
```xml
<ModernUI:PathButton
    Width="100"
    Height="30"
    Command="{Binding Path=CmdAgg[DialogBackCommand]}"
    IsDefault="True"
    PathData="{StaticResource ResourceKey=IconDialogBack}"
    PathWidth="14">
    <ModernUI:PathButton.ToolTip>
        <ModernUI:MToolTip Content="Zur�ck zum vorherigen Dialog" PlacementEx="TopLeft" />
    </ModernUI:PathButton.ToolTip>
    <ModernUI:PathButton.Content>
        <TextBlock>
            <Run Text="Zur�ck" />
        </TextBlock>
    </ModernUI:PathButton.Content>
</ModernUI:PathButton>
```

Im C# Source werden die ben�tigten Commands f�r den XAML-Code registriert und eine Methode zur Ausf�hrung zugewiesen.
Hier gibt es nun die M�glichkeite per Enum; Hier wird der Textteil **Command** intern angeh�ngt.</br>
Aus dem Enum *CommandButtons.DialogBack* wird intern *DialogBackCommand* wie es auch im XAML geschrieben ist.
```csharp
this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
```

oder als String. Hier mu� die Texterweiterung *Command* nicht extra hinzugef�gt werden

```csharp
this.CmdAgg.AddOrSetCommand("DialogBackCommand", new RelayCommand(this.DialogBackHandler));
```

### Get/Set
Im Unterschied zu einem *Standard Get;Set;* werden bei dieser Variante keine *Private Field-Variabeln* ben�tigt. Die Bindungs-Funktion �ber das *INotifyPropertyChanged* ist bereits in der Basis-Klasse enthalten. Daher kann eine vereinfachte schreibweise verwendet werden.
```csharp
public string DemoText
{
    get => base.GetValue<string>();
    set => base.SetValue(value);
}
```
In der zweiten Variante kann auch eine Methode registriert werden, die z.B. bei der Eingabe ausgel��t wird.
```csharp
public string Titel
{
    get => base.GetValue<string>();
    set => base.SetValue(value, this.CheckContent);
}
```
Hier kann eine Behadlung oder Pr�fung der Eingabe durchgef�hrt werden.
```csharp
private void CheckContent<T>(T value, string propertyName)
{
    PropertyInfo propInfo = this.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyName);
    if (propInfo != null)
    {
        this.ChangedContent(true);
    }

    this.ValidationErrors.Clear();
    foreach (string property in this.GetProperties(this))
    {
        Func<Result<string>> function = null;
        if (this.ValidationRules.TryGetValue(property, out function) == true)
        {
            Result<string> ruleText = this.DoValidation(function, property);
            if (string.IsNullOrEmpty(ruleText.Value) == false)
            {
                this.ValidationErrors.Add(property, ruleText.Value);
            }
        }
    }
}
```

### Validierung von Eingaben

Die Pr�fung der Eingaben erfolgt �ber die Klasse *ValidationRule* im Namespace *Core*. Hier sind schon einige Pr�fungen hinterlegt, die bei Bedarf erweitert werden k�nnen.

|Name  |Typ|Beschreibung |
|:----------------|:-|:----------|
|NotEmpty|String|Inhalt darf nicht leer sein|
|NotEmptyAndMinChar|String|Inhalt darf nicht leer sein, und mind. x Zeichen|
|InRange|Int|Der Inhalt mu� zwischen x und y liegen|
|GreaterThanZero|Int/Double|Der Inhalt mu� gr��er 0 sein|

```csharp
private void RegisterValidations()
{
    this.ValidationRules.Add(nameof(this.Titel), () =>
    {
        return ValidationRule<DialogC>.This(this).NotEmpty(x => x.Titel, "Titel");
    });

    this.ValidationRules.Add(nameof(this.Description), () =>
    {
        return ValidationRule<DialogC>.This(this).NotEmpty(x => x.Description, "Beschreibung");
    });
}
```

<img src="MT_Validierung.png" style="width:750px;"/>

Die Fehlerhaften Elemente werden in einer ListBox dargestellt. �ber die Auswahl in der ListBox kann auf das jeweilige Eingabeelement gesprungen werden.

### Applikation Settings
Die Einstellungen f�r eine Applikation werden in eine JSON Datei unter *ProgramData\\APPLICATION* gespeichert.
Der Dateiname wird als *APPLICATION.USERNAME.Setting* erstellt.</br>
Die Basisklasse *SmartSettingsBase* beinhaltet alle Funktionen zum Lesen, Schreiben und pr�fen der Settings.

```csharp
    public class ApplicationSettings : SmartSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
        /// </summary>
        public ApplicationSettings() : base(null,App.SHORTNAME) { }

        public string LastUser { get; set; }

        public DateTime LastAccess { get; set; }

        public bool ExitApplicationQuestion { get; set; }

        public bool SaveLastWindowsPosition { get; set; }

        public int SetLoggingLevel { get; set; }
    }
```

**Beschreibung der Funktionen**

|Name  |Typ|Beschreibung |
|:----------------|:-|:----------|
|Save()|void|Speichern des aktuellen Inhalt in eine JSON Datei|
|Load()|bool|Laden der Einstellungen aus einer JSON Datei in die daf�r vorgesehende Properties|
|Reset()|void|Alle Properties auf den festgelegten Default-Wert setzten|
|Delete()|void|Die Settingsdatei l�schen|
|IsExitSettings()|bool|Pr�ft, ob die Settingsdatei vorhanden ist|
|Filename|string|Gibt den Namen der Settingsdatei zur�ck|
|Pathname|string|Gibt das Verzeichnis der Settingsdatei zur�ck|
|GetProperties|IReadOnlyList\<PropertyInfo>|Gibt Liste der Properties zur�ck|
|GetDefaults|IReadOnlyList\<PropertyInfo>|Gibt Liste der Default Properties zur�ck|

### Logging

### Message Dialoge

## ModernTemplate erstellen

# Release Notes

![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)
- ModernTemplate; Projekt Template zur Auswahl mit dem Visual Studio f�r die Neuerstellung eines Projektes.
