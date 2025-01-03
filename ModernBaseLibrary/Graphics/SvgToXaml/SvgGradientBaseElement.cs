﻿
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal abstract class SvgGradientBaseElement : SvgBaseElement
    {
        public readonly List<SvgStopElement> Stops = new List<SvgStopElement>();
        public readonly SvgGradientUnits GradientUnits = SvgGradientUnits.ObjectBoundingBox;
        public readonly SvgSpreadMethod SpreadMethod = SvgSpreadMethod.Pad;
        public readonly SvgTransform Transform = null;

        public SvgGradientBaseElement(SvgDocument document, SvgBaseElement parent, XElement gradientElement)
          : base(document, parent, gradientElement)
        {

            XAttribute gradient_units_attribute = gradientElement.Attribute("gradientUnits");
            if (gradient_units_attribute != null)
                switch (gradient_units_attribute.Value)
                {
                    case "objectBoundingBox":
                        GradientUnits = SvgGradientUnits.ObjectBoundingBox;
                        break;

                    case "userSpaceOnUse":
                        GradientUnits = SvgGradientUnits.UserSpaceOnUse;
                        break;

                    default:
                        throw new NotImplementedException(String.Format("gradientUnits value '{0}' is no supported", gradient_units_attribute.Value));
                }

            XAttribute gradient_transform_attribute = gradientElement.Attribute("gradientTransform");
            if (gradient_transform_attribute != null)
                Transform = SvgTransform.Parse(gradient_transform_attribute.Value);

            XAttribute spread_method_attribute = gradientElement.Attribute("spreadMethod");
            if (spread_method_attribute != null)
                switch (spread_method_attribute.Value)
                {
                    case "pad":
                        SpreadMethod = SvgSpreadMethod.Pad;
                        break;

                    case "reflect":
                        SpreadMethod = SvgSpreadMethod.Reflect;
                        break;

                    case "repeat":
                        SpreadMethod = SvgSpreadMethod.Repeat;
                        break;
                }



            foreach (XElement element in from element in gradientElement.Elements()
                                         where element.Name.NamespaceName == "http://www.w3.org/2000/svg"
                                         select element)
                switch (element.Name.LocalName)
                {
                    case "stop":
                        Stops.Add(new SvgStopElement(Document, this, element));
                        break;

                    default:
                        throw new NotImplementedException(String.Format("Unhandled element: {0}", element));
                }
        }

        protected abstract GradientBrush CreateBrush();

        protected virtual GradientBrush SetBrush(GradientBrush brush)
        {
            switch (SpreadMethod)
            {
                case SvgSpreadMethod.Pad:
                    brush.SpreadMethod = GradientSpreadMethod.Pad;
                    break;

                case SvgSpreadMethod.Reflect:
                    brush.SpreadMethod = GradientSpreadMethod.Reflect;
                    break;

                case SvgSpreadMethod.Repeat:
                    brush.SpreadMethod = GradientSpreadMethod.Repeat;
                    break;
            }

            switch (GradientUnits)
            {
                case SvgGradientUnits.ObjectBoundingBox:
                    brush.MappingMode = BrushMappingMode.RelativeToBoundingBox;
                    break;

                case SvgGradientUnits.UserSpaceOnUse:
                    brush.MappingMode = BrushMappingMode.Absolute;
                    break;
            }

            if (Transform != null)
                brush.Transform = Transform.ToTransform();

            foreach (SvgStopElement stop in Stops)
                brush.GradientStops.Add(stop.ToGradientStop());

            return brush;
        }

        public GradientBrush ToBrush()
        {
            GradientBrush brush = CreateBrush();

            if (Reference != null)
            {
                if (!Document.Elements.ContainsKey(Reference))
                    return null;

                SvgGradientBaseElement reference = Document.Elements[Reference] as SvgGradientBaseElement;
                if (reference == null)
                    throw new NotImplementedException();
                reference.SetBrush(brush);
            }
            return SetBrush(brush);
        }
    }
}
