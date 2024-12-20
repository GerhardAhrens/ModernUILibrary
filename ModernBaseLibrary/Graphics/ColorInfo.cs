//-----------------------------------------------------------------------
// <copyright file="ColorInfo.cs" company="Lifeprojects.de">
//     Class: ColorInfo
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.08.2017</date>
//
// <summary>
// Klasse mit Methoden zur Konvertierung von Color-Werten
// https://www.99colors.net/dot-net-colors
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows.Media;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public sealed class ColorInfo
    {

        public ColorInfo(string colorName)
        {
            this.ColorName = colorName;
            var cv = typeof(Colors).GetProperties().Select(s => new InternalColorInfo(s.Name, (Color)s.GetValue(null, null))).FirstOrDefault(f => f.Name == colorName);
            if (cv != null)
            {
                this.ColorValue = cv.Value;
            }
            else
            {
                this.ColorValue = Colors.Transparent;
            }
        }

        public ColorInfo(Color colorName)
        {
            this.ColorValue = colorName;
            var cv = typeof(Colors).GetProperties().Select(s => new InternalColorInfo(s.Name, (Color)s.GetValue(null, null))).FirstOrDefault(f => f.Value == colorName);
            if (cv != null)
            {
                this.ColorName = cv.Name;
            }
            else
            {
                this.ColorName = "Unknown Color";
            }
        }

        public ColorInfo(Brush colorName)
        {
            var cv = typeof(Brushes).GetProperties().Select(s => new InternalBrushInfo(s.Name, (Brush)s.GetValue(null, null))).FirstOrDefault(f => f.Value == colorName);
            if (cv != null)
            {
                this.ColorName = cv.Name;
                this.ColorValue = (cv.Value as SolidColorBrush).Color;
            }
            else
            {
                this.ColorName = "Unknown Color";
                this.ColorValue = Colors.Transparent;
            }
        }

        public string ColorName { get; set; }

        public Color ColorValue { get; set; }

        public Color ToColor
        {
            get { return (Color)this.ColorValue; }
        }

        public SolidColorBrush ToSolidColorBrush
        {
            get {
                BrushConverter conv = new BrushConverter();
                SolidColorBrush brush = conv.ConvertFromString(this.ColorName) as SolidColorBrush;
                return brush;
            }
        }

        public string ToHexValue
        {
            get { return ColorValue.ToString(); }
        }

        public string ToRGBValue
        {
            get { return this.ColorToRGB((Color)ColorValue); }
        }

        public int ToIntValue
        {
            get { return this.ColorToInt((Color)ColorValue); }
        }

        public string ToColorName
        {
            get { return this.ColorName; }
        }

        private string ColorToRGB(Color color)
        {
            string colorAsString = string.Empty;

            try
            {
                string rgbValue = $"{color.R.ToString()},{color.G.ToString()},{color.B.ToString()}";
                colorAsString = $"RGB(" + rgbValue + ")";
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
            }

            return colorAsString;
        }

        private int ColorToInt(Color color, bool withAlphaChannel = true)
        {
            string colorAsString = string.Empty;
            try
            {
                if (withAlphaChannel == true)
                {
                    colorAsString = BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, color.A }, 0).ToString();
                }
                else
                {
                    colorAsString = BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, 0x00 }, 0).ToString();
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
            }

            return colorAsString.ToInt();
        }

        [DebuggerDisplay("Name={Name}")]
        internal class InternalColorInfo
        {
            public InternalColorInfo(string namen, Color value)
            {
                this.Name = namen;
                this.Value = value;
            }

            public string Name { get; set; }

            public Color Value { get; set; }
        }

        [DebuggerDisplay("Name={Name}")]
        internal class InternalBrushInfo
        {
            public InternalBrushInfo(string namen, Brush value)
            {
                this.Name = namen;
                this.Value = value;
            }

            public string Name { get; set; }

            public Brush Value { get; set; }
        }
    }
}
