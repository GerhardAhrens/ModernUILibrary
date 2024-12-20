namespace ModernBaseLibrary.Graphics.SVG
{
    /// <summary>
    ///   A coordinate.
    /// </summary>
    internal class SvgCoordinate : SvgLength
    {

        public SvgCoordinate(double value)
          : base(value)
        {
            // ...
        }

        public SvgCoordinate(double value, string unit)
          : base(value, unit)
        {
            // ...
        }

        public static new SvgCoordinate Parse(string value)
        {
            SvgLength length = SvgLength.Parse(value);

            return new SvgCoordinate(length.Value, length.Unit);

        }
    }
}
