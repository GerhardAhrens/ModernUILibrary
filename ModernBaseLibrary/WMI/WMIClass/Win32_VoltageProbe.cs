//-----------------------------------------------------------------------
// <copyright file="MyClass.cs" company="Lifeprojects.de">
//     Class: MyClass
//     Copyright © Lifeprojects.de yyyy
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse für Win32_VoltageProbe
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.WMI.WMIClass
{
    using System;

    public sealed class Win32_VoltageProbe
    {
        public Int32 Accuracy { get; set; }

        public UInt16 Availability { get; set; }

        public string Caption { get; set; }

        public UInt32 ConfigManagerErrorCode { get; set; }

        public bool ConfigManagerUserConfig { get; set; }

        public string CreationClassName { get; set; }

        public Int32 CurrentReading { get; set; }

        public string Description { get; set; }

        public string DeviceID { get; set; }

        public bool ErrorCleared { get; set; }

        public string ErrorDescription { get; set; }

        public DateTime InstallDate { get; set; }

        public bool IsLinear { get; set; }

        public UInt32 LastErrorCode { get; set; }

        public Int32 LowerThresholdCritical { get; set; }

        public Int32 LowerThresholdFatal { get; set; }

        public Int32 LowerThresholdNonCritical { get; set; }

        public Int32 MaxReadable { get; set; }

        public Int32 MinReadable { get; set; }

        public string Name { get; set; }

        public Int32 NominalReading { get; set; }

        public Int32 NormalMax { get; set; }

        public Int32 NormalMin { get; set; }

        public string PNPDeviceID { get; set; }

        public UInt16[] PowerManagementCapabilities { get; set; }

        public bool PowerManagementSupported { get; set; }

        public UInt32 Resolution { get; set; }

        public string Status { get; set; }

        public UInt16 StatusInfo { get; set; }

        public string SystemCreationClassName { get; set; }

        public string SystemName { get; set; }

        public Int32 Tolerance { get; set; }

        public Int32 UpperThresholdCritical { get; set; }

        public Int32 UpperThresholdFatal { get; set; }

        public Int32 UpperThresholdNonCritical { get; set; }
    }
}
