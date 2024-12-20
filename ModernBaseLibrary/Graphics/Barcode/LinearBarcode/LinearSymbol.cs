//-----------------------------------------------------------------------
// <copyright file="LinearSymbol.cs" company="Lifeprojects.de">
//     Class: LinearSymbol
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
    /// <summary>
    /// Barcode encoding symbol for a given character or function
    /// </summary>
    internal class LinearSymbol
    {
        internal LinearSymbol(string character, int characterType, LinearPattern pattern, int width)
        {
            this.Character = character;
            this.CharacterType = characterType;
            this.Pattern = pattern;
            this.Width = width;
        }

        public string Character { get; set; }

        public int CharacterType { get; set; }

        public LinearPattern Pattern { get; set; }

        public int Width { get; set; }
    }
}
