//-----------------------------------------------------------------------
// <copyright file="VirtualDrive.cs" company="Lifeprojects.de">
//     Class: VirtualDrive
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.02.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core.IO
{
    using System.IO;

    public static class VirtualDrive
    {
        private const int VD_RAW_TARGET_PATH = 0x00000001;
        private const int VD_REMOVE_DEFINITION = 0x00000002;
        private const int VD_EXACT_MATCH_ON_REMOVE = 0x00000004;

        private const int DRIVE_UNKNOWN = 0;
        private const int DRIVE_NO_ROOT_DIR = 1;
        private const int DRIVE_FIXED = 3;

        /// <summary>
        /// Erstellung eines nicht persistenten Laufwerks.
        /// </summary>
        /// <param name="driveChar">Laufwerksbuchstabe.</param>
        /// <param name="path">Pfad zu dem zu verknüpfenden Ordner.</param>
        /// <returns>True/False beim Versuch das Laufwerk zu erstellen</returns>
        public static bool Create(char driveChar, string path)
        {
            bool result = false;
            bool resultDrive = VDOperation(driveChar, path, true);
            if (IsReady(driveChar) == true)
            {
                result = resultDrive;
            }

            return result;
        }

        /// <summary>
        /// Löschung eines nicht persistenten Laufwerks.
        /// </summary>
        /// <param name="driveChar">Laufwerksbuchstabe.</param>
        /// <param name="path">Pfad zu dem zu verknüpfenden Ordner.</param>
        /// <returns>True/False beim Versuch das Laufwerk zu löschen</returns>
        public static bool Delete(char driveChar, string path)
        {
            bool result = false;
            bool resultDrive = VDOperation(driveChar, path, false);
            if (IsReady(driveChar) == false)
            {
                result = resultDrive;
            }

            return result;
        }

        public static bool IsExist(char driveChar)
        {
            bool result = false;

            try
            {
                DriveInfo myDrive = new DriveInfo(driveChar.ToString());
                if (myDrive.IsReady == true)
                {
                    result = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;
        }

        private static bool IsReady(char driveChar)
        {
            bool result = false;

            try
            {
                DriveInfo myDrive = new DriveInfo(driveChar.ToString());
                if (myDrive.IsReady == true)
                {
                    result = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;
        }

        private static bool VDOperation(char driveChar, string path, bool create)
        {
            if (Directory.Exists(path) == false)
            {
                return false;
            }

            string drive = $"{driveChar.ToString().ToUpper()}:";

            int driveType = -1;

            using (VirtualDriveAPI vd = new VirtualDriveAPI())
            {
                driveType = vd.DriveType($"{drive}{Path.DirectorySeparatorChar}");
            }


            //Hinweis: Ein erstelltes virtuelles Laufwerk ist vom Typ DRIVE_FIXED
            if ((create && driveType != DRIVE_UNKNOWN && driveType != DRIVE_NO_ROOT_DIR) || (!create && driveType != DRIVE_FIXED))
            {
                return false;
            }

            int flags = VD_RAW_TARGET_PATH;

            if (create == false)
            {
                flags |= (VD_REMOVE_DEFINITION | VD_EXACT_MATCH_ON_REMOVE);
            }

            bool isDevice = false;
            using (VirtualDriveAPI vd = new VirtualDriveAPI())
            {
                isDevice = vd.CreateDevice(flags, drive, $"{Path.DirectorySeparatorChar}??{Path.DirectorySeparatorChar}{path}");
            }

            return isDevice;
        }
    }
}

