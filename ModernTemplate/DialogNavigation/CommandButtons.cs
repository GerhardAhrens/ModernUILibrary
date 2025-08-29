//-----------------------------------------------------------------------
// <copyright file="CommandButtons.cs" company="company">
//     Class: CommandButtons
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Zur Steuerung der Command-Button (z.B. im Ribbon Menü) werden hier die Enums eingetragen.
// Diese werden zum einen in 'DialogFactory' als auch für die Menüsteurung benötigt.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [EnumKey("home", "Home Dialog mit Steuerung Main Menü")]
        [Description("Home Dialog mit Steuerung Main Menü")]
        Home = 1,
        [EnumKey("customA","Benutzer definierter Dialog - A")]
        CustomA = 2,
        [EnumKey("customB", "Benutzer definierter Dialog - B")]
        CustomB = 3,
        [EnumKey("katalogA", "Stammdaten Dialog - A")]
        CatKatalogA = 10,
        [EnumKey("katalogB", "Stammdaten Dialog - B")]
        CatKatalogB = 11,
        [EnumKey("katalogC", "Stammdaten Dialog - C")]
        CatKatalogC = 12,
        [EnumKey("", "Anwendung beenden")]
        CloseApp = 100,
        [EnumKey("", "Zurück")]
        DialogBack = 101,
        [EnumKey("", "Hilfe")]
        Help = 110,
        [EnumKey("about", "Über ...")]
        AppAbout = 111,
        [EnumKey("settings", "Anwendung Einstellungen")]
        AppSettings = 112,
        [EnumKey("templateOverview", "Template Übersicht")]
        TemplateOverviewUC = 1000,
        [EnumKey("templateDetail", "Template Detail")]
        TemplateDetailUC = 1001
    }
}
