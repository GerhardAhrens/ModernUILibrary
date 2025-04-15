namespace ModernTemplate.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog mit Steuerung Main Men�")]
        Home = 1,
        [Description("Anwendung beenden")]
        CloseApp = 100,
        [Description("Hilfe")]
        Help = 101,
        [Description("�ber ...")]
        AppAbout = 102,
    }
}
