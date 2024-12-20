//-----------------------------------------------------------------------
// <copyright file="LinearVectors.cs" company="Lifeprojects.de">
//     Class: LinearVectors
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
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    /// <summary>
    /// Vectored version of the barcode
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class LinearVectors
    {
        internal LinearVectors()
        {
            // Empty Constructor
            this.Width = 0;
        }

        internal LinearVectors(LinearEncoder encoder)
        {
            this.Data.Clear();
            this.Width = encoder.LinearEncoding.MinimumWidth * encoder.XDimension;

            foreach (KeyValuePair<int, LinearSymbol> symbol in encoder.LinearEncoding.Symbols)
            {
                foreach (KeyValuePair<int, LinearModule> module in symbol.Value.Pattern)
                {
                    LinearModule newModule = new LinearModule(module.Value.ModuleType, module.Value.Width * encoder.XDimension);
                    this.Data.Add(this.Data.Count, newModule);
                }
            }
        }

        /// <summary>
        /// The combined bar and space modules, ordered list that represents the full barcode.
        /// </summary>
        public Dictionary<int, LinearModule> Data { get; } = new Dictionary<int, LinearModule>();

        /// <summary>
        /// Total point width of the vector data
        /// </summary>
        public int Width { get; }
    }
}
