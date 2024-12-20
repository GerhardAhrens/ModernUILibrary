
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;rect&gt; element.
    /// </summary>
    internal class SvgRectElement : SvgDrawableBaseElement
    {
        public readonly SvgCoordinate X = new SvgCoordinate(0.0);
        public readonly SvgCoordinate Y = new SvgCoordinate(0.0);
        public readonly SvgLength Width = new SvgLength(0.0);
        public readonly SvgLength Height = new SvgLength(0.0);
        public readonly SvgLength CornerRadiusX = new SvgLength(0.0);
        public readonly SvgLength CornerRadiusY = new SvgLength(0.0);

        public SvgRectElement(SvgDocument document, SvgBaseElement parent, XElement rectElement)
          : base(document, parent, rectElement)
        {
            XAttribute x_attribute = rectElement.Attribute("x");
            if (x_attribute != null)
                X = SvgCoordinate.Parse(x_attribute.Value);

            XAttribute y_attribute = rectElement.Attribute("y");
            if (y_attribute != null)
                Y = SvgCoordinate.Parse(y_attribute.Value);

            XAttribute width_attribute = rectElement.Attribute("width");
            if (width_attribute != null)
                Width = SvgLength.Parse(width_attribute.Value);

            XAttribute height_attribute = rectElement.Attribute("height");
            if (height_attribute != null)
                Height = SvgLength.Parse(height_attribute.Value);

            XAttribute rx_attribute = rectElement.Attribute("rx");
            if (rx_attribute != null)
                CornerRadiusX = SvgCoordinate.Parse(rx_attribute.Value);

            XAttribute ry_attribute = rectElement.Attribute("ry");
            if (ry_attribute != null)
                CornerRadiusY = SvgCoordinate.Parse(ry_attribute.Value);
        }

        public override Geometry GetBaseGeometry()
        {
            return new RectangleGeometry(new Rect(new Point(X.ToDouble(), Y.ToDouble()),
                                         new Size(Width.ToDouble(), Height.ToDouble())),
                                         CornerRadiusX.ToDouble(),
                                         CornerRadiusY.ToDouble());
        }

    }
}
