//-----------------------------------------------------------------------
// <copyright file="SvgURL.cs" company="Lifeprojects.de">
//     Class: SvgURL
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
    using System;

    internal class SvgURL
    {
        public readonly string Id;

        public SvgURL(string id)
        {
            Id = id;
        }

        public static SvgURL Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value = value.Trim();

            if (value == "none")
                return null;

            if (value == "")
                throw new ArgumentException("value must not be empty.");

            if (value.StartsWith("url"))
            {
                value = value.Substring(3).Trim();
                if (value.StartsWith("(") && value.EndsWith(")"))
                {
                    value = value.Substring(1, value.Length - 2).Trim();
                    if (value.StartsWith("#"))
                        return new SvgURL(value.Substring(1));
                }
            }

            throw new ArgumentException(String.Format("Unsupported URL value: {0}", value));
        }
    }
}
