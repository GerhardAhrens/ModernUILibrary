//-----------------------------------------------------------------------
// <copyright file="ValueAddedTax.cs" company="Lifeprojects.de">
//     Class: ValueAddedTax
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>02.08.2018</date>
//
// <summary>
// Berechnung Brutto/Netto
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Math
{
    public class ValueAddedTax
    {
        public static decimal GetNetAmount(decimal amount, decimal vat)
        {
            decimal result = 0;

            result = (amount / ((vat/100) + 1)/100) * 100;

            return result;
        }

        public static decimal GetGrossAmount(decimal amount, decimal vat)
        {
            decimal result = 0;

            result = amount+(amount*(vat/100));

            return result;
        }
    }
}
