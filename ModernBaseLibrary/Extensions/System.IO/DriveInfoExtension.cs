//-----------------------------------------------------------------------
// <copyright file="DriveInfoExtension.cs" company="Lifeprojects.de">
//     Class: DriveInfoExtension
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2018</date>
//
// <summary>Extension Class for DriveInfo</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.IO;

    public static class DriveInfoExtension
    {
        public static double TotalFreeSpaceFormatted(this DriveInfo @this, DiskSizeUnit sizeUnit) {
            double freeSpace = -1;
            double formatDivideBy = 1;

            if (@this != null) {
                long freeSpaceNative = @this.TotalFreeSpace;
                formatDivideBy = Math.Pow(1024, (int)sizeUnit);

                freeSpace = freeSpaceNative / formatDivideBy;
            }

            return freeSpace;
        }

        public static double AvailableFreeSpaceFormatted(this DriveInfo @this, DiskSizeUnit sizeUnit) {
            double freeSpace = -1;
            double formatDivideBy = 1;

            if (@this != null) {
                long freeSpaceNative = @this.AvailableFreeSpace;
                formatDivideBy = Math.Pow(1024, (int)sizeUnit);

                freeSpace = freeSpaceNative / formatDivideBy;
            }

            return freeSpace;
        }
    }

    public enum DiskSizeUnit {
        Bytes = 0,
        KiloBytes = 1,
        MegaBytes = 2,
        GigaBytes = 3,
        TeraBytes = 4
    }
}
