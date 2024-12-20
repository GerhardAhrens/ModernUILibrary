
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;ellipse&gt; element.
    /// </summary>
    internal class SvgEllipseElement : SvgDrawableBaseElement
    {
        public readonly SvgCoordinate CenterX = new SvgCoordinate(0.0);
        public readonly SvgCoordinate CenterY = new SvgCoordinate(0.0);
        public readonly SvgLength RadiusX = new SvgLength(0.0);
        public readonly SvgLength RadiusY = new SvgLength(0.0);

        public SvgEllipseElement(SvgDocument document, SvgBaseElement parent, XElement ellipseElement)
          : base(document, parent, ellipseElement)
        {
            XAttribute cx_attribute = ellipseElement.Attribute("cx");
            if (cx_attribute != null)
                CenterX = SvgCoordinate.Parse(cx_attribute.Value);

            XAttribute cy_attribute = ellipseElement.Attribute("cy");
            if (cy_attribute != null)
                CenterY = SvgCoordinate.Parse(cy_attribute.Value);

            XAttribute rx_attribute = ellipseElement.Attribute("rx");
            if (rx_attribute != null)
                RadiusX = SvgCoordinate.Parse(rx_attribute.Value);

            XAttribute ry_attribute = ellipseElement.Attribute("ry");
            if (ry_attribute != null)
                RadiusY = SvgCoordinate.Parse(ry_attribute.Value);
        }

        public override Geometry GetBaseGeometry()
        {
            return new EllipseGeometry(
              new Point(CenterX.ToDouble(), CenterY.ToDouble()), RadiusX.ToDouble(),
              RadiusY.ToDouble());
        }
    }
}
