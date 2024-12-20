
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    internal class SvgMatrixTransform : SvgTransform
    {
        public readonly double M11;
        public readonly double M12;
        public readonly double M21;
        public readonly double M22;
        public readonly double OffsetX;
        public readonly double OffsetY;

        public SvgMatrixTransform(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public override Transform ToTransform()
        {
            return new MatrixTransform(M11, M12, M21, M22, OffsetX, OffsetY);
        }

        public static new SvgMatrixTransform Parse(string transform)
        {
            string[] tokens = transform.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 6)
                return new SvgMatrixTransform(Double.Parse(tokens[0].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                              Double.Parse(tokens[1].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                              Double.Parse(tokens[2].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                              Double.Parse(tokens[3].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                              Double.Parse(tokens[4].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                              Double.Parse(tokens[5].Trim(), CultureInfo.InvariantCulture.NumberFormat));

            throw new ArgumentException();
        }
    }
}
