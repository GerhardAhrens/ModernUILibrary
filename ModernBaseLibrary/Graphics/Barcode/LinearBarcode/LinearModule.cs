//-----------------------------------------------------------------------
// <copyright file="LinearModule.cs" company="Lifeprojects.de">
//     Class: LinearModule
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
    /// The bar or space of element that makes up a symbol pattern.
    /// </summary>
    public class LinearModule
    {
        /// <summary>
        /// Module type (bar or space).
        /// </summary>
        public BarcodeModuleType ModuleType { get; set; }

        /// <summary>
        /// Module point width.
        /// </summary>
        public int Width { get; set; }

        internal LinearModule(BarcodeModuleType moduleType, int width)
        {
            ModuleType = moduleType;
            Width = width;
        }
    }
}
