
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;switch&gt; element.
    /// </summary>
    internal class SvgSwitchElement : SvgContainerBaseElement
    {
        public SvgSwitchElement(SvgDocument document, SvgBaseElement parent, XElement svgElement)
          : base(document, parent, svgElement)
        {
        }
    }
}
