﻿
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    /// <summary>
    ///   Represents an RGB color.
    /// </summary>
    internal class SvgColor
    {
        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;

        public SvgColor(float red, float green, float blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public SvgColor(byte red, byte green, byte blue)
        {
            Red = red / 255.0f;
            Green = green / 255.0f;
            Blue = blue / 255.0f;
        }

        public Color ToColor()
        {
            return Color.FromScRgb(1, Red, Green, Blue);
        }

        public static SvgColor Parse(string value)
        {
            if (value.StartsWith("#"))
            {
                string color = value.Substring(1).Trim();
                if (color.Length == 3)
                {
                    float r = (float)(Byte.Parse(String.Format("{0}{0}", color[0]), NumberStyles.HexNumber) / 255.0);
                    float g = (float)(Byte.Parse(String.Format("{0}{0}", color[1]), NumberStyles.HexNumber) / 255.0);
                    float b = (float)(Byte.Parse(String.Format("{0}{0}", color[2]), NumberStyles.HexNumber) / 255.0);
                    return new SvgColor(r, g, b);
                }

                if (color.Length == 6)
                {
                    float r = (float)(Byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber) / 255.0);
                    float g = (float)(Byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber) / 255.0);
                    float b = (float)(Byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber) / 255.0);
                    return new SvgColor(r, g, b);
                }
            }

            if (value.StartsWith("rgb"))
            {
                string color = value.Substring(3).Trim();
                if (color.StartsWith("(") && color.EndsWith(")"))
                {
                    color = color.Substring(1, color.Length - 2).Trim();

                    string[] components = color.Split(',');
                    if (components.Length == 3)
                    {
                        float r, g, b;

                        components[0] = components[0].Trim();
                        if (components[0].EndsWith("%"))
                        {
                            components[0] = components[0].Substring(0, components[0].Length - 1).Trim();
                            r = (float)(Double.Parse(components[0], CultureInfo.InvariantCulture.NumberFormat) / 100.0);
                        }
                        else
                            r = (float)(Byte.Parse(components[0]) / 255.0);

                        components[1] = components[1].Trim();
                        if (components[1].EndsWith("%"))
                        {
                            components[1] = components[1].Substring(0, components[1].Length - 1).Trim();
                            g = (float)(Double.Parse(components[1], CultureInfo.InvariantCulture.NumberFormat) / 100.0);
                        }
                        else
                            g = (float)(Byte.Parse(components[1]) / 255.0);

                        components[2] = components[1].Trim();
                        if (components[2].EndsWith("%"))
                        {
                            components[2] = components[2].Substring(0, components[2].Length - 1).Trim();
                            b = (float)(Double.Parse(components[2], CultureInfo.InvariantCulture.NumberFormat) / 100.0);
                        }
                        else
                            b = (float)(Byte.Parse(components[2]) / 255.0);

                        return new SvgColor(r, g, b);
                    }
                }
            }

            if (value == "none")
                return null;


            switch (value)
            {
                case "black":
                    return new SvgColor((float)(0 / 255.0), (float)(0 / 255.0), (float)(0 / 255.0));
                case "green":
                    return new SvgColor((float)(0 / 255.0), (float)(128 / 255.0), (float)(0 / 255.0));
                case "silver":
                    return new SvgColor((float)(192 / 255.0), (float)(192 / 255.0), (float)(192 / 255.0));
                case "lime":
                    return new SvgColor((float)(0 / 255.0), (float)(255 / 255.0), (float)(0 / 255.0));
                case "gray":
                    return new SvgColor((float)(128 / 255.0), (float)(128 / 255.0), (float)(128 / 255.0));
                case "olive":
                    return new SvgColor((float)(128 / 255.0), (float)(128 / 255.0), (float)(0 / 255.0));
                case "white":
                    return new SvgColor((float)(255 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
                case "yellow":
                    return new SvgColor((float)(255 / 255.0), (float)(255 / 255.0), (float)(0 / 255.0));
                case "maroon":
                    return new SvgColor((float)(128 / 255.0), (float)(0 / 255.0), (float)(0 / 255.0));
                case "navy":
                    return new SvgColor((float)(0 / 255.0), (float)(0 / 255.0), (float)(128 / 255.0));
                case "red":
                    return new SvgColor((float)(255 / 255.0), (float)(0 / 255.0), (float)(0 / 255.0));
                case "blue":
                    return new SvgColor((float)(0 / 255.0), (float)(0 / 255.0), (float)(255 / 255.0));
                case "purple":
                    return new SvgColor((float)(128 / 255.0), (float)(0 / 255.0), (float)(128 / 255.0));
                case "teal":
                    return new SvgColor((float)(0 / 255.0), (float)(128 / 255.0), (float)(128 / 255.0));
                case "fuchsia":
                    return new SvgColor((float)(255 / 255.0), (float)(0 / 255.0), (float)(255 / 255.0));
                case "aqua":
                    return new SvgColor((float)(0 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            }

            throw new ArgumentException(String.Format("Unsupported color value: {0}", value));

        }

    }
}
