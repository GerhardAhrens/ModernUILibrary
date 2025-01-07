//-----------------------------------------------------------------------
// <copyright file="SystemMetricsInfo.cs" company="Lifeprojects.de">
//     Class: SystemMetricsInfo
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.08.2017</date>
//
// <summary>
//      Class with SystemMetricsInfo
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Runtime.Versioning;

    using SWF = System.Windows.Forms;

    [SupportedOSPlatform("windows")]
    public class SystemMetricsInfo
    {
        private static readonly List<SWF.Screen> screenList = null;

        static SystemMetricsInfo()
        {
            screenList = new List<SWF.Screen>();
            if (screenList == null || screenList.Count == 0)
            {
                SWF.Screen[] screens = SWF.Screen.AllScreens;

                for (int i = 0; i < screens.Length; i++)
                {
                    screenList.Add(screens[i]);
                }
            }
        }

        public static int CountMonitors
        {
            get
            {
                return screenList.Count;
            }
        }

        public static List<SWF.Screen> Screens
        {
            get
            {
                return screenList;
            }
        }

        public static InfoDeviceType DetectingDeviceType()
        {
            ManagementClass systemEnclosures = new ManagementClass("Win32_SystemEnclosure");
            foreach (ManagementObject obj in systemEnclosures.GetInstances())
            {
                foreach (int i in (ushort[])obj["ChassisTypes"])
                {
                    if (i > 0 && i < 25)
                    {
                        return (InfoDeviceType)i;
                    }
                }
            }

            return InfoDeviceType.Unknown;
        }

        public static SWF.Screen PrimaryScreen()
        {
            SWF.Screen resultScreen = SystemMetricsInfo.Screens.FirstOrDefault(p => p.Primary == true);
            return resultScreen;
        }
    }
}