//-----------------------------------------------------------------------
// <copyright file="VirtualDriveAPI.cs" company="Lifeprojects.de">
//     Class: VirtualDriveAPI
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.02.2022</date>
//
// <summary>
// Klasse zum Kapseln von API Funktionen der Kernel32.dll
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class VirtualDriveAPI : DisposableCoreBase
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool DefineDosDevice(int dwFlags, string lpDeviceName, string lpTargetPath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetDriveType(string lpRootPathName);

        static VirtualDriveAPI()
        {
            try
            {
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }
        }

        public bool CreateDevice(int dwFlags, string lpDeviceName, string lpTargetPath)
        {
            bool result = false;

            try
            {
                result = DefineDosDevice(dwFlags, lpDeviceName, lpTargetPath);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return result;
        }

        public int DriveType(string lpRootPathName)
        {
            int result = -1;

            try
            {
                result = GetDriveType(lpRootPathName);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return result;
        }
    }
}
