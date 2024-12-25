//-----------------------------------------------------------------------
// <copyright file="WmiExtensions.cs" company="Lifeprojects.de">
//     Class: WmiExtensions
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.01.2013</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Extension
{
    using System.Management;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class WmiExtensions
    {
        public static ManagementObject First (this ManagementObjectSearcher searcher)
        {
            ManagementObject result = null;
            foreach (ManagementObject item in searcher.Get())
            {
                result = item;
                break;
            }

            return result;
        }
    }
}
