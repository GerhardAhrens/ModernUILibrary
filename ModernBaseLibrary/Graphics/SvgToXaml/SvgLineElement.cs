
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;line&gt; element.
    /// </summary>
    class SvgLineElement : SvgDrawableBaseElement
    {
        public readonly SvgCoordinate X1 = new SvgCoordinate(0.0);
        public readonly SvgCoordinate Y1 = new SvgCoordinate(0.0);
        public readonly SvgCoordinate X2 = new SvgCoordinate(0.0);
        public readonly SvgCoordinate Y2 = new SvgCoordinate(0.0);

        public SvgLineElement(SvgDocument document, SvgBaseElement parent, XElement lineElement)
          : base(document, parent, lineElement)
        {
            XAttribute x1_attribute = lineElement.Attribute("x1");
            if (x1_attribute != null)
                X1 = SvgCoordinate.Parse(x1_attribute.Value);

            XAttribute y1_attribute = lineElement.Attribute("y1");
            if (y1_attribute != null)
                Y1 = SvgCoordinate.Parse(y1_attribute.Value);

            XAttribute x2_attribute = lineElement.Attribute("x2");
            if (x2_attribute != null)
                X2 = SvgCoordinate.Parse(x2_attribute.Value);

            XAttribute y2_attribute = lineElement.Attribute("y2");
            if (y2_attribute != null)
                Y2 = SvgCoordinate.Parse(y2_attribute.Value);
        }

        public override Geometry GetBaseGeometry()
        {
            return new LineGeometry(new Point(X1.ToDouble(), Y1.ToDouble()),
                                    new Point(X2.ToDouble(), Y2.ToDouble()));
        }
    }
}
