
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows.Media.Effects;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;filterEffect&gt; element.
    /// </summary>
   internal abstract class SvgFilterEffectBaseElement : SvgBaseElement
    {
        public SvgFilterEffectBaseElement(SvgDocument document, SvgBaseElement parent, XElement filterEffectElement)
          : base(document, parent, filterEffectElement)
        {
        }

        public abstract BitmapEffect ToBitmapEffect();
    }
}
