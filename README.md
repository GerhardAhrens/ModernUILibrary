# Modern Library für C# Funktionen und UI Controls

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.15-yellow.svg)]

Die beiden Bibliotheken **ModernUI** und **Modern Base Library** sind eine Zusammenfassung von Arbeiten aus den letzten 15 Jahre in C# und WPF. So sind eine vielzahl von UserControls und C# Klassen entstanden, die für eine Wiederverwertung aufbereitet wurden.

Die Funktionalität in zwei Bibliotheken
- ModernUI
- ModernBaseLibrary

aufgeteilt. Zusätzlich gibt es noch
- Modern IU Demo<br> 
  Hier werden die meisten vorhanden UI Elemente in verschiedenen Kategorien dargestellt
- Modern Test
  Für viele Klassen und Funktionen sind Tests erstellt um die Funktionsweis zu dokumentieren.

Desweiteren wird über die beiden Bibliotheken ein Entwicklungsframework zur einfachen Erstellung von Applikationen mit WPF und dem MVVM Pattern zur Verfügung gestellt.
Hier wird aber nicht der klassische Ansatz des MVVM Pattern verfolgt, sondern ein Ansatz den ich **Inside MVVM** nenne.
Mit diesem Ansatz wird auf eine eigene View-Model-Klasse verzichtet. Eine **Window** oder **UserControl** hat hier bereits eine eigene Basisklasse, inder alle notwendigen Funktionalitäten für Binding, Command als ein EventAggregator beinhalten. Das Beispiel dazu ist in dem Projekt **ModernMVVMDemo** dargestellt.

# Hinweis
Die Bibliothek ist für private Zwecke entstanden, daher nicht sichergestellt, das alle UI Controls und Klassen auch einwandfrei funktionieren.
Diese Bibliothek ist eher eine Sammlung von meinen Arbeiten aus den letzten Jahren. Da zu allem auch die Sourcen vorhanden sind, kann jeder notwendige Korrekturen selbst durchführen.

# Modern UI Demo

In dem Demoprogramm werden eine Vielzahl von UI Controls vorgestellt sowie Source zu verschiedenen Themenbereiche als *Code-Snippets* angezeigt.

# Modern UI Controls for NET

Die Bibliothek beinhaltet eine Vielzahl von WPF NET UI controls für z.B.. TextBox in verschiedenen Varianten, Button, CheckBox, RadioButton, ComboBox, einfache Charts usw.

<img src="./ModernUIDemoA.png" style="width:750px;"/>

Ein große Anzahl von Controls sind auf einem einheitlichen *Look and Feel* im Bezug auf Darstellung, Contextmenü aufgebaut.
In vielen Controls, wenn diese mit einem Bild oder Grafik ausgestattet ist, kommen Vektor Grafiken in Form von XAML Icon (PathGeometry) zum Einsatz.<br>
Hier ein paar Beispiele:

## TextBox mit ContextMenu
<img src="./ModernUITextBoxCM.png" style="width:300px;"/>

## Flat Button mit Grafik und Default-Markierung (linker Button)
<img src="./ModernUIButton.png" style="width:250px;"/>

## ComboBox mit ContextMenu
<img src="./ModernUIComboBox.png" style="width:300px;"/>

## DataGrid mit Erweiteren Funktionen
<img src="./ModernUIDataGrid.png" style="width:700px;"/>


# Modern Base Library for NET

Klassen für vielfältigen Funktionen

## Release Notes
![Version](https://img.shields.io/badge/Version-1.0.2024.1-yellow.svg)<br>
- Aufbau der Bibliothek mit UI Controls
- Demo Programm um die Verwendung der UI Controls darzustellen

![Version](https://img.shields.io/badge/Version-1.0.2025.10-yellow.svg)
- Aufbau der Modern Base Library
- Erweiterung des Demoprogramm um C# Source in einem eigenen einfachen Editor darzustellen
