//-----------------------------------------------------------------------
// <copyright file="FileExtension.cs" company="Lifeprojects.de">
//     Class: FileExtension
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>10.04.2025</date>
//
// <summary>
// Klasse für File Extension
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Extension
{
    using System.IO;

    public static class FileExtension
    {
        /// <summary>
        /// Converts to bytearray.
        /// </summary>
        /// <param name="pFilepath">The p filepath.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] FileToByteArray(this string pFilepath)
        {
            if (!File.Exists(pFilepath))
                return null;
            var imageData = File.ReadAllBytes(pFilepath);
            return imageData;
        }
    }
}