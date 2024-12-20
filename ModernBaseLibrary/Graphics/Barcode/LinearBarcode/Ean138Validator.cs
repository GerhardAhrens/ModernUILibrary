//-----------------------------------------------------------------------
// <copyright file="Ean138Validator.cs" company="Lifeprojects.de">
//     Class: Ean138Validator
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
    internal class Ean138Validator : ILinearValidator
    {
        public string Parse(string text, Symbology symbology)
        {
            switch (symbology)
            {
                case Symbology.Ean13:
                    return LinearHelpers.GetOnlyNumeric(text, 12);

                case Symbology.UpcA:
                    return LinearHelpers.GetOnlyNumeric(text, 11);

                case Symbology.Ean8:
                    return LinearHelpers.GetOnlyNumeric(text, 7);

                default:
                    return LinearHelpers.GetOnlyNumeric(text, 12);
            }
        }
    }
}
