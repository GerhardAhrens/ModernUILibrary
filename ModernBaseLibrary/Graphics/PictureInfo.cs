//-----------------------------------------------------------------------
// <copyright file="PictureInfo.cs" company="Lifeprojects.de">
//     Class: PictureInfo
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.06.2023 10:51:47</date>
//
// <summary>
// Ermitteln des Picture Types
// </summary>
// <Website>
// https://en.wikipedia.org/wiki/List_of_file_signatures
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System.Collections.Generic;
    using System;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class PictureInfo
    {
        private static readonly List<byte> jpg = new List<byte> { 0xFF, 0xD8 };
        private static readonly List<byte> bmp = new List<byte> { 0x42, 0x4D };
        private static readonly List<byte> gif = new List<byte> { 0x47, 0x49, 0x46 };
        private static readonly List<byte> png = new List<byte> { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly List<byte> svgXmlSmall = new List<byte> { 0x3C, 0x3F, 0x78, 0x6D, 0x6C }; // "<?xml"
        private static readonly List<byte> svgXmlCapital = new List<byte> { 0x3C, 0x3F, 0x58, 0x4D, 0x4C }; // "<?XML"
        private static readonly List<byte> svgSmall = new List<byte> { 0x3C, 0x73, 0x76, 0x67 }; // "<svg"
        private static readonly List<byte> svgCapital = new List<byte> { 0x3C, 0x53, 0x56, 0x47 }; // "<SVG"
        private static readonly List<byte> intelTiff = new List<byte> { 0x49, 0x49, 0x2A, 0x00 };
        private static readonly List<byte> motorolaTiff = new List<byte> { 0x4D, 0x4D, 0x00, 0x2A };
        private static readonly List<byte> icon = new List<byte> { 0x00, 0x00, 0x01, 0x00 };

        private static readonly List<(List<byte> magic, string extension)> imageFormats = new List<(List<byte> magic, string extension)>()
    {
        (jpg, "jpg"),
        (bmp, "bmp"),
        (gif, "gif"),
        (png, "png"),
        (svgSmall, "svg"),
        (svgCapital, "svg"),
        (intelTiff,"tif"),
        (motorolaTiff, "tif"),
        (svgXmlSmall, "svg"),
        (svgXmlCapital, "svg"),
        (icon, "ico")
    };

        public static string TryGetExtension(Byte[] array)
        {
            // check for simple formats first
            foreach (var imageFormat in imageFormats)
            {
                if (array.IsImage(imageFormat.magic))
                {
                    if (imageFormat.magic != svgXmlSmall && imageFormat.magic != svgXmlCapital)
                        return imageFormat.extension;

                    // special handling for SVGs starting with XML tag
                    int readCount = imageFormat.magic.Count; // skip XML tag
                    int maxReadCount = 1024;

                    do
                    {
                        if (array.IsImage(svgSmall, readCount) || array.IsImage(svgCapital, readCount))
                        {
                            return imageFormat.extension;
                        }
                        readCount++;
                    }
                    while (readCount < maxReadCount && readCount < array.Length - 1);

                    return null;
                }
            }
            return null;
        }

        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = fileNameOrExtension.Contains(".") ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private static bool IsImage(this Byte[] array, List<byte> comparer, int offset = 0)
        {
            int arrayIndex = offset;
            foreach (byte c in comparer)
            {
                if (arrayIndex > array.Length - 1 || array[arrayIndex] != c)
                    return false;
                ++arrayIndex;
            }
            return true;
        }
    }
}
