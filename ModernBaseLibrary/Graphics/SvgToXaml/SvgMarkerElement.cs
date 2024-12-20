
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;marker&gt; element.
    /// </summary>
    internal class SvgMarkerElement : SvgContainerBaseElement
    {
        public SvgMarkerElement(SvgDocument document, SvgBaseElement parent, XElement markerElement)
          : base(document, parent, markerElement)
        {
        }
    }
}
