
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Collections.Generic;
    using System.Windows.Media;

    class SvgTransformGroup : SvgTransform
    {
        public readonly List<SvgTransform> Transforms = new List<SvgTransform>();

        public SvgTransformGroup(SvgTransform[] transforms)
        {
            foreach (SvgTransform transform in transforms)
                Transforms.Add(transform);
        }

        public override Transform ToTransform()
        {
            TransformGroup transform_group = new TransformGroup();

            foreach (SvgTransform transform in Transforms)
                transform_group.Children.Add(transform.ToTransform());

            return transform_group;
        }
    }
}
