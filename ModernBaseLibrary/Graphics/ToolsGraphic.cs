//-----------------------------------------------------------------------
// <copyright file="ToolsGraphic.cs" company="Lifeprojects.de">
//     Class: ToolsGraphic
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.02.2022</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    public class ToolsGraphic
    {
        private static readonly object _Lock = new object();
        private static ToolsGraphic instance;


        /// <summary>
        /// Die Methode gibt eine Instanz der Klasse ToolsGraphic zurück.
        /// </summary>
        /// <returns></returns>
        public static ToolsGraphic Instance()
        {
            lock (_Lock)
            {
                if (instance == null)
                {
                    instance = new ToolsGraphic();
                }
            }

            return instance;
        }

        public static BitmapSource ResizeToBitmapSource(BitmapSource source, int width, int height)
        {
            TransformedBitmap transformBitmap = new TransformedBitmap(source,
                                                      new ScaleTransform(width / source.PixelWidth,
                                                                         height / source.PixelHeight,
                                                                         0, 0));
            return transformBitmap;
        }

        /// <summary>
        /// Obtient le thumnail de l'image source (incluant la rotation)
        /// </summary>
        /// <param name="pMediaUrl"></param>
        /// <returns></returns>
        public static BitmapSource GetThumbnail(string pMediaUrl)
        {
            ExifOrientations orientation = ExifOrientations.Normal;
            BitmapSource ret = null;
            BitmapMetadata meta = null;
            double angle = 0;

            try
            {
                BitmapFrame frame = BitmapFrame.Create(
                    new Uri(pMediaUrl),
                    BitmapCreateOptions.DelayCreation,
                    BitmapCacheOption.None);

                if (frame.Thumbnail == null)
                {
                    BitmapImage image = new BitmapImage();
                    image.DecodePixelHeight = 90; 
                    image.BeginInit();
                    image.UriSource = new Uri(pMediaUrl);
                    image.CacheOption = BitmapCacheOption.None;
                    image.CreateOptions = BitmapCreateOptions.DelayCreation;
                    image.EndInit();

                    if (image.CanFreeze)
                    {
                        image.Freeze();
                    }

                    ret = image;
                }
                else
                {
                    meta = frame.Metadata as BitmapMetadata;
                    ret = frame.Thumbnail;
                }

                if ((meta != null) && (ret != null))
                {
                    if (meta.GetQuery("/app1/ifd/{ushort=274}") != null)
                    {
                        orientation = (ExifOrientations)Enum.Parse(typeof(ExifOrientations), meta.GetQuery("/app1/ifd/{ushort=274}").ToString());
                    }

                    switch (orientation)
                    {
                        case ExifOrientations.Rotate90:
                            angle = -90;
                            break;
                        case ExifOrientations.Rotate180:
                            angle = 180;
                            break;
                        case ExifOrientations.Rotate270:
                            angle = 90;
                            break;
                    }

                    if (angle != 0)
                    {
                        ret = new TransformedBitmap(ret.Clone(), new RotateTransform(angle));
                        ret.Freeze();
                    }
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
            }

            return ret;
        }

        public static BitmapImage LoadBitmapImage(string imageAbsolutePath,
                                                 BitmapCacheOption bitmapCacheOption = BitmapCacheOption.None)
        {
            BitmapImage image = new BitmapImage();

            if (File.Exists(imageAbsolutePath) == true)
            {
                image.BeginInit();
                image.CacheOption = bitmapCacheOption;
                image.UriSource = new Uri(imageAbsolutePath);
                image.EndInit();
            }

            return image;
        }

        public static int HexColorToInt(string pHexColor)
        {
            int result = 0;
            result = Convert.ToInt32(pHexColor.Substring(1), 16);

            return result;
        }

        public static System.Drawing.Color IntToColor(int pIntColor)
        {
            System.Drawing.Color result = System.Drawing.Color.FromArgb(pIntColor);
            return result;
        }

        public static string IntToHexColor(int pIntColor)
        {
            string result = string.Empty;
            if (pIntColor == 0 || pIntColor == 16777215)
            {
                result = "00FFFFFF";
            }
            else
            {
                result = System.Drawing.Color.FromArgb(pIntColor).Name;
            }

            return string.Format("#{0}", result);
        }

        public static byte[] ImageToByteArray(System.Drawing.Image pBitmapSource, System.Drawing.Imaging.ImageFormat pImageFormat)
        {
            MemoryStream memStream = new MemoryStream();
            pBitmapSource.Save(memStream, pImageFormat);
            memStream.Flush();
            return memStream.ToArray();
        }

        public static byte[] BitmapSorceToByteArray(BitmapSource pBitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
            encoder.QualityLevel = 100;
            byte[] bit = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();
            }

            return bit;
        }

        public static Bitmap ByteArrayToBitmap(byte[] pSource)
        {
            ImageConverter ic = new ImageConverter();
            System.Drawing.Image img = (System.Drawing.Image)ic.ConvertFrom(pSource);
            Bitmap bitmap = new Bitmap(img);
            return bitmap;
        }

        public static BitmapSource ByteArrayToBitmapSource(byte[] pSource)
        {
            var stream = new MemoryStream(pSource);
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream);
        }

        public static System.Drawing.Image LoadImageNoLock(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path)); 
            return System.Drawing.Image.FromStream(ms);
        }

        public static BitmapImage ImageSourceToBitmapImage(ImageSource pImageSource)
        {
            BitmapImage retVal = null;

            retVal = new BitmapImage(new Uri(pImageSource.ToString()));

            return retVal;
        }

        public static BitmapFrame ResizedToBitmapFrame(ImageSource source, int width, int height, int margin)
        {
            var rect = new System.Windows.Rect(margin, margin, width - (margin * 2), height - (margin * 2));

            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawDrawing(group);
            }

            var resizedImage = new RenderTargetBitmap(width, height,  96, 96, PixelFormats.Default); 
            resizedImage.Render(drawingVisual);

            return BitmapFrame.Create(resizedImage);
        }

        /// <summary>
        /// Converting text to image (png).
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void CreateImageFromText(string text, Font font, System.Drawing.Color textColor, int maxWidth, string path, ImageFormat imageFormat)
        {
            Bitmap img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, maxWidth);

            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            if (imageFormat != ImageFormat.Png)
            {
                // Get the color of a background pixel.
                System.Drawing.Color backColor = img.GetPixel(1, 1);
                img.MakeTransparent(backColor);
            }
            else
            {
                img.MakeTransparent(System.Drawing.Color.Transparent);
            }

            drawing = Graphics.FromImage(img);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(System.Drawing.Color.Transparent);
            System.Drawing.Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, imageFormat);
            img.Dispose();
        }

        /// <summary>
        /// Converting text to image.
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void CreateImageFromText(string text, Font font, System.Drawing.Color textColor, int maxWidth, string path)
        {
            Bitmap img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, maxWidth);

            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            img.MakeTransparent(System.Drawing.Color.Transparent);

            drawing = Graphics.FromImage(img);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(System.Drawing.Color.Transparent);
            System.Drawing.Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);
            img.Dispose();
        }

        /// <summary>
        /// Converting text to image (png).
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void CreateImageFromText(string text, Font font, int maxWidth, string path)
        {
            System.Drawing.Color fontColor = System.Drawing.Color.Black;

            Bitmap img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, maxWidth);

            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            img.MakeTransparent(System.Drawing.Color.Transparent);

            drawing = Graphics.FromImage(img);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(System.Drawing.Color.Transparent);
            System.Drawing.Brush textBrush = new SolidBrush(fontColor);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);
            img.Dispose();
        }

        /// <summary>
        /// Converting text to image (png).
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void CreateImageFromText(string text, float fontSize, int maxWidth, string path)
        {
            System.Drawing.Color fontColor = System.Drawing.Color.Black;
            Font font = new Font("Minion", fontSize);

            Bitmap img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, maxWidth);

            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            img.MakeTransparent(System.Drawing.Color.Transparent);

            drawing = Graphics.FromImage(img);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(System.Drawing.Color.Transparent);
            System.Drawing.Brush textBrush = new SolidBrush(fontColor);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);
            img.Dispose();
        }

        public Bitmap MakeTransparent(Bitmap bitmap, System.Drawing.Color color, int tolerance)
        {
            Bitmap transparentImage = new Bitmap(bitmap);

            for (int i = transparentImage.Size.Width - 1; i >= 0; i--)
            {
                for (int j = transparentImage.Size.Height - 1; j >= 0; j--)
                {
                    var currentColor = transparentImage.GetPixel(i, j);
                    if (Math.Abs(color.R - currentColor.R) < tolerance &&
                         Math.Abs(color.G - currentColor.G) < tolerance &&
                         Math.Abs(color.B - currentColor.B) < tolerance)
                        transparentImage.SetPixel(i, j, color);
                }
            }

            transparentImage.MakeTransparent(color);
            return transparentImage;
        }

        public Size GetSizeFromStream(Stream imageStream)
        {
            Bitmap bitmap = new Bitmap(imageStream);
            return new Size(bitmap.Width, bitmap.Height);
        }

        public byte[] CreateThumbnail(byte[] passedImage, int maxValue)
        {
            byte[] returnedThumbnail;

            using (MemoryStream startMemoryStream = new MemoryStream(), newMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                startMemoryStream.Write(passedImage, 0, passedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(startMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = maxValue;
                    HW_ratio = (double)((double)maxValue / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = maxValue;
                    HW_ratio = (double)((double)maxValue / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(newMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                returnedThumbnail = newMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return returnedThumbnail;
        }

        private Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }

        public static void SaveImage(BitmapSource source, string fileName)
        {
            BitmapEncoder encoder;
            string lowerExtension = Path.GetExtension(fileName).ToLower();
            if (lowerExtension.Contains("jpeg") || lowerExtension.Contains("jpg"))
            {
                encoder = new JpegBitmapEncoder();
            }
            else if (lowerExtension.Contains("png"))
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                return;
            }

            encoder.Frames.Add(BitmapFrame.Create(source));
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                encoder.Save(fileStream);
            }
        }
    }
}