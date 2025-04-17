namespace ModernTemplate.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog mit Steuerung Main Menü")]
        Home = 1,
        [Description("Benutzer definierter Dialog")]
        Custom = 2,
        [Description("Anwendung beenden")]
        CloseApp = 100,
        [Description("Zurück")]
        DialogBack = 101,
        [Description("Hilfe")]
        Help = 110,
        [Description("Über ...")]
        AppAbout = 111,
        [Description("Anwendung Einstellungen")]
        AppSettings = 112,
    }
}
