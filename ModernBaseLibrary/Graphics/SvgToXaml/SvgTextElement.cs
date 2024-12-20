
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;text&gt; element.
    /// </summary>
    internal class SvgTextElement : SvgDrawableContainerBaseElement
    {

        public SvgTextElement(SvgDocument document, SvgBaseElement parent, XElement svgElement)
          : base(document, parent, svgElement)
        {
        }
    }
}
