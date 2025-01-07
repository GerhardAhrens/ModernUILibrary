//-----------------------------------------------------------------------
// <copyright file="WMIQueryTyp.cs" company="Lifeprojects.de">
//     Class: WMIQueryTyp
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>21.09.2021</date>
//
// <summary>
// Enum mit einer Liste von WMI Query Typen
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.WMI
{
    using System.ComponentModel;

    public enum WMIQueryTyp : int
    {
        [Description("Unbekannt")]
        None = 0,
        [Description("Display/Monitor")]
        Win32_DesktopMonitor = 1,
        [Description("Display/Monitor")]
        WmiMonitorBasicDisplayParams = 2,
        [Description("Display/Monitor")]
        WmiMonitorID = 3,
        [Description("Processor")]
        Win32_Processor = 10,
        [Description("OperatingSystem")]
        Win32_OperatingSystem = 11,
        [Description("ComputerSystem")]
        Win32_ComputerSystem = 12,
        [Description("ComputerSystem")]
        Win32_BIOS = 13,
        [Description("ComputerSystem")]
        Win32_LogicalDisk = 14,
        [Description("LocalTime")]
        Win32_LocalTime = 15,
        [Description("ComputerSystem")]
        Win32_Battery = 16,
        [Description("ComputerSystem")]
        Win32_CurrentProbe = 17,
        [Description("ComputerSystem")]
        Win32_VoltageProbe = 18,
    }
}
