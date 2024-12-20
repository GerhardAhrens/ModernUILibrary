
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;

    internal class SvgPoint
    {

        public readonly double X;
        public readonly double Y;

        public SvgPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }
}
