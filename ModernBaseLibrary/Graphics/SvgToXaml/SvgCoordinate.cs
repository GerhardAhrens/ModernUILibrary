//-----------------------------------------------------------------------
// <copyright file="SvgCoordinate.cs" company="Lifeprojects.de">
//     Class: SvgCoordinate
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
    /// <summary>
    ///   A coordinate.
    /// </summary>
    internal class SvgCoordinate : SvgLength
    {

        public SvgCoordinate(double value)
          : base(value)
        {
            // ...
        }

        public SvgCoordinate(double value, string unit)
          : base(value, unit)
        {
            // ...
        }

        public static new SvgCoordinate Parse(string value)
        {
            SvgLength length = SvgLength.Parse(value);

            return new SvgCoordinate(length.Value, length.Unit);

        }
    }
}
