//-----------------------------------------------------------------------
// <copyright file="ByteArrayToImageSourceConverter.cs" company="Lifeprojects.de">
//     Class: ByteArrayToImageSourceConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.01.2017</date>
//
// <summary>
//      WPF Converter um einen bool-Wert eine Farbe zuzuweisen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public sealed class ByteArrayToImageSourceConverter : ConverterBase<byte[], ImageSource>
    {
        protected override ImageSource Convert(byte[] value, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    ImageSource bmp = this.ImageFromBuffer((byte[])value);
                    return bmp;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        private BitmapImage ImageFromBuffer(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}