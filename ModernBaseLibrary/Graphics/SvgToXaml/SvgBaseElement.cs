//-----------------------------------------------------------------------
// <copyright file="SvgBaseElement.cs" company="Lifeprojects.de">
//     Class: SvgBaseElement
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
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    ///   Base class for all other SVG elements.
    /// </summary>
    internal class SvgBaseElement
    {
        public readonly SvgBaseElement Parent;
        public readonly string Id = null;
        public readonly XElement Element;
        public readonly SvgDocument Document;
        public readonly string Reference = null;

        public SvgSVGElement Root
        {
            get
            {
                return Document.Root;
            }
        }

        protected SvgBaseElement(SvgDocument document, SvgBaseElement parent, XElement element)
        {
            Document = document;
            Parent = parent;

            // Create attributes from styles...
            XAttribute style_attribute = element.Attribute("style");
            if (style_attribute != null)
            {
                foreach (string property in style_attribute.Value.Split(';'))
                {
                    string[] tokens = property.Split(':');
                    if (tokens.Length == 2)
                        try
                        {
                            element.SetAttributeValue(tokens[0], tokens[1]);
                        }
                        catch (XmlException)
                        {
                            continue;
                        }
                }
                style_attribute.Remove();
            }

            XAttribute id_attribute = element.Attribute("id");
            if (id_attribute != null)
                Document.Elements[Id = id_attribute.Value] = this;

            XAttribute href_attribute = element.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"));
            if (href_attribute != null)
            {
                string reference = href_attribute.Value;
                if (reference.StartsWith("#"))
                {
                    Reference = reference.Substring(1);
                }
            }

            Element = element;
        }
    }
}
