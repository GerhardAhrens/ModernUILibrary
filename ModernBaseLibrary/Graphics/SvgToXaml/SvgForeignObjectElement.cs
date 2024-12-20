
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;foreignObject&gt; element.
    /// </summary>
    internal class SvgForeignObjectElement : SvgDrawableContainerBaseElement
    {

        public SvgForeignObjectElement(SvgDocument document, SvgBaseElement parent, XElement foreignObjectElement)
          : base(document, parent, foreignObjectElement)
        {
        }
    }
}
