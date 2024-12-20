
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal class SvgRadialGradientElement : SvgGradientBaseElement
    {
        public readonly SvgCoordinate CX = new SvgCoordinate(0.5);
        public readonly SvgCoordinate CY = new SvgCoordinate(0.5);
        public readonly SvgLength R = new SvgCoordinate(0.5);
        public readonly SvgCoordinate FX = null;
        public readonly SvgCoordinate FY = null;

        public SvgRadialGradientElement(SvgDocument document, SvgBaseElement parent, XElement radialGradientElement)
          : base(document, parent, radialGradientElement)
        {
            XAttribute cx_attribute = radialGradientElement.Attribute("cx");
            if (cx_attribute != null)
                CX = SvgCoordinate.Parse(cx_attribute.Value);

            XAttribute cy_attribute = radialGradientElement.Attribute("cy");
            if (cy_attribute != null)
                CY = SvgCoordinate.Parse(cy_attribute.Value);

            XAttribute r_attribute = radialGradientElement.Attribute("r");
            if (r_attribute != null)
                R = SvgCoordinate.Parse(r_attribute.Value);

            XAttribute fx_attribute = radialGradientElement.Attribute("fx");
            if (fx_attribute != null)
                FX = SvgCoordinate.Parse(fx_attribute.Value);

            XAttribute fy_attribute = radialGradientElement.Attribute("fy");
            if (fy_attribute != null)
                FY = SvgCoordinate.Parse(fy_attribute.Value);

        }

        protected override GradientBrush CreateBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;
            return brush;
        }

        protected override GradientBrush SetBrush(GradientBrush brush)
        {
            RadialGradientBrush radial_gradient_brush = base.SetBrush(brush) as RadialGradientBrush;
            if (radial_gradient_brush != null)
            {
                double cx = CX.ToDouble();
                double cy = CY.ToDouble();
                double fx = (FX != null) ? FX.ToDouble() : cx;
                double fy = (FY != null) ? FY.ToDouble() : cy;

                radial_gradient_brush.GradientOrigin = new Point(fx, fy);
                radial_gradient_brush.RadiusX = R.ToDouble();
                radial_gradient_brush.RadiusY = R.ToDouble();
                radial_gradient_brush.Center = new Point(cx, cy);
            }
            return brush;
        }
    }
}
