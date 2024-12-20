
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;flowRegíon&gt; element.
    /// </summary>
    internal class SvgFlowRegionElement : SvgDrawableContainerBaseElement
    {

        public SvgFlowRegionElement(SvgDocument document, SvgBaseElement parent, XElement flowRegionElement)
          : base(document, parent, flowRegionElement)
        {
        }

        public Geometry GetClipGeometry()
        {
            GeometryGroup geometry_group = new GeometryGroup();

            foreach (SvgBaseElement element in Children)
            {
                if (element is SvgDrawableBaseElement)
                {
                    Geometry geometry = (element as SvgDrawableBaseElement).GetBaseGeometry();
                    if (geometry != null)
                        geometry_group.Children.Add(geometry);
                }
                else
                    throw new NotImplementedException();
            }

            return geometry_group;
        }
    }
}
