//-----------------------------------------------------------------------
// <copyright file="SvgDrawableContainerBaseElement.cs" company="Lifeprojects.de">
//     Class: SvgDrawableContainerBaseElement
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.10.2023</date>
//
// <summary>
// Die Klasse gehört zur Funktion "SvgToXaml",
// eine SVG-Vektor-Grafik für XAML nutzbar zu machen.
// </summary>
// <remark>
// Die Klasse wurde ursprünglich vom
// Copyright (C) 2009,2011 Boris Richter <himself@boris-richter.net>
// erstellt, und von mir für NET 8 überarbeitet und angepasst.
// </remark>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal class SvgDrawableContainerBaseElement : SvgContainerBaseElement
    {
        public readonly SvgLength Opacity = new SvgLength(1.0);
        public readonly SvgTransform Transform;
        public readonly SvgURL ClipPath = null;
        public readonly SvgURL Filter = null;
        public readonly SvgURL Mask = null;
        public readonly SvgDisplay Display = SvgDisplay.Inline;

        public SvgDrawableContainerBaseElement(SvgDocument document, SvgBaseElement parent, XElement drawableContainerElement)
          : base(document, parent, drawableContainerElement)
        {
            XAttribute opacity_attribute = drawableContainerElement.Attribute("opacity");
            if (opacity_attribute != null)
            {
                Opacity = SvgLength.Parse(opacity_attribute.Value);
            }

            XAttribute transform_attribute = drawableContainerElement.Attribute("transform");
            if (transform_attribute != null)
            {
                Transform = SvgTransform.Parse(transform_attribute.Value);
            }

            XAttribute clip_attribute = drawableContainerElement.Attribute("clip-path");
            if (clip_attribute != null)
            {
                ClipPath = SvgURL.Parse(clip_attribute.Value);
            }

            XAttribute filter_attribute = drawableContainerElement.Attribute("filter");
            if (filter_attribute != null)
            {
                Filter = SvgURL.Parse(filter_attribute.Value);
            }

            XAttribute mask_attribute = drawableContainerElement.Attribute("mask");
            if (mask_attribute != null)
            {
                Mask = SvgURL.Parse(mask_attribute.Value);
            }

            XAttribute display_attribute = drawableContainerElement.Attribute("display");
            if (display_attribute != null)
                switch (display_attribute.Value)
                {
                    case "inline":
                        Display = SvgDisplay.Inline;
                        break;

                    case "block":
                        Display = SvgDisplay.Block;
                        break;

                    case "list-item":
                        Display = SvgDisplay.ListItem;
                        break;

                    case "run-in":
                        Display = SvgDisplay.RunIn;
                        break;

                    case "compact":
                        Display = SvgDisplay.Compact;
                        break;

                    case "marker":
                        Display = SvgDisplay.Marker;
                        break;

                    case "table":
                        Display = SvgDisplay.Table;
                        break;

                    case "inline-table":
                        Display = SvgDisplay.InlineTable;
                        break;

                    case "table-row-group":
                        Display = SvgDisplay.TableRowGroup;
                        break;

                    case "table-header-group":
                        Display = SvgDisplay.TableHeaderGroup;
                        break;

                    case "table-footer-group":
                        Display = SvgDisplay.TableFooterGroup;
                        break;

                    case "table-row":
                        Display = SvgDisplay.TableRow;
                        break;

                    case "table-column-group":
                        Display = SvgDisplay.TableColumnGroup;
                        break;

                    case "table-column":
                        Display = SvgDisplay.TableColumn;
                        break;

                    case "table-cell":
                        Display = SvgDisplay.TableCell;
                        break;

                    case "table-caption":
                        Display = SvgDisplay.TableCaption;
                        break;

                    case "none":
                        Display = SvgDisplay.None;
                        break;

                    default:
                        throw new NotImplementedException();
                }

        }

        public virtual Geometry GetGeometry()
        {
            GeometryGroup geometry_group = new GeometryGroup();

            foreach (SvgBaseElement element in Children)
            {
                if (element is SvgDrawableBaseElement)
                {
                    geometry_group.Children.Add((element as SvgDrawableBaseElement).GetGeometry());
                }
                else if (element is SvgDrawableContainerBaseElement)
                {
                    geometry_group.Children.Add((element as SvgDrawableContainerBaseElement).GetGeometry());
                }
            }

            if (Transform != null)
            {
                geometry_group.Transform = Transform.ToTransform();
            }

            if (ClipPath != null)
            {
                SvgClipPathElement clip_path_element = Document.Elements[ClipPath.Id] as SvgClipPathElement;
                if (clip_path_element != null)
                {
                    return Geometry.Combine(geometry_group, clip_path_element.GetClipGeometry(), GeometryCombineMode.Exclude, null);
                }
            }

            return geometry_group;
        }

        public virtual Drawing Draw()
        {
            DrawingGroup drawing_group = new DrawingGroup();

            drawing_group.Opacity = Opacity.ToDouble();
            if (Transform != null)
            {
                drawing_group.Transform = Transform.ToTransform();
            }

            foreach (SvgBaseElement child_element in Children)
            {
                SvgBaseElement element = child_element;
                if (element is SvgUseElement)
                    element = (element as SvgUseElement).GetElement();

                Drawing drawing = null;

                if (element is SvgDrawableBaseElement)
                {
                    if ((element as SvgDrawableBaseElement).Display != SvgDisplay.None)
                    {
                        drawing = (element as SvgDrawableBaseElement).Draw();
                    }
                }
                else if (element is SvgDrawableContainerBaseElement)
                {
                    if ((element as SvgDrawableContainerBaseElement).Display != SvgDisplay.None)
                    {
                        drawing = (element as SvgDrawableContainerBaseElement).Draw();
                    }
                }

                if (drawing != null)
                {
                    drawing_group.Children.Add(drawing);
                }
            }

            if (Filter != null)
            {
                SvgFilterElement filter_element = Document.Elements[Filter.Id] as SvgFilterElement;
                if (filter_element != null)
                {
                    drawing_group.BitmapEffect = filter_element.ToBitmapEffect();
                }
            }

            if (ClipPath != null)
            {
                SvgClipPathElement clip_path_element = Document.Elements[ClipPath.Id] as SvgClipPathElement;
                if (clip_path_element != null)
                {
                    drawing_group.ClipGeometry = clip_path_element.GetClipGeometry();
                }
            }

            if (Mask != null)
            {
                SvgMaskElement mask_element = Document.Elements[Mask.Id] as SvgMaskElement;
                if (mask_element != null)
                {
                    DrawingBrush opacity_mask = mask_element.GetOpacityMask();
                    /*
                    if(Transform != null)
                      opacity_mask.Transform = Transform.ToTransform();
                    */
                    drawing_group.OpacityMask = opacity_mask;
                }
            }

            return drawing_group;
        }
    }
}
