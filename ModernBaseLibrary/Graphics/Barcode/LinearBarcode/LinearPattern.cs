//-----------------------------------------------------------------------
// <copyright file="LinearPattern.cs" company="Lifeprojects.de">
//     Class: LinearPattern
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Barcode pattern of bars and spaces for a given symbol.
    /// </summary>
    internal class LinearPattern : Dictionary<int, LinearModule>
    {
        /// <summary>
        /// Creates a linear pattern using a string of int values for each module width.
        /// </summary>
        /// <param name="intPattern"></param>
        /// <param name="firstModule"></param>
        internal LinearPattern(string intPattern, BarcodeModuleType firstModule)
        {
            BarcodeModuleType moduleType = firstModule;
            for (int position = 0; position <= intPattern.Length - 1; position++)
            {
                Add(position, new LinearModule(moduleType, Convert.ToInt32(intPattern.Substring(position, 1))));

                switch (moduleType)
                {
                    case BarcodeModuleType.Bar:
                        moduleType = BarcodeModuleType.Space;
                        break;
                    case BarcodeModuleType.Space:
                        moduleType = BarcodeModuleType.Bar;
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a linear pattern using a (N)arrow (W)ide string and narrow to wide ratio for calculating each module width.
        /// </summary>
        /// <param name="narrowWidePattern"></param>
        /// <param name="firstModule"></param>
        /// <param name="wideRatio"></param>
        internal LinearPattern(string narrowWidePattern, BarcodeModuleType firstModule, int wideRatio)
        {
            BarcodeModuleType moduleType = firstModule;
            for (int position = 0; position <= narrowWidePattern.Length - 1; position++)
            {
                int moduleWidth = narrowWidePattern.Substring(position, 1).ToUpper() == "N" ? 1 : wideRatio;
                Add(position, new LinearModule(moduleType, moduleWidth));

                switch (moduleType)
                {
                    case BarcodeModuleType.Bar:
                        moduleType = BarcodeModuleType.Space;
                        break;
                    case BarcodeModuleType.Space:
                        moduleType = BarcodeModuleType.Bar;
                        break;
                }
            }
        }

        /// <summary>
        /// Get the pattern total point width.
        /// </summary>
        /// <returns>Returns the sum of the module widths within the pattern.</returns>
        public int GetWidth()
        {
            int symbolWidth = 0;

            for (int module = 0; module <= Count - 1; module++)
            {
                symbolWidth += this[module].Width;
            }

            return symbolWidth;
        }
    }
}
