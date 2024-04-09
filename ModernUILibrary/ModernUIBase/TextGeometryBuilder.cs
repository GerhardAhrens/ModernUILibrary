//-----------------------------------------------------------------------
// <copyright file="TextGeometryBuilder.cs" company="Lifeprojects.de">
//     Class: TextGeometryBuilder
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>09.04.2024 11:09:59</date>
//
// <summary>
// Klasse für 
// </summary>
// <Website>
// https://norberteder.com/wpf-geometry-aus-formatiertem-text-erstellen/
// </Website>
//-----------------------------------------------------------------------

namespace ModernUILibrary.ModernUIBase
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    public class TextGeometryBuilder
    {
        private static Dictionary<string, Geometry> cachedGeometries = new Dictionary<string, Geometry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextGeometryBuilder"/> class.
        /// </summary>
        public TextGeometryBuilder()
        {
        }

        public string Text { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStretch FontStretch { get; set; }
        public double FontSize { get; set; }
        public Point Origin { get; set; }

        public Geometry Geometry
        {
            get
            {
                if (cachedGeometries.ContainsKey(Text))
                {
                    return cachedGeometries[Text];
                }

                FormattedText formattedText = new FormattedText(Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize,Brushes.Black,0);

                var geometry = formattedText.BuildGeometry(Origin);
                cachedGeometries.Add(Text, geometry);
                return geometry;
            }
        }

        public PathGeometry PathGeometry
        {
            get
            {
                return PathGeometry.CreateFromGeometry(Geometry);
            }
        }
    }
}
