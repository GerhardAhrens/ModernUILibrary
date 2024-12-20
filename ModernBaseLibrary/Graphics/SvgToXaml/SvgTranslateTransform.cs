
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    internal class SvgTranslateTransform : SvgTransform
    {
        public readonly double X;
        public readonly double Y;

        public SvgTranslateTransform(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override Transform ToTransform()
        {
            return new TranslateTransform(X, Y);
        }

        public static new SvgTranslateTransform Parse(string transform)
        {
            string[] tokens = transform.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new FormatException("A translate transformation must have two values");

            return new SvgTranslateTransform(Double.Parse(tokens[0].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                             Double.Parse(tokens[1].Trim(), CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}
