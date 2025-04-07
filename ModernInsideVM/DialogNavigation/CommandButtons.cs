namespace ModernInsideVM.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog mit Steuerung Main Men�")]
        Home = 1,
        [Description("Dialog A - Als �bersicht")]
        DialogA = 2,
        [Description("Dialog B - Mit �bergebener Guid")]
        DialogB = 3,
        [Description("Anwendung beenden")]
        CloseApp = 100,
    }
}
