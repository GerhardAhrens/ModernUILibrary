//-----------------------------------------------------------------------
// <copyright file="FluentColor.cs" company="Lifeprojects.de">
//     Class: FluentColor
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@pta.de</email>
// <date>07.05.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur Color Behandlung auf Basis einer
// FluentAPI (FluentColor) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System.Diagnostics;
    using System.Drawing;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentColor : FluentColor<FluentColor>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentColor"/> class.
        /// </summary>
        public FluentColor(Color value) : base(value)
        {
        }
    }

    public class FluentColor<TAssertions> : ReferenceTypeAssertions<Color, TAssertions> where TAssertions : FluentColor<TAssertions>
    {
        public FluentColor(Color value) : base(value)
        {
            this.ColorValue = value;
        }

        private Color ColorValue { get; set; }

        public int ColorToInt()
        {
            return (int)((this.ColorValue.A << 24) | (this.ColorValue.R << 16) | (this.ColorValue.G << 8) | (this.ColorValue.B << 0));
        }

        public string ColorToHex()
        {
            return string.Format("#{0:X6}", this.ColorValue.ToArgb() & 0x00FFFFFF);
        }

        #region Is-Methodes

        #endregion Is-Methodes

        #region To-Methodes

        /// <summary>
        /// returns the RGB Value of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>string</returns>
        /// <remarks></remarks>
        public string ToHtmlColor()
        {
            return ColorTranslator.ToHtml(this.ColorValue);
        }

        /// <summary>
        /// returns the OLE Value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ToOleColor()
        {
            return ColorTranslator.ToOle(this.ColorValue);
        }

        /// <summary>
        /// returns the Win32 value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ToWin32Color()
        {
            return ColorTranslator.ToWin32(this.ColorValue);
        }
        #endregion To-Methodes
    }
}