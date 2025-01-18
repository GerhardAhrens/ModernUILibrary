//-----------------------------------------------------------------------
// <copyright file="StringFluent.cs" company="Lifeprojects.de">
//     Class: StringFluent
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur String Behandlung auf Basis einer
// FluentAPI (StringFluentExtension) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;

    using EasyPrototypingNET.Core;

    using ModernBaseLibrary.Core;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentInt : FluentInt<FluentInt>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentInt"/> class.
        /// </summary>
        public FluentInt(int value) : base(value)
        {
        }
    }

    public class FluentInt<TAssertions> : ReferenceTypeAssertions<int, TAssertions> where TAssertions : FluentInt<TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentInt{TAssertions}"/> class.
        /// </summary>
        public FluentInt(int value) : base(value)
        {
            this.IntValue = value;
        }

        private int IntValue { get; set; }

        #region To-Methodes()
        public string ToString(string format, IFormatProvider provider)
        {
            return this.IntValue.ToString(format, provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.IntValue.ToString(provider);
        }

        public string ToString(string format = null)
        {
            return this.IntValue.ToString(format);
        }

        public Color ToColor()
        {
            byte[] values = BitConverter.GetBytes(this.IntValue);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(values);
            }

            return Color.FromArgb(values[0], values[1], values[2]);
        }

        /// <summary>
        /// Gibt vom einem OLE Color Code einen Color Typ zurück
        /// </summary>
        /// <returns>OLE Color Code</returns>
        public Color FromOleColor()
        {
            return ColorTranslator.FromOle(this.IntValue);
        }

        /// <summary>
        /// Gibt vom einem Win32 Color Code einen Color Typ zurück
        /// </summary>
        /// <returns>Win32 Color Code</returns>
        public Color FromWin32()
        {
            return ColorTranslator.FromWin32(this.IntValue);
        }

        #endregion To-Methodes()

        #region Is-Methodes()
        public bool IsBetween(int minValue, int maxValue)
        {
            return minValue.CompareTo(this.IntValue) == -1 && this.IntValue.CompareTo(maxValue) == -1;
        }

        /// <summary>
        /// Prüft, ob ein Integer innerhalb, ausserhalb, oder genau auf der Grenze (min, max) eines Bereiches liegt.
        /// </summary>
        /// <param name="pBorder1"></param>
        /// <param name="pBorder2"></param>
        /// <returns>Enum RangeCheck</returns>
        public RangeCheck IsInRange(int pBorder1, int pBorder2)
        {
            return (RangeCheck)(Math.Sign(pBorder1.CompareTo(pBorder2)) == 0 ?
                Math.Abs(Math.Sign(this.IntValue.CompareTo(pBorder1))) + 1 :
                Math.Abs(Math.Sign(this.IntValue.CompareTo(pBorder1)) + Math.Sign(this.IntValue.CompareTo(pBorder2))));
        }
        #endregion Is-Methodes()
    }
}
