
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal sealed class SvgDocument
    {
        public readonly Dictionary<string, SvgBaseElement> Elements = new Dictionary<string, SvgBaseElement>();

        public readonly SvgSVGElement Root;
        public readonly SvgReaderOptions Options;

        public SvgDocument(XElement root, SvgReaderOptions options)
        {
            Root = new SvgSVGElement(this, null, root);
            Options = options;
        }

        public DrawingImage Draw()
        {
            return new DrawingImage(Root.Draw());
        }
    }
}
