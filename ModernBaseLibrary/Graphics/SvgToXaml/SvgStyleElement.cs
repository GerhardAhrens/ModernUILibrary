
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;style&gt; element.
    /// </summary>
    internal class SvgStyleElement : SvgBaseElement
    {

        public SvgStyleElement(SvgDocument document, SvgBaseElement parent, XElement styleElement)
          : base(document, parent, styleElement)
        {
        }
    }
}
