
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;path&gt; element.
    /// </summary>
    internal class SvgPathElement : SvgDrawableBaseElement
    {
        public readonly string Data;

        public SvgPathElement(SvgDocument document, SvgBaseElement parent, XElement pathElement)
          : base(document, parent, pathElement)
        {
            XAttribute d_attribute = pathElement.Attribute("d");
            if (d_attribute != null)
                Data = d_attribute.Value;
            else
                Data = null;
        }

        public override Geometry GetBaseGeometry()
        {
            return Geometry.Parse(Data).Clone();
        }
    }
}
