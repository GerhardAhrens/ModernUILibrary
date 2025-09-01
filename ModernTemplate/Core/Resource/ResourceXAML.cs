//-----------------------------------------------------------------------
// <copyright file="ResourceXAML.cs" company="Lifeprojects.de">
//     Class: ResourceXAML
//     Copyright � Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>14.05.2025 11:06:49</date>
//
// <summary>
// Die Klasse ResourceXAML Of T liest einen Wert aus dem Resource-File 'ApplicationResource_DE.xaml' aus und gibt diesen f�r den gew�nschten Typ zur�ck.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ModernTemplate.Core.Resource;

    public static class ResourceXAML
    {
        private static readonly Application app;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceXAML"/> class.
        /// </summary>
        static ResourceXAML()
        {
            app = Application.Current;
        }

        public static T GetAs<T>(string resourceName)
        {
            T result = default(T);

            try
            {
                if (typeof(T) == typeof(string))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)find;
                    }
                }
                else if (typeof(T) == typeof(int))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(int));
                    }
                }
                else if (typeof(T) == typeof(double))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(double));
                    }
                }
                else if (typeof(T) == typeof(bool))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(bool));
                    }
                }
                else if (typeof(T) == typeof(Brush))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(Brush));
                    }
                }
                else if (typeof(T) == typeof(Color))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(Color));
                    }
                }
                else if (typeof(T) == typeof(SolidColorBrush))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Convert.ChangeType(find, typeof(SolidColorBrush));
                    }
                }
                else if (typeof(T) == typeof(System.Drawing.Bitmap))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        BitmapImage image = (BitmapImage)Convert.ChangeType(find, typeof(BitmapImage));
                        result = (T)Convert.ChangeType(BitmapImageToBitmap(image),typeof(System.Drawing.Bitmap));
                    }
                }
                else if (typeof(T) == typeof(DialogResource))
                {
                    object find = app.TryFindResource(resourceName);
                    if (find != null)
                    {
                        result = (T)Activator.CreateInstance(typeof(DialogResource), find.ToString());
                    }
                    else
                    {
                        result = (T)Activator.CreateInstance(typeof(DialogResource), resourceName);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        private static System.Drawing.Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }
    }
}
