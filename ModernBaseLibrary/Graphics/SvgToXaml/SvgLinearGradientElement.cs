
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;linearGradient&gt; element.
    /// </summary>
    internal class SvgLinearGradientElement : SvgGradientBaseElement
    {
        public readonly SvgCoordinate X1 = new SvgCoordinate(0);
        public readonly SvgCoordinate Y1 = new SvgCoordinate(0);
        public readonly SvgCoordinate X2 = new SvgCoordinate(1);
        public readonly SvgCoordinate Y2 = new SvgCoordinate(0);

        public SvgLinearGradientElement(SvgDocument document, SvgBaseElement parent, XElement linearGradientElement)
          : base(document, parent, linearGradientElement)
        {
            XAttribute x1_attribute = linearGradientElement.Attribute("x1");
            if (x1_attribute != null)
                X1 = SvgCoordinate.Parse(x1_attribute.Value);

            XAttribute y1_attribute = linearGradientElement.Attribute("y1");
            if (y1_attribute != null)
                Y1 = SvgCoordinate.Parse(y1_attribute.Value);

            XAttribute x2_attribute = linearGradientElement.Attribute("x2");
            if (x2_attribute != null)
                X2 = SvgCoordinate.Parse(x2_attribute.Value);

            XAttribute y2_attribute = linearGradientElement.Attribute("y2");
            if (y2_attribute != null)
                Y2 = SvgCoordinate.Parse(y2_attribute.Value);
        }

        protected override GradientBrush CreateBrush()
        {
            return new LinearGradientBrush();
        }

        protected override GradientBrush SetBrush(GradientBrush brush)
        {
            LinearGradientBrush linear_gradient_brush = base.SetBrush(brush) as LinearGradientBrush;
            if (linear_gradient_brush != null)
            {
                linear_gradient_brush.StartPoint = new Point(X1.ToDouble(), Y1.ToDouble());
                linear_gradient_brush.EndPoint = new Point(X2.ToDouble(), Y2.ToDouble());
            }
            return brush;
        }
    }
}
