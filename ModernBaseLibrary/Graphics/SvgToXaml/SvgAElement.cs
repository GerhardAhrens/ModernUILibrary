﻿//-----------------------------------------------------------------------
// <copyright file="SvgAElement.cs" company="Lifeprojects.de">
//     Class: SvgAElement
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
// erstellt, und von mir für NET 7 überarbeitet und angepasst.
// </remark>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Xml.Linq;

    internal class SvgAElement : SvgDrawableContainerBaseElement
    {
        public SvgAElement(SvgDocument document, SvgBaseElement parent, XElement aElement) : base(document, parent, aElement)
        {
        }
    }
}