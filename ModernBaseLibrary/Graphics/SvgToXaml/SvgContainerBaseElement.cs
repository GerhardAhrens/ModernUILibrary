namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    ///   Base element for all container elements.
    /// </summary>
    internal class SvgContainerBaseElement : SvgBaseElement
    {
        public readonly List<SvgBaseElement> Children = new List<SvgBaseElement>();

        public SvgContainerBaseElement(SvgDocument document, SvgBaseElement parent, XElement containerElement)
          : base(document, parent, containerElement)
        {
            foreach (XElement element in from element in containerElement.Elements()
                                         where element.Name.NamespaceName == "http://www.w3.org/2000/svg"
                                         select element)
                switch (element.Name.LocalName)
                {
                    case "svg":
                        Children.Add(new SvgSVGElement(document, this, element));
                        break;

                    case "g":
                        Children.Add(new SvgGElement(document, this, element));
                        break;

                    case "defs":
                        Children.Add(new SvgDefsElement(document, this, element));
                        break;

                    case "symbol":
                        Children.Add(new SvgSymbolElement(document, this, element));
                        break;

                    case "clipPath":
                        Children.Add(new SvgClipPathElement(document, this, element));
                        break;

                    case "mask":
                        Children.Add(new SvgMaskElement(document, this, element));
                        break;

                    case "pattern":
                        Children.Add(new SvgPatternElement(document, this, element));
                        break;

                    case "marker":
                        Children.Add(new SvgMarkerElement(document, this, element));
                        break;

                    case "a":
                        Children.Add(new SvgAElement(document, this, element));
                        break;

                    case "switch":
                        Children.Add(new SvgSwitchElement(document, this, element));
                        break;

                    case "path":
                        Children.Add(new SvgPathElement(document, this, element));
                        break;

                    case "text":
                        Children.Add(new SvgTextElement(document, this, element));
                        break;

                    case "rect":
                        Children.Add(new SvgRectElement(document, this, element));
                        break;

                    case "circle":
                        Children.Add(new SvgCircleElement(document, this, element));
                        break;

                    case "ellipse":
                        Children.Add(new SvgEllipseElement(document, this, element));
                        break;

                    case "line":
                        Children.Add(new SvgLineElement(document, this, element));
                        break;

                    case "polyline":
                        Children.Add(new SvgPolylineElement(document, this, element));
                        break;

                    case "polygon":
                        Children.Add(new SvgPolygonElement(document, this, element));
                        break;

                    case "image":
                        Children.Add(new SvgImageElement(document, this, element));
                        break;

                    case "use":
                        Children.Add(new SvgUseElement(document, this, element));
                        break;

                    case "linearGradient":
                        Children.Add(new SvgLinearGradientElement(document, this, element));
                        break;

                    case "radialGradient":
                        Children.Add(new SvgRadialGradientElement(document, this, element));
                        break;

                    case "filter":
                        Children.Add(new SvgFilterElement(document, this, element));
                        break;

                    case "metadata":
                        Children.Add(new SvgMetadataElement(document, this, element));
                        break;

                    case "flowRoot":
                        Children.Add(new SvgFlowRootElement(document, this, element));
                        break;

                    case "flowRegion":
                        Children.Add(new SvgFlowRegionElement(document, this, element));
                        break;

                    case "flowPara":
                        Children.Add(new SvgFlowParaElement(document, this, element));
                        break;

                    case "flowSpan":
                        Children.Add(new SvgFlowSpanElement(document, this, element));
                        break;

                    case "tspan":
                        Children.Add(new SvgTSpanElement(document, this, element));
                        break;

                    case "foreignObject":
                        Children.Add(new SvgForeignObjectElement(document, this, element));
                        break;

                    case "style":
                        Children.Add(new SvgStyleElement(document, this, element));
                        break;

                    case "title":
                        Children.Add(new SvgStyleElement(document, this, element));
                        break;

                    case "desc":
                        Children.Add(new SvgStyleElement(document, this, element));
                        break;

                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Unhandled element: {0}; {1}", element, element.Name.LocalName));
                }
        }
    }
}
