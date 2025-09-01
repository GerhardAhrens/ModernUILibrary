# Modern Projekt Template

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

# Modern Template
Das **Modern Template** soll die Entwicklung von Desktop-Anwendungen unter C#/WPF unterstützen. Durch den Einsatz des Templates kann die Entwicklungszeit bis zu 30% reduziert werden. Das wird dadurch erreicht, Das dass Template vile Basis Funktionalitäten wie Ordnerstruktur, Dialog-Management, Logging, Konfigurationsverwaltung usw. bereits integriert hat.

# Interne Abhängigkeiten
Das Template verwendet zwei weitere Bilbliotheken
- ModernBaseLibrary
- ModernUILibrary

Beide liegen als Source in der gleichen Solution **ModernUILibrary**. Er werden dazu keine weiteren Bibliotheken verwendet. Alle weiteren Pakete sind von Microsoft selbst, da das NET Core mit jeder Version stärker framgmentiert wird.
Die beiden Bibliotheken sind im Verzeichnis ***\\_Lib\\*** abgelegt.</br>
Je nach Aufgabe 

# Interne Struktur und Funktion
Anwendungen die auf Basis des **Modern Template** ersstellt werden, sind als *Single Page* Anwendungen ausgelegt. Alle Dialoge werdfen in einem *ContentControl* dargestellt. Die Steuerung zwischen den Dialoge erfolgt über ein Mediator. Im Grunde ist jeder Dialog autark, es können aber über den Mediator Parameter zwischen Dialoge ausgetauscht werden. So kann z.B. von einem Übersichtsdialog ein Bearbeitungsdialog aufgerufen werden.

## Features des Template
Wie obenen bereits erwähnt, bringt das Template einen Reihe von Funktionalitäten mit.

