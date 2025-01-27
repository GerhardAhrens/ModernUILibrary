//-----------------------------------------------------------------------
// <copyright file="BitmapToImageSourceConverter.cs" company="Lifeprojects.de">
//     Class: BitmapToImageSourceConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.03.2019</date>
//
// <summary>
// Converter Class for Bitmap to ImageSource Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
    public class BitmapToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Bitmap bmp = value as Bitmap;
            if (bmp == null)
            {
                return null;
            }

            using (Stream str = new MemoryStream())
            {
                bmp.Save(str, System.Drawing.Imaging.ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return bdc.Frames[0];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
