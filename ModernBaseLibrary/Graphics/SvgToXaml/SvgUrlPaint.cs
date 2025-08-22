//-----------------------------------------------------------------------
// <copyright file="SvgUrlPaint.cs" company="Lifeprojects.de">
//     Class: SvgUrlPaint
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

    internal class SvgUrlPaint : SvgPaint
    {
        public readonly string Url;

        public SvgUrlPaint(string url)
        {
            Url = url;
        }

        public override Brush ToBrush(SvgBaseElement element)
        {
            if (!element.Document.Elements.ContainsKey(Url))
            {
                return null;
            }

            SvgBaseElement reference = element.Document.Elements[Url];
            if (reference is SvgGradientBaseElement)
            {
                return (reference as SvgGradientBaseElement).ToBrush();
            }
            else if (reference is SvgPatternElement)
            {
                return (reference as SvgPatternElement).ToBrush();
            }

            throw new NotImplementedException();
        }
    }
}
