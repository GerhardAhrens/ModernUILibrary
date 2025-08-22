//-----------------------------------------------------------------------
// <copyright file="SvgClipPathElement.cs" company="Lifeprojects.de">
//     Class: SvgClipPathElement
//     Copyright © Lifeprojects.de 2023
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

    /// <summary>
    ///   Represents a &lt;clipPath&gt; element.
    /// </summary>
    internal class SvgClipPathElement : SvgContainerBaseElement
    {
        public SvgClipPathElement(SvgDocument document, SvgBaseElement parent, XElement clipPathElement) : base(document, parent, clipPathElement)
        {
        }

        public Geometry GetClipGeometry()
        {
            GeometryGroup geometry_group = new GeometryGroup();

            foreach (SvgBaseElement child_element in Children)
            {
                SvgBaseElement element = child_element;
                if (element is SvgUseElement)
                {
                    element = (element as SvgUseElement).GetElement();
                }


                if (element is SvgDrawableBaseElement)
                {
                    Geometry geometry = (element as SvgDrawableBaseElement).GetGeometry();
                    if (geometry != null)
                    {
                        geometry_group.Children.Add(geometry);
                    }
                }
                else if (element is SvgDrawableContainerBaseElement)
                {
                    Geometry geometry = (element as SvgDrawableContainerBaseElement).GetGeometry();
                    if (geometry != null)
                    {
                        geometry_group.Children.Add(geometry);
                    }
                }
            }

            return geometry_group;
        }
    }
}
