//-----------------------------------------------------------------------
// <copyright file="Interleaved2Of5Validator.cs" company="Lifeprojects.de">
//     Class: Interleaved2Of5Validator
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.05.2023</date>
//
// <summary>
// Die Klasse gehört zur Erstellung von LinearBarcode,
// überarbeitet für NET 7
// </summary>
// <Website>
// https://github.com/brettreynolds/Barcoded-dotNet-Framework
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Barcode
{
    internal class Interleaved2Of5Validator : ILinearValidator
    {
        public string Parse(string text, Symbology symbology)
        {
            switch (symbology)
            {
                case Symbology.I2of5:
                    return LinearHelpers.GetEvenNumeric(text);

                case Symbology.I2of5C:
                    return LinearHelpers.GetOddNumeric(text);

                default:
                    return LinearHelpers.GetOddNumeric(text);
            }
        }
    }
}
