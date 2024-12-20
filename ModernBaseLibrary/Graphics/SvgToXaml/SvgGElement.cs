
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;g&gt; element.
    /// </summary>
    internal class SvgGElement : SvgDrawableContainerBaseElement
    {
        public SvgGElement(SvgDocument document, SvgBaseElement parent, XElement gElement)
          : base(document, parent, gElement)
        {
        }
    }
}
