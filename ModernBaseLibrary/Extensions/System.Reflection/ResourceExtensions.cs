/*
 * <copyright file="ResourceExtensions.cs" company="Lifeprojects.de">
 *     Class: ResourceExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for ResourceExtensions Types
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public static class ResourceExtensions
    {
        private static readonly Assembly assmbly = null;
        private static readonly string[] resourceList = null;

        static ResourceExtensions()
        {
            if (UnitTestDetector.IsInUnitTest == true)
            {
                assmbly = Assembly.GetCallingAssembly();
            }
            else
            {
                assmbly = Assembly.GetExecutingAssembly();
            }

            resourceList = assmbly.GetResourceNames().ToArray();
        }

        public static Icon GetIconFromResource(this Bitmap @this, string pictureResource)
        {
            string nameSpace = assmbly.GetName().Name;
            string resourceLocation = $"{nameSpace}.Resources.Picture.{pictureResource}";
            Stream icoResource = assmbly.GetManifestResourceStream(resourceLocation);
            if (icoResource != null)
            {
                Bitmap bmpResource = new Bitmap(icoResource);
                Icon icoForDialog = Icon.FromHandle(bmpResource.GetHicon());
                return (icoForDialog);
            }
            return (null);
        }

        public static Icon GetIconFromResource(this Assembly @this, string resourceName)
        {
            string result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(resourceName) == false)
                {
                    using (Stream iconStream = @this.GetManifestResourceStream(resourceName))
                    {
                        Bitmap bmpResource = new Bitmap(iconStream);
                        Icon icoForDialog = Icon.FromHandle(bmpResource.GetHicon());
                        return (icoForDialog);
                    }

                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return default;
        }

        public static Image GetImageFromResource(this Bitmap @this, string pictureResource)
        {
            string nameSpace = assmbly.GetName().Name;
            string resourceLocation = $"{nameSpace}.Resources.Picture.{pictureResource}";
            Stream icoResource = assmbly.GetManifestResourceStream(resourceLocation);
            if (icoResource != null)
            {
                using (Bitmap bmpResource = new Bitmap(icoResource))
                {
                    return ((Image)bmpResource);
                }
            }
            return (null);
        }

        public static Image GetImageFromResource(this Assembly @this, string resourceName)
        {
            string result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(resourceName) == false)
                {
                    using (Stream imageStream = assmbly.GetManifestResourceStream(resourceName))
                    {
                        if (imageStream != null)
                        {
                            Bitmap bmpResource = new Bitmap(imageStream);
                            return ((Image)bmpResource);
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return default;
        }

        public static byte[] GetByteFromResource(this Assembly @this, string resourceName)
        {
            string result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(resourceName) == false)
                {
                    using (Stream imageStream = @this.GetManifestResourceStream(resourceName))
                    {
                        if (imageStream != null)
                        {
                            Bitmap bmpResource = new Bitmap(imageStream);
                            return BitmapToByteArray(bmpResource);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return default;
        }

        public static string GetStringFromResource(this Assembly @this, string resourceName)
        {
            string result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(resourceName) == false)
                {
                    using (Stream stringStream = @this.GetManifestResourceStream(resourceName))
                    {
                        using (StreamReader reader = new StreamReader(stringStream))
                        {
                            result = reader.ReadToEnd();
                        }

                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return default;
        }

        public static List<string> GetResourceNames(this Assembly @this)
        {
            if (resourceList.IsNullOrEmpty() == false)
            {
                return resourceList.ToList();
            }
            else
            {
                return @this.GetManifestResourceNames().ToList();
            }
        }

        public static string GetResourceName(this Assembly @this, string resourceName)
        {
            if (resourceList.IsNullOrEmpty() == false)
            {
                return resourceList.FirstOrDefault(s => s.Contains(resourceName) == true);
            }
            else
            {
                return @this.GetManifestResourceNames().FirstOrDefault(s => s.Contains(resourceName) == true); ;
            }
        }

        private static byte[] BitmapToByteArray(Bitmap pBitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                pBitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Stream GetEmbeddedResourceStream(this Assembly assembly, string relativeResourcePath)
        {
            if (string.IsNullOrEmpty(relativeResourcePath))
            {
                throw new ArgumentNullException("relativeResourcePath");
            }

            var resourcePath = String.Format("{0}.{1}",
                Regex.Replace(assembly.ManifestModule.Name, @"\.(exe|dll)$",
                      string.Empty, RegexOptions.IgnoreCase), relativeResourcePath);

            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new ArgumentException(String.Format("The specified embedded resource \"{ 0 }\" is not found.", relativeResourcePath));
            }

            return stream;
        }

        public static Stream HasEmbeddedResource(this Assembly assembly, string relativeResourcePath)
        {
            if (string.IsNullOrEmpty(relativeResourcePath))
            {
                throw new ArgumentNullException("relativeResourcePath");
            }

            var resourcePath = String.Format("{0}.{1}", Regex.Replace(assembly.ManifestModule.Name, @"\.(exe|dll)$", string.Empty, RegexOptions.IgnoreCase), relativeResourcePath);

            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new ArgumentException(String.Format("The specified embedded resource \"{ 0 }\" is not found.", relativeResourcePath));
            }

            return stream;
        }
    }
}