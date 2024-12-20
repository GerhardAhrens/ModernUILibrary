
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;svg&gt; element.
    /// </summary>
    internal class SvgSVGElement : SvgDrawableContainerBaseElement
    {
        public SvgSVGElement(SvgDocument document, SvgBaseElement parent, XElement svgElement)
          : base(document, parent, svgElement)
        {
        }
    }
}
