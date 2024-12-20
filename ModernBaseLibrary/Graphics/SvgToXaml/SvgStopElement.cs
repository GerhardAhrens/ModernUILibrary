
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal class SvgStopElement : SvgBaseElement
    {
        public readonly SvgLength Offset = new SvgLength(0);
        public readonly SvgColor Color = new SvgColor(0, 0, 0);
        public readonly SvgLength Opacity = new SvgLength(1);

        public SvgStopElement(SvgDocument document, SvgBaseElement parent, XElement stopElement)
          : base(document, parent, stopElement)
        {
            XAttribute offset_attribute = stopElement.Attribute("offset");
            if (offset_attribute != null)
                Offset = SvgLength.Parse(offset_attribute.Value);

            XAttribute stop_color_attribute = stopElement.Attribute("stop-color");
            if (stop_color_attribute != null)
                Color = SvgColor.Parse(stop_color_attribute.Value);

            XAttribute stop_opacity_attribute = stopElement.Attribute("stop-opacity");
            if (stop_opacity_attribute != null)
                Opacity = SvgLength.Parse(stop_opacity_attribute.Value);
        }

        public GradientStop ToGradientStop()
        {
            Color color = Color.ToColor();
            color.A = (byte)Math.Round(Opacity.ToDouble() * 255);

            GradientStop stop = new GradientStop();
            stop.Color = color;
            stop.Offset = Offset.ToDouble();

            return stop;
        }
    }
}
