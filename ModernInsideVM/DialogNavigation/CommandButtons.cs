namespace ModernInsideVM.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog mit Steuerung Main Menü")]
        Home = 1,
        [Description("Dialog A - Als Übersicht")]
        DialogA = 2,
        [Description("Dialog B - Mit übergebener Guid (Detail)")]
        DialogB = 3,
        [Description("Dialog C - Mit Eingabe Validierung")]
        DialogC = 4,
        [Description("Anwendung beenden")]
        CloseApp = 100,
        [Description("Hilfe")]
        Help = 101,
        [Description("Über ...")]
        AppAbout = 102,
    }
}
