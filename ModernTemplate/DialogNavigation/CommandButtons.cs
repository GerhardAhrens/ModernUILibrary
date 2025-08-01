namespace ModernTemplate.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [EnumKey("home", "Home Dialog mit Steuerung Main Men�")]
        [Description("Home Dialog mit Steuerung Main Men�")]
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
        [EnumKey("", "Zur�ck")]
        DialogBack = 101,
        [EnumKey("", "Hilfe")]
        Help = 110,
        [EnumKey("about", "�ber ...")]
        AppAbout = 111,
        [EnumKey("settings", "Anwendung Einstellungen")]
        AppSettings = 112,
        [EnumKey("templateOverview", "Template �bersicht")]
        TemplateOverviewUC = 1000,
        [EnumKey("templateDetail", "Template Detail")]
        TemplateDetailUC = 1001
    }
}
