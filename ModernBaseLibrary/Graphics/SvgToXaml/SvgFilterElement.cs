
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Effects;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;filter&gt; element.
    /// </summary>
    internal class SvgFilterElement : SvgBaseElement
    {
        public readonly List<SvgFilterEffectBaseElement> FilterEffects = new List<SvgFilterEffectBaseElement>();

        public SvgFilterElement(SvgDocument document, SvgBaseElement parent, XElement filterElement)
          : base(document, parent, filterElement)
        {
            foreach (XElement element in from element in filterElement.Elements()
                                         where element.Name.NamespaceName == "http://www.w3.org/2000/svg"
                                         select element)
                switch (element.Name.LocalName)
                {

                    case "feGaussianBlur":
                        FilterEffects.Add(new SvgFEGaussianBlurElement(document, this, element));
                        break;

                    case "feBlend":
                        FilterEffects.Add(new SvgFEBlendElement(document, this, element));
                        break;

                    case "feColorMatrix":
                        FilterEffects.Add(new SvgFEColorMatrixElement(document, this, element));
                        break;

                    default:
                        throw new NotImplementedException(String.Format("Unhandled element: {0}", element));
                }

        }

        public BitmapEffect ToBitmapEffect()
        {
            if (Document.Options.IgnoreEffects)
                return null;

            BitmapEffectGroup bitmap_effect_group = new BitmapEffectGroup();

            foreach (SvgFilterEffectBaseElement filter_effect in FilterEffects)
            {
                BitmapEffect bitmap_effect = filter_effect.ToBitmapEffect();
                if (bitmap_effect != null)
                    bitmap_effect_group.Children.Add(bitmap_effect);
            }

            if (bitmap_effect_group.Children.Count == 0)
                return null;

            return bitmap_effect_group;
        }
    }
}
