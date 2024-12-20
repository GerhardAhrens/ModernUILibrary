
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    ///   Represents a &lt;flowRoot&gt; element.
    /// </summary>
    internal class SvgFlowRootElement : SvgDrawableContainerBaseElement
    {
        public readonly SvgFlowRegionElement FlowRegion;

        public SvgFlowRootElement(SvgDocument document, SvgBaseElement parent, XElement flowRootElement)
          : base(document, parent, flowRootElement)
        {
            for (int i = 0; i < Children.Count; ++i)
            {
                FlowRegion = Children[i] as SvgFlowRegionElement;
                if (FlowRegion != null)
                {
                    Children.RemoveAt(i);
                    break;
                }
            }

            if (FlowRegion == null)
                throw new NotImplementedException();
        }

        public override Drawing Draw()
        {
            DrawingGroup drawing_group = new DrawingGroup();

            drawing_group.Opacity = Opacity.ToDouble();
            if (Transform != null)
                drawing_group.Transform = Transform.ToTransform();

            foreach (SvgBaseElement element in Children)
            {
                Drawing drawing = null;

                if (element is SvgDrawableBaseElement)
                    drawing = (element as SvgDrawableBaseElement).Draw();
                else if (element is SvgDrawableContainerBaseElement)
                    drawing = (element as SvgDrawableContainerBaseElement).Draw();

                if (drawing != null)
                    drawing_group.Children.Add(drawing);


            }

            if (Filter != null)
            {
                SvgFilterElement filter_element = Document.Elements[Filter.Id] as SvgFilterElement;
                if (filter_element != null)
                    drawing_group.BitmapEffect = filter_element.ToBitmapEffect();
            }

            if (ClipPath != null)
            {
                SvgClipPathElement clip_path_element = Document.Elements[ClipPath.Id] as SvgClipPathElement;
                if (clip_path_element != null)
                    drawing_group.ClipGeometry = clip_path_element.GetClipGeometry();
            }

            if (Mask != null)
            {
                SvgMaskElement mask_element = Document.Elements[Mask.Id] as SvgMaskElement;
                if (mask_element != null)
                {
                    drawing_group.OpacityMask = mask_element.GetOpacityMask();

                    GeometryGroup geometry_group = new GeometryGroup();

                    if (drawing_group.ClipGeometry != null)
                        geometry_group.Children.Add(drawing_group.ClipGeometry);

                    geometry_group.Children.Add(mask_element.GetClipGeometry());
                    drawing_group.ClipGeometry = geometry_group;

                }
            }

            return drawing_group;
        }
    }
}
