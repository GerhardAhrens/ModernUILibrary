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
        [EnumKey("custom","Benutzer definierter Dialog")]
        Custom = 2,
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
    }
}
