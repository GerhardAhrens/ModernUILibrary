
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;flowSpan&gt; element.
    /// </summary>
    internal class SvgFlowSpanElement : SvgDrawableContainerBaseElement
    {

        public SvgFlowSpanElement(SvgDocument document, SvgBaseElement parent, XElement flowSpanElement)
          : base(document, parent, flowSpanElement)
        {
        }
    }
}
