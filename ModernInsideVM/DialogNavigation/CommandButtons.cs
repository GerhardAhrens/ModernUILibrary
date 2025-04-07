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
        [Description("Dialog B - Mit übergebener Guid")]
        DialogB = 3,
        [Description("Anwendung beenden")]
        CloseApp = 100,
    }
}
