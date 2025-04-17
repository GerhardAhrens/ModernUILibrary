namespace ModernTemplate.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog mit Steuerung Main Men�")]
        Home = 1,
        [Description("Benutzer definierter Dialog")]
        Custom = 2,
        [Description("Anwendung beenden")]
        CloseApp = 100,
        [Description("Zur�ck")]
        DialogBack = 101,
        [Description("Hilfe")]
        Help = 110,
        [Description("�ber ...")]
        AppAbout = 111,
        [Description("Anwendung Einstellungen")]
        AppSettings = 112,
    }
}
