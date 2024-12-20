//-----------------------------------------------------------------------
// <copyright file="TextImageGenerator.cs" company="Lifeprojects.de">
//     Class: TextImageGenerator
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.02.2023</date>
//
// <summary>
// Erstellen von Images (aus inem Text) zum speichern in einer Datei
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class TextImageGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextImageGenerator"/> class.
        /// </summary>
        public TextImageGenerator(Color textColor, Color backgroundColor, string font, int padding, int fontSize)
        {
            this.TextColor = textColor;
            this.BackgroundColor = backgroundColor;
            this.Font = new Font(font, fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Padding = padding;
            this.FontSize = fontSize;
        }
        public TextImageGenerator() : this(Color.Black, Color.White, "Arial", 20, 20)
        { 
        }

        private Color TextColor { get; set; }

        private Color BackgroundColor { get; set; }

        private Font Font { get; set; }

        private int Padding { get; set; }

        private int FontSize { get; set; }

        public Bitmap CreateBitmap(string text)
        {
            // Create graphics for rendering 
            Graphics retBitmapGraphics = Graphics.FromImage(new Bitmap(1, 1));
            // measure needed width for the image
            var bitmapWidth = (int)retBitmapGraphics.MeasureString(text, this.Font).Width;
            // measure needed height for the image
            var bitmapHeight = (int)retBitmapGraphics.MeasureString(text, this.Font).Height;
            // Create the bitmap with the correct size and add padding
            Bitmap retBitmap = new Bitmap(bitmapWidth + Padding, bitmapHeight + Padding);
            retBitmap.MakeTransparent(retBitmap.GetPixel(1, 1));


            // Add the colors to the new bitmap.
            retBitmapGraphics = Graphics.FromImage(retBitmap);
            // Set Background color
            retBitmapGraphics.Clear(this.BackgroundColor);
            retBitmapGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            retBitmapGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            retBitmapGraphics.DrawString(text, Font, new SolidBrush(this.TextColor), Padding / 2, Padding / 2);
            // flush the changes
            retBitmapGraphics.Flush();

            return (retBitmap);
        }
        public string CreateBase64Image(string text, ImageFormat imageFormat)
        {
            var bitmap = CreateBitmap(text);
            var stream = new System.IO.MemoryStream();
            // save into stream
            bitmap.Save(stream, imageFormat);
            // convert to byte array
            var imageBytes = stream.ToArray();
            // convert to base64 string
            return Convert.ToBase64String(imageBytes);
        }

        public void SaveAsJpg(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Jpeg);
        }

        public void SaveAsPng(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Png);
        }

        public void SaveAsGif(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Gif);
        }

        public void SaveAsBmp(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Bmp);
        }
    }
}
