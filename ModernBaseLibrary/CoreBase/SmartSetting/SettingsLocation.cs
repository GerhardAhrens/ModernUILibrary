//-----------------------------------------------------------------------
// <copyright file="SettingsLocation.cs" company="Lifeprojects.de">
//     Class: SettingsLocation
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2017</date>
//
// <summary>
// Enum zur festlegung der Speicherorts von Einstellungem (Applikation, User)
//</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.CoreBase
{
    using System.ComponentModel;

    public enum SettingsLocation
    {
        [Description("No Location, default is 1")]
        None = 0,
        [Description("Settings are stored in the assembly directory")]
        AssemblyLocation = 1,
        [Description("Settings are stored in the ProgramData directory")]
        ProgramData = 2,
        [Description("Settings are stored in the Custom selected directory")]
        CustomLocation = 3
    }
}
