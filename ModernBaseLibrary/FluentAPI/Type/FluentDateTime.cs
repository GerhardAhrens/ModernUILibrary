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

    using ModernBaseLibrary.Core;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentDateTime : FluentDateTime<FluentDateTime>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentDateTime"/> class.
        /// </summary>
        public FluentDateTime(DateTime value) : base(value)
        {
        }
    }

    public class FluentDateTime<TAssertions> : ReferenceTypeAssertions<DateTime, TAssertions> where TAssertions : FluentDateTime<TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentDateTime{TAssertions}"/> class.
        /// </summary>
        public FluentDateTime(DateTime value) : base(value)
        {
            this.DateTimeValue = value;
        }

        private DateTime DateTimeValue { get; set; }

        #region Is-Methodes
        /// <summary>
        /// Prüft, ob ein Datum in einem Bereich minDateTime, maxDateTime liegt.
        /// </summary>
        /// <param name="minValue">MinDateTime</param>
        /// <param name="maxValue">MaxDateTime</param>
        /// <returns>True = Datum liegt im Bereich, False = Datum liegt ausserhalb des Bereichs</returns>
        public bool IsBetween(DateTime minValue, DateTime maxValue)
        {
            return minValue.CompareTo(this.DateTimeValue) == -1 && this.DateTimeValue.CompareTo(maxValue) == -1;
        }

        /// <summary>
        /// Prüft, ob ein Datum innerhalb, ausserhalb, oder genau auf der Grenze (min, max) eines Bereiches liegt.
        /// </summary>
        /// <param name="pBorder1"></param>
        /// <param name="pBorder2"></param>
        /// <returns>Enum RangeCheck</returns>
        public RangeCheck IsInRange(DateTime pBorder1, DateTime pBorder2)
        {
            return (RangeCheck)(Math.Sign(pBorder1.CompareTo(pBorder2)) == 0 ?
                Math.Abs(Math.Sign(this.DateTimeValue.CompareTo(pBorder1))) + 1 :
                Math.Abs(Math.Sign(this.DateTimeValue.CompareTo(pBorder1)) + Math.Sign(this.DateTimeValue.CompareTo(pBorder2))));
        }
        #endregion Is-Methodes
    }
}
