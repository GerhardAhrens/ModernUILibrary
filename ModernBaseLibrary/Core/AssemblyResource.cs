//-----------------------------------------------------------------------
// <copyright file="AssemblyResource.cs" company="Lifeprojects.de">
//     Class: AssemblyResource
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>12.03.2015</date>
//
// <summary>Class of AssemblyResource Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Extension;

    using Size = System.Drawing.Size;

    [SupportedOSPlatform("windows")]
    public static class AssemblyResource
    {
        private const string RootResource = "Resources";
        private const string RootNamespace = "ModernBaseLibrary.Resources";
        private static Assembly assembly;
        private static readonly string myNamespace;

        static AssemblyResource()
        {
            if (UnitTestDetector.IsInUnitTest == true)
            {
                AssemblyResource.assembly = Assembly.GetEntryAssembly();
            }
            else
            {
                AssemblyResource.assembly = Assembly.GetEntryAssembly();
            }

            myNamespace = GetCurrentNamespace();
        }

        public static AssemblyLocation AssemblyResourceLocation { get; set; }

        public static string AssemblyPath
        {
            get { return Path.GetDirectoryName(assembly.Location); }
        }

        public static string AssemblyName
        {
            get { return Path.GetFileNameWithoutExtension(assembly.Location); }
        }

        public static Assembly Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }


        public static string RootResourceName { get; set; }

        public static int Count()
        {
            string[] names = AssemblyResource.assembly.GetManifestResourceNames();
            if (names.Length == 0)
            {
                return 0;
            }
            else
            {
                return names.Count();
            }
        }

        public static bool HasResource(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return false;
            }

            try
            {
                SetAssembly(AssemblyResourceLocation);

                string[] names = assembly.GetManifestResourceNames();

                int count = names.ToList().Count(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public static bool HasResource(string resourceName, AssemblyLocation assemblyResourceLocation)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return false;
            }

            try
            {
                SetAssembly(assemblyResourceLocation);

                string[] names = assembly.GetManifestResourceNames();

                int count = names.ToList().Count(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public static IList<string> GetAllResourceNames(string fileTyp, bool fullNamespace, AssemblyLocation assemblyResourceLocation)
        {
            List<string> result = null;

            if (string.IsNullOrEmpty(fileTyp) == true)
            {
                return null;
            }

            try
            {
                SetAssembly(assemblyResourceLocation);

                 result = new List<string>();

                string[] names = assembly.GetManifestResourceNames();

                names.ToList().OrderBy(p => p).ForEach(p =>
                {
                    string resNameSpace = $"{RootNamespace}";
                    if (p.Contains(resNameSpace) == true)
                    {
                        if (fullNamespace == true)
                        {
                            if (GetFileTyp(p, fileTyp) == true)
                            {
                                result.Add(p);
                            }
                        }
                        else
                        {
                            if (GetFileTyp(p, fileTyp) == true)
                            {
                                result.Add(p.Replace(resNameSpace, string.Empty));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public static IList<string> GetAllResourceNames(string fileTyp, string folder, AssemblyLocation assemblyResourceLocation)
        {
            List<string> result = null;

            if (string.IsNullOrEmpty(fileTyp) == true)
            {
                return null;
            }

            try
            {
                SetAssembly(assemblyResourceLocation);

                result = new List<string>();

                string[] names = assembly.GetManifestResourceNames();

                names.ToList().OrderBy(p => p).ForEach(p =>
                {
                    string resNameSpace = $"{folder}";
                    if (p.Contains(resNameSpace) == true)
                    {
                        if (GetFileTyp(p, fileTyp) == true)
                        {
                            result.Add(p);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public static IList<string> GetAllResourceNames(string fileTyp, bool fullNamespace)
        {
            return GetAllResourceNames(fileTyp, fullNamespace, AssemblyLocation.EntryAssembly);
        }

        public static IList<string> GetAllResourceNames(AssemblyLocation assemblyResourceLocation)
        {
            return GetAllResourceNames("*", true, assemblyResourceLocation);
        }

        public static IList<string> GetAllResourceNames(bool fullNamespace = true)
        {
            return GetAllResourceNames("*", fullNamespace, AssemblyLocation.EntryAssembly);
        }

        public static TResult GetResourceContent<TResult>(string resourceName, int iconSize = 32)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return default(TResult);
            }

            try
            {
                SetAssembly(AssemblyResourceLocation);

                string[] names = assembly.GetManifestResourceNames();
                if (names.Length  == 0)
                {
                    return default(TResult);
                }

                resourceName = names.ToList().FirstOrDefault(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);
                if (string.IsNullOrEmpty(resourceName) == true)
                {
                    return default(TResult);
                }

                Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                if (resourceStream != null)
                {
                    if(typeof(TResult) == typeof(string))
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
                        Image image = StreamToImage(resourceStream);

                        BitmapImage bitmapImage = null;
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Bmp);
                            ms.Seek(0, SeekOrigin.Begin);

                            bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                        }

                        return (TResult)(object)bitmapImage;
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
                        using (var reader = new StreamReader(resourceName))
                        {
                            var canvasStream = XamlReader.Load(reader.BaseStream) as UIElement;
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
                        return default(TResult);
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

        public static TResult GetResourceContent<TResult>(string resourceName, AssemblyLocation assemblyResourceLocation, int iconSize = 32)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return default(TResult);
            }

            try
            {
                SetAssembly(assemblyResourceLocation);

                string[] names = assembly.GetManifestResourceNames();
                if (names.Length == 0)
                {
                    return default(TResult);
                }

                resourceName = names.ToList().FirstOrDefault(p => p.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase) == true);
                if (string.IsNullOrEmpty(resourceName) == true)
                {
                    return default(TResult);
                }

                Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
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
                        Image image = StreamToImage(resourceStream);

                        BitmapImage bitmapImage = null;
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Bmp);
                            ms.Seek(0, SeekOrigin.Begin);

                            bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                        }

                        return (TResult)(object)bitmapImage;
                    }
                    else if (typeof(TResult) == typeof(BitmapSource))
                    {
                        Image image = StreamToImage(resourceStream);

                        BitmapImage bitmapImage = null;
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Bmp);
                            ms.Seek(0, SeekOrigin.Begin);

                            bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                        }

                        return (TResult)(object)bitmapImage;
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
                            var canvasStream = XamlReader.Load(reader.BaseStream) as UIElement;
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

        public static Stream GetResourceStream(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return null;
            }

            try
            {
                SetAssembly(AssemblyResourceLocation);
                string streamName = $"{RootNamespace}.{resourceName}";
                Stream stream = assembly.GetManifestResourceStream(streamName);
                if (stream != null)
                {
                    return stream;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string errortext = $"Can't load Image from {resourceName}, Error: {ex.Message}";
                throw new ArgumentException(errortext);
            }
        }


        public static Image GetImageFromIcon(string resourceName, int size = 32)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                return null;
            }

            try
            {
                assembly = Assembly.GetEntryAssembly();
                string picName = $"{myNamespace}.{resourceName}";
                Stream picStream = assembly.GetManifestResourceStream(picName);
                if (picStream != null)
                {
                    Size iconSize = new Size(size, size);
                    Icon icon = new Icon(picStream, iconSize);
                    return icon.ToBitmap();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string errortext = $"Can't load Image from {resourceName}, Error: {ex.Message}";
                throw new ArgumentException(errortext);
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

        public static byte[] ImageToByteArray(Image image, ImageFormat imageFormat)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, imageFormat);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }

        public static Image ByteArrayToImage(byte[] imageByAr)
        {
            Image img = default(Image);

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(imageByAr))
                {
                    img = Image.FromStream(memoryStream);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return img;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetCurrentNamespaceEx()
        {
            return System.Reflection.Assembly.GetCallingAssembly().EntryPoint.DeclaringType.Namespace;
        }

        private static void SetAssembly(AssemblyLocation assemblyResourceLocation)
        {
            if (assemblyResourceLocation == AssemblyLocation.CallingAssembly)
            {
                AssemblyResource.assembly = Assembly.GetCallingAssembly();
            }
            else if (assemblyResourceLocation == AssemblyLocation.EntryAssembly)
            {
                AssemblyResource.assembly = Assembly.GetEntryAssembly();
            }
            else if (assemblyResourceLocation == AssemblyLocation.ExecutingAssembly)
            {
                AssemblyResource.assembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                AssemblyResource.assembly = Assembly.GetEntryAssembly();
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

        private static string GetResourceName(string resourceName)
        {
            string fullName = $"{myNamespace}.Resources.{resourceName}";

            return fullName;
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }

        private static Stream GetStream(Assembly assembly, string resourceName)
        {
            string name = string.Concat(assembly.GetName().Name, ".", resourceName);
            Stream s = assembly.GetManifestResourceStream(name);
            return s;
        }

        private static string GetCurrentNamespace()
        {
            string currentNamespace = string.Empty;

            if (Assembly.GetEntryAssembly().EntryPoint != null)
            {
                currentNamespace = Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;
            }
            else
            {
                currentNamespace = GetRootNamespace();
            }

            return currentNamespace;
        }

        private static string GetRootNamespace()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            string ns = null;

            foreach (var frame in stackFrames)
            {
                string _ns = frame.GetMethod().DeclaringType.Namespace;
                int indexPeriod = _ns.IndexOf('.');
                string rootNs = _ns;
                if (indexPeriod > 0)
                {
                    rootNs = _ns.Substring(0, indexPeriod);
                }

                if (rootNs == "System")
                {
                    break;
                }

                ns = _ns;
            }

            return ns;
        }

    }
}