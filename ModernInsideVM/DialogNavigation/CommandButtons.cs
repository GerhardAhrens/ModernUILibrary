namespace ModernInsideVM.Core
{
    using System.ComponentModel;

    public enum CommandButtons : int
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Home Dialog ohne Funktion")]
        Home = 1,
        [Description("Dialog A")]
        DialogA = 2,
        [Description("Dialog B")]
        DialogB = 3,
        [Description("Anwendung beenden")]
        CloseApp = 100,
    }
}
