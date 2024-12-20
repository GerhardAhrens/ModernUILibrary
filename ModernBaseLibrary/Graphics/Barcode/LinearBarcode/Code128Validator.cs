//-----------------------------------------------------------------------
// <copyright file="Code128Validator.cs" company="Lifeprojects.de">
//     Class: Code128Validator
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
    internal class Code128Validator : ILinearValidator
    {
        public string Parse(string text, Symbology symbology)
        {
            return LinearHelpers.GetOnlyAscii(text);
        }
    }
}
