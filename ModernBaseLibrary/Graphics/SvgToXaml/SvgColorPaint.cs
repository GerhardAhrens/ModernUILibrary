//-----------------------------------------------------------------------
// <copyright file="SvgColorPaint.cs" company="Lifeprojects.de">
//     Class: SvgColorPaint
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
    using System.Windows.Media;

    /// <summary>
    ///   A paint with a solid color.
    /// </summary>
    internal class SvgColorPaint : SvgPaint
    {
        public readonly SvgColor Color;

        public SvgColorPaint(SvgColor color)
        {
            Color = color;
        }

        public override Brush ToBrush(SvgBaseElement element)
        {
            return new SolidColorBrush(Color.ToColor());
        }
    }
}
