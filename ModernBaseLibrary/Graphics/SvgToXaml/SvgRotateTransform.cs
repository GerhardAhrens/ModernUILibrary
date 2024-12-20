
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    internal class SvgRotateTransform : SvgTransform
    {
        public readonly double Angle;

        public SvgRotateTransform(double angle)
        {
            this.Angle = angle;
        }

        public override Transform ToTransform()
        {
            return new RotateTransform(this.Angle);
        }

        public static new SvgRotateTransform Parse(string transform)
        {
            string[] tokens = transform.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 1)
                return new SvgRotateTransform(Double.Parse(tokens[0].Trim(), CultureInfo.InvariantCulture.NumberFormat));

            throw new NotSupportedException();
        }
    }
}
