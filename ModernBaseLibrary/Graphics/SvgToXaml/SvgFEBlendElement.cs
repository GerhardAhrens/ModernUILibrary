
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows.Media.Effects;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;feBlend&gt; element.
    /// </summary>
    internal class SvgFEBlendElement : SvgFilterEffectBaseElement
    {

        public SvgFEBlendElement(SvgDocument document, SvgBaseElement parent, XElement feBlendElement)
          : base(document, parent, feBlendElement)
        {
        }

        public override BitmapEffect ToBitmapEffect()
        {
            return null;
        }
    }
}
