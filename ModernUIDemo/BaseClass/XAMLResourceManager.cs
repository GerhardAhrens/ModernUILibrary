//-----------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Lifeprojects.de">
//     Class: ResourceManager
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.02.2023</date>
//
// <summary>
// Klasse zum Verwalten von Resource
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    public class XAMLResourceManager
    {
        private static Assembly assembly;

        static XAMLResourceManager()
        {
        }

        public static int Count(AssemblyLocation AssemblyLocation = AssemblyLocation.ExecutingAssembly)
        {
            SetAssembly(AssemblyLocation);
            string[] names = assembly.GetManifestResourceNames();
            return names.Count();
        }

        public static bool HasResource(string resourceName, AssemblyLocation AssemblyLocation = AssemblyLocation.ExecutingAssembly)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return false;
            }

            try
            {
                SetAssembly(AssemblyLocation);

                string[] names = assembly.GetManifestResourceNames();

                int count = names.ToList().Count(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);

                return count > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static TResult GetResourceContent<TResult>(string resourceName, AssemblyLocation assemblyResourceLocation, int iconSize)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return default(TResult);
            }

            try
            {
                SetAssembly(assemblyResourceLocation);
                string[] names = assembly.GetManifestResourceNames();
                string resFullName = names.SingleOrDefault(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);
                Stream resourceStream = assembly.GetManifestResourceStream(resFullName);
                if (resourceStream != null)
                {
                    if (typeof(TResult) == typeof(string))
                    {
                        string streamResult = string.Empty;
                        streamResult = StreamToString(resourceStream);
                        return (TResult)Convert.ChangeType(streamResult, typeof(TResult));
                    }
                    else if (typeof(TResult) == typeof(Image))
                    {
                        Image image = StreamToImage(resourceStream);
                        return (TResult)(object)image;
                    }
                    else if (typeof(TResult) == typeof(ImageSource))
                    {
                        byte[] contentByteArray = new byte[resourceStream.Length];
                        if (contentByteArray != null)
                        {
                            resourceStream.Read(contentByteArray, 0, contentByteArray.Length);
                            ImageSource image = ByteToImageSource(contentByteArray);
                            return (TResult)(object)image;

                        }
                    }
                    else if (typeof(TResult) == typeof(Icon))
                    {
                        Image image = StreamToImage(resourceStream);
                        if (image != null)
                        {
                            Icon icon = CreateIconFromImage(image, iconSize, false);
                            return (TResult)(object)icon;
                        }
                    }
                    else if (typeof(TResult) == typeof(System.Windows.Controls.Canvas))
                    {
                        System.Windows.Controls.Canvas resCanvas;
                        using (var reader = new StreamReader(resourceStream))
                        {
                            var canvasStream = XamlReader.Load(reader.BaseStream) as System.Windows.UIElement;
                            resCanvas = canvasStream as System.Windows.Controls.Canvas;
                        }

                        return (TResult)(object)resCanvas;
                    }
                    else if (typeof(TResult) == typeof(byte[]))
                    {
                        byte[] contentByteArray = new byte[resourceStream.Length];
                        if (contentByteArray != null)
                        {
                            resourceStream.Read(contentByteArray, 0, contentByteArray.Length);
                            return (TResult)(object)contentByteArray;
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Der angegebene Typ '{typeof(TResult).Name}' wird nicht unterstützt.");
                    }
                }
                else
                {
                    return default(TResult);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return default(TResult);
        }

        public static TResult GetResourceContent<TResult>(string resourceName, AssemblyLocation assemblyResourceLocation = AssemblyLocation.ExecutingAssembly)
        {
            return XAMLResourceManager.GetResourceContent<TResult>(resourceName, assemblyResourceLocation, 32);
        }

        private static void SetAssembly(AssemblyLocation assemblyResourceLocation)
        {
            if (assemblyResourceLocation == AssemblyLocation.CallingAssembly)
            {
                XAMLResourceManager.assembly = Assembly.GetCallingAssembly();
            }
            else if (assemblyResourceLocation == AssemblyLocation.EntryAssembly)
            {
                XAMLResourceManager.assembly = Assembly.GetEntryAssembly();
            }
            else if (assemblyResourceLocation == AssemblyLocation.ExecutingAssembly)
            {
                XAMLResourceManager.assembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                XAMLResourceManager.assembly = Assembly.GetEntryAssembly();
            }
        }

        public static Icon CreateIconFromImage(Image img, int size, bool keepAspectRatio)
        {
            Bitmap square = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(square);

            int x, y, w, h;

            try
            {
                if (!keepAspectRatio || img.Height == img.Width)
                {
                    x = y = 0;
                    w = h = size;
                }
                else
                {
                    float r = (float)img.Width / (float)img.Height;

                    if (r > 1)
                    {
                        w = size;
                        h = (int)((float)size / r);
                        x = 0;
                        y = (size - h) / 2;
                    }
                    else
                    {
                        w = (int)((float)size * r);
                        h = size;
                        y = 0;
                        x = (size - w) / 2;
                    }
                }

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, x, y, w, h);
                g.Flush();
                return Icon.FromHandle(square.GetHicon());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Icon StreamToIcon(Stream resourceStream, int size = 32)
        {
            if (resourceStream == null)
            {
                return null;
            }

            Size iconSize = new Size(size, size);
            Icon icon = new Icon(resourceStream, iconSize);

            return icon;
        }

        private static Image StreamToImage(Stream resourceStream)
        {
            if (resourceStream == null)
            {
                return null;
            }

            return Image.FromStream(resourceStream);
        }

        private static string StreamToString(Stream resourceStream)
        {
            if (resourceStream == null)
            {
                return null;
            }

            string streamResult = string.Empty;
            using (StreamReader reader = new StreamReader(resourceStream))
            {
                streamResult = reader.ReadToEnd();
            }

            return streamResult;
        }

        private static ImageSource ByteToImageSource(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        private static bool GetFileTyp(string resourceName, string fileTyp = "*")
        {
            bool result = false;

            if (fileTyp == "*")
            {
                result = true;
            }
            else
            {
                if (resourceName.ToLower().EndsWith(fileTyp.ToLower()) == true)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
