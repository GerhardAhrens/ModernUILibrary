
//-----------------------------------------------------------------------
// <copyright file="SystemMessageBox.cs" company="Lifeprojects.de">
//     Class: SystemMessageBox
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.01.2022</date>
//
// <summary>
// Klasse zum anzeigen einer MessageBox die über die Windows API erzeugt wird
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Win32API
{
    public enum MsgOptions : uint
    {
        MB_ABORTRETRYIGNORE = 0x00000002,
        MB_CANCELTRYCONTINUE = 0x00000006,
        MB_HELP = 0x00004000,
        MB_OK = 0x00000000,
        MB_OKCANCEL = 0x00000001,
        MB_ICONEXCLAMATION = 0x00000030,
        MB_ICONWARNING = 0x00000030,
        MB_ICONINFORMATION = 0x00000040,
        MB_ICONASTERISK = 0x00000040,
        MB_ICONQUESTION = 0x00000020
    }

}
