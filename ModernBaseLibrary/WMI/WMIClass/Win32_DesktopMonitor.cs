//-----------------------------------------------------------------------
// <copyright file="Win32_DesktopMonitor.cs" company="Lifeprojects.de">
//     Class: Win32_DesktopMonitor
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>31.03.2022</date>
//
// <summary>
// Die Klasse stellt ein Objekt für die Klasse WMIQuery zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.WMI.WMIClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class Win32_DesktopMonitor
    {
        public UInt16 Availability { get; set; }

        public UInt32 Bandwidth { get; set; }

        public string Caption { get; set; }

        public UInt32 ConfigManagerErrorCode { get; set; }

        public bool ConfigManagerUserConfig { get; set; }

        public string CreationClassName { get; set; }

        public string Description { get; set; }

        public string DeviceID { get; set; }

        public UInt16 DisplayType { get; set; }

        public bool ErrorCleared { get; set; }

        public string ErrorDescription { get; set; }

        public DateTime InstallDate { get; set; }

        public bool IsLocked { get; set; }

        public UInt32 LastErrorCode { get; set; }

        public string MonitorManufacturer { get; set; }

        public string MonitorType { get; set; }

        public string Name { get; set; }

        public UInt32 PixelsPerXLogicalInch { get; set; }

        public UInt32 PixelsPerYLogicalInch { get; set; }

        public string PNPDeviceID { get; set; }

        public UInt16 PowerManagementCapabilities { get; set; }

        public bool PowerManagementSupported { get; set; }

        public UInt32 ScreenHeight { get; set; }

        public UInt32 ScreenWidth { get; set; }

        public string Status { get; set; }

        public UInt16 StatusInfo { get; set; }

        public string SystemCreationClassName { get; set; }

        public string SystemName { get; set; }
    }
}
