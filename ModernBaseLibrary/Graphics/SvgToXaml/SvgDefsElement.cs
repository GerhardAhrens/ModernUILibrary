
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;defs&gt; element.
    /// </summary>
    internal class SvgDefsElement : SvgContainerBaseElement
    {
        public SvgDefsElement(SvgDocument document, SvgBaseElement parent, XElement defsElement)
          : base(document, parent, defsElement)
        {
        }
    }
}
