# Modern Projekt Template

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

# Modern Template
Das **Modern Template** soll die Entwicklung von Desktop-Anwendungen unter C#/WPF unterstützen. Durch den Einsatz des Templates kann die Entwicklungszeit bis zu 30% reduziert werden. Das wird dadurch erreicht, Das dass Template vile Basis Funktionalitäten wie Ordnerstruktur, Dialog-Management, Logging, Konfigurationsverwaltung usw. bereits integriert hat.

## Wie wird die Entwicklungszeit reduziert?
Zum einen durch den Einsatz des Templates in dem bereits ein Teil der Basisstrauktur einer Desktop-Anwendung abgebildet ist. Nach dem das Template in einer eigenen Solution angewedet wird, kann die Solution direkt kompiliert und gestartet werden. So hat mann schon schnell einen Klickbaren-Prototyp.</br>
Eine weitere Reduktion des Aufwandes erfolgt durch den Verzicht auf die strikte Anwendung des MVVM-Pattern. Daraus ergibt sich die Möglichkeit GUI-nahe Funktionen direkt im *Code-Behinde* zu erstellen.</br>
Je nach Bedarf können natürlich auf ViewModel-Klassen erstellt und verwendet werden. In jedem Fall wird aber das Binding von Controls weiter verwendet. So ist es eben auch möglich eine DataTable bzw. DataRow an ein Control zu binden. Es muß nur eine andere Schreibweise verwendet werden.

```xml
<ModernUI:TextBoxEx
    x:Name="txtWord"
    Grid.Row="0"
    Grid.Column="1"
    Width="{StaticResource ResourceKey=TextBoxWidth}"
    MaxLength="250"
    Text="{Binding Path=CurrentRow[Word], UpdateSourceTrigger=PropertyChanged}" />
```
Das zu Bindende Property (bzw. eigentlich das ItemArray-Element) wird in eckiger Klammer geschrieben.
Ein weiterer Punkt für die Aufwandsreduzierung ist eine Umfangreiche Bibliothek mit UI Controls und C# Klassen für viele verschiedene Bereiche.</br>
Zum einen bilden die beiden Bibliotheken die Grundlage des Entwicklungsframework bzw. des Templates weiter ist die Bibliothek auch eine Quelle für Informationen zur Erstellung eigener Funktionaliäten gedacht.

# Interne Abhängigkeiten
Das Template verwendet zwei weitere Bilbliotheken
- ModernBaseLibrary
- ModernUILibrary

Beide liegen als Source in der gleichen Solution **ModernUILibrary**. Er werden dazu keine weiteren Bibliotheken verwendet. Alle weiteren Pakete sind von Microsoft selbst, da das NET Core mit jeder Version stärker fragmentiert wird.
Die beiden Bibliotheken sind im Verzeichnis ***\\_Lib\\*** abgelegt.</br>
Je nach Aufgabe können weitere Bibliotheken auch in Form von GuGet-Paketen der Solution hinzugefügt werden.

# Interne Struktur und Funktion
Anwendungen die auf Basis des **Modern Template** erstellt werden, sind als *Single Page* Anwendungen ausgelegt. Alle Dialoge werdfen in einem *ContentControl* dargestellt. Die Steuerung zwischen den Dialoge erfolgt über ein Mediator. Im Grunde ist jeder Dialog autark, es können aber über den Mediator Parameter zwischen Dialoge ausgetauscht werden. So kann z.B. von einem Übersichtsdialog ein Bearbeitungsdialog aufgerufen werden.

## Datenzugriffe
Die Datenzugriffe erfolgen über Repository-Klassen. Im einfachsten Fall kann zu einem Dialog (Übersicht und Bearbeitung) eine Klasse vorhanden sein die entweder eine Liste (irgendein Listentyp) oder einen Datesansatz zur Bearbeitung (CRUD) zurück (Typ-spezifisch) gibt. 

Die gesamte Datenhaltung zwischen der GUI und der Datenbank wird auf Basis von DataTable und DataRow gehalten.


## Features des Template
Wie obenen bereits erwähnt, bringt das Template einen Reihe von Funktionalitäten mit.

### [Logging](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/Core/_MT_Core.md#CoreLogging)
### [Setting](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/Core/_MT_Core.md#CoreSettings)
### [Eingabe Validierung](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/Core/_MT_Core.md#CoreValidation)
### [Meldungsdialoge (auch intern Notification)](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/Core/_MT_Core.md)

# Nach der Fertigstellung einer Anwendung
Nach der Fertigstellung können die verschiedenen Beschreibungen als auch Klassen/XAML Templates gelöscht werden.
Die Beschreibungen dienen bei der Erstelllung einer neuen Anwendung als Hilfestellung und Orientierung.
