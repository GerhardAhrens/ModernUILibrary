
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    internal class SvgSkewTransform : SvgTransform
    {
        public readonly double AngleX;
        public readonly double AngleY;

        public SvgSkewTransform(double angleX, double angleY)
        {
            AngleX = angleX;
            AngleY = angleY;
        }

        public override Transform ToTransform()
        {
            return new SkewTransform(AngleX, AngleY);
        }

        public static new SvgSkewTransform Parse(string transform)
        {
            string[] tokens = transform.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new FormatException("A skew transformation must have two values");

            return new SvgSkewTransform(Double.Parse(tokens[0].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        Double.Parse(tokens[1].Trim(), CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}
