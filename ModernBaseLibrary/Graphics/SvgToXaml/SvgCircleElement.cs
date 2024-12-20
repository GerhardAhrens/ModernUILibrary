
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;circle&gt; element.
    /// </summary>
    internal class SvgCircleElement : SvgDrawableBaseElement
    {
        /// <summary>
        ///   The x-coordinate of the circle's center.
        /// </summary>
        public readonly SvgCoordinate CenterX = new SvgCoordinate(0);

        /// <summary>
        ///   The y-coordinate of the circle's center.
        /// </summary>
        public readonly SvgCoordinate CenterY = new SvgCoordinate(0);

        /// <summary>
        ///   The circle's radius.
        /// </summary>
        public readonly SvgLength Radius = new SvgLength(0);

        public SvgCircleElement(SvgDocument document, SvgBaseElement parent, XElement circleElement)
          : base(document, parent, circleElement)
        {
            XAttribute cx_attribute = circleElement.Attribute("cx");
            if (cx_attribute != null)
                CenterX = SvgCoordinate.Parse(cx_attribute.Value);

            XAttribute cy_attribute = circleElement.Attribute("cy");
            if (cy_attribute != null)
                CenterY = SvgCoordinate.Parse(cy_attribute.Value);

            XAttribute r_attribute = circleElement.Attribute("r");
            if (r_attribute != null)
                Radius = SvgLength.Parse(r_attribute.Value);
        }

        public override Geometry GetBaseGeometry()
        {
            return new EllipseGeometry(new Point(CenterX.ToDouble(), CenterY.ToDouble()),
                                       Radius.ToDouble(), Radius.ToDouble());
        }

    }
}
