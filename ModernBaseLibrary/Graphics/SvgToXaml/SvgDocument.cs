//-----------------------------------------------------------------------
// <copyright file="SvgDocument.cs" company="Lifeprojects.de">
//     Class: SvgDocument
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
            this.Root = new SvgSVGElement(this, null, root);
            this.Options = options;
        }

        public DrawingImage Draw()
        {
            return new DrawingImage(Root.Draw());
        }
    }
}
