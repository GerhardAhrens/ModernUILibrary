# Verzeichnis Core

Im Verzeichnis *Core* werden alle Funktionalitäten abgelegt, die in der gesamten Anwendung verwendet werden können.

## Logging
Es wird eine Logging-Funktion zur Verfügung gestellt, deren Funktionalität durch die abzuleitende Klasse *AbstractOutHandler* verändert bzw. anggepasst werden kann. Im Standard erfolgt das Logging in eine datei über die Klasse *LogFileOutHandler*.
```csharp
App.Logger.Error(ex, errorText);
```
oder
```csharp
App.Logger.Warning($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
```
Die Stufe des Errorlevel kann konfiguriert werden und wird erst nach dem Neustart der Anwendung wirksam.

## Settings

##  Eingabe Validierung

## Meldungsdialoge (auch intern Notification)

