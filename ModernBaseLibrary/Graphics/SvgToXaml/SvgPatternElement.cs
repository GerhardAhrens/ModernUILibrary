
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;pattern&gt; element.
    /// </summary>
    internal class SvgPatternElement : SvgDrawableContainerBaseElement
    {
        public readonly SvgTransform PatternTransform = null;
        public readonly SvgPatternUnits PatternUnits = SvgPatternUnits.ObjectBoundingBox;
        public readonly SvgLength Width = null;
        public readonly SvgLength Height = null;

        public SvgPatternElement(SvgDocument document, SvgBaseElement parent, XElement patternElement)
          : base(document, parent, patternElement)
        {
            XAttribute pattern_transform_attribute = patternElement.Attribute("patternTransform");
            if (pattern_transform_attribute != null)
                PatternTransform = SvgTransform.Parse(pattern_transform_attribute.Value);

            XAttribute pattern_units_attribute = patternElement.Attribute("patternUnits");
            if (pattern_units_attribute != null)
                switch (pattern_units_attribute.Value)
                {
                    case "objectBoundingBox":
                        PatternUnits = SvgPatternUnits.ObjectBoundingBox;
                        break;

                    case "userSpaceOnUse":
                        PatternUnits = SvgPatternUnits.UserSpaceOnUse;
                        break;

                    default:
                        throw new NotImplementedException(String.Format("patternUnits value '{0}' is no supported", pattern_units_attribute.Value));
                }

            XAttribute width_attribute = patternElement.Attribute("width");
            if (width_attribute != null)
                Width = SvgLength.Parse(width_attribute.Value);

            XAttribute height_attribute = patternElement.Attribute("height");
            if (height_attribute != null)
                Height = SvgLength.Parse(height_attribute.Value);

        }

        //==========================================================================
        public DrawingBrush ToBrush()
        {
            DrawingBrush brush = null;

            if (Reference == null)
                brush = new DrawingBrush(Draw());
            else
            {
                SvgBaseElement element = Document.Elements[Reference];
                if (element is SvgPatternElement)
                    brush = (element as SvgPatternElement).ToBrush();
                else
                    throw new NotSupportedException("Other references than patterns are not supported");
            }

            if (brush == null)
                return null;

            if ((Width != null) || (Height != null))
            {
                double width = brush.Drawing.Bounds.Width;
                double height = brush.Drawing.Bounds.Height;
            }

            if (PatternUnits == SvgPatternUnits.UserSpaceOnUse)
            {
                brush.ViewportUnits = BrushMappingMode.Absolute;
                brush.Viewport = brush.Drawing.Bounds;
            }

            if (PatternTransform != null)
            {
                DrawingGroup drawing_group = new DrawingGroup();
                drawing_group.Transform = PatternTransform.ToTransform();
                drawing_group.Children.Add(brush.Drawing);
                brush.Drawing = drawing_group;
            }

            return brush;
        }
    }
}
