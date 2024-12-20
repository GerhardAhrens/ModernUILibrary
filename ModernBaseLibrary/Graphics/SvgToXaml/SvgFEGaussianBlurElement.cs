
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows.Media.Effects;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents an &lt;feGaussianBlur&gt; element.
    /// </summary>
    internal class SvgFEGaussianBlurElement : SvgFilterEffectBaseElement
    {
        public readonly SvgLength StdDeviation = new SvgLength(1.0);

        public SvgFEGaussianBlurElement(SvgDocument document, SvgBaseElement parent, XElement feGaussianBlurElement)
          : base(document, parent, feGaussianBlurElement)
        {
            XAttribute std_deviation_attribute = feGaussianBlurElement.Attribute("stdDeviation");
            if (std_deviation_attribute != null)
                StdDeviation = SvgCoordinate.Parse(std_deviation_attribute.Value);

        }

        public override BitmapEffect ToBitmapEffect()
        {
            BlurBitmapEffect blur_bitmap_effect = new BlurBitmapEffect();
            blur_bitmap_effect.Radius = StdDeviation.ToDouble();
            blur_bitmap_effect.KernelType = KernelType.Box;
            return blur_bitmap_effect;
        }
    }
}
