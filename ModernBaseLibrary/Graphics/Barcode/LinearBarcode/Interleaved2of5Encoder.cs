//-----------------------------------------------------------------------
// <copyright file="Interleaved2Of5Encoder.cs" company="Lifeprojects.de">
//     Class: Interleaved2Of5Encoder
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
    using System.Runtime.Versioning;

    /// <summary>
    /// Interleaved 2 of 5 barcode encoder.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal class Interleaved2Of5Encoder : LinearEncoder
    {
        private Dictionary<string, LinearPattern> _patternDictionary;
        private readonly bool _useCheckDigit;

        public Interleaved2Of5Encoder(Symbology symbology) : base(symbology)
        {
            switch (Symbology)
            {
                case Symbology.I2of5:
                    // Interleaved 2 of 5
                    _useCheckDigit = false;
                    Description = "Interleaved 2 of 5";
                    break;

                case Symbology.I2of5C:
                    // Interleaved 2 of 5 with check digit
                    _useCheckDigit = true;
                    Description = "Interleaved 2 of 5 With Check Digit";
                    break;

                default:
                    // Interleaved 2 of 5 with check digit
                    _useCheckDigit = true;
                    Description = "Interleaved 2 of 5 With Check Digit";
                    break;
            }
        }

        internal override ILinearValidator BarcodeValidator { get; } = new Interleaved2Of5Validator();

        protected override void Encode(string barcodeValue)
        {
            LoadSymbologyPattern();

            // Add check digit to barcode value.
            if (_useCheckDigit)
            {
                int checkDigit = LinearHelpers.GetUpcCheckDigit(barcodeValue);
                barcodeValue += checkDigit;
                EncodedValue = barcodeValue;
            }
            EncodedValue = barcodeValue;
            ZplEncode = EncodedValue;

            for (int encodePosition = 0; encodePosition <= barcodeValue.Length - 1; encodePosition += 2)
            {
                // Check if first or last character in barcode and insert start/stop symbol
                if (encodePosition == 0)
                {
                    LinearEncoding.Add("*", 1, _patternDictionary["START"]);
                }

                string digitPair = barcodeValue.Substring(encodePosition, 2);
                LinearEncoding.Add(digitPair, 0, _patternDictionary[digitPair]);


                if (encodePosition == barcodeValue.Length - 2)
                {
                    LinearEncoding.Add("*", 1, _patternDictionary["STOP"]);
                }
            }

            SetMinXDimension();
            SetMinBarcodeHeight();

        }

        /// <summary>
        /// Increases the barcode Xdimension to minimum required by symbology, if currently set lower
        /// </summary>
        internal override void SetMinXDimension()
        {
            int xdimensionOriginal = XDimension;
            int minXdimension = (int)Math.Ceiling(Dpi * 0.0075);
            XDimension = Math.Max(XDimension, minXdimension);

            // Set flag to show xdimension was adjusted
            if (xdimensionOriginal != XDimension)
            {
                XDimensionChanged = true;
            }
        }

        /// <summary>
        /// Increases the barcode height to minimum required by symbology, if currently set lower
        /// </summary>
        internal override void SetMinBarcodeHeight()
        {
            int barcodeHeightOriginal = BarcodeHeight;
            int minBarcodeHeight = (int)Math.Ceiling(Math.Max(LinearEncoding.MinimumWidth * XDimension * 0.15, Dpi * 0.25));
            BarcodeHeight = Math.Max(BarcodeHeight, minBarcodeHeight);

            // Set flag to show barcode height was adjusted
            if (barcodeHeightOriginal != BarcodeHeight)
            {
                BarcodeHeightChanged = true;
            }
        }

        private void LoadSymbologyPattern()
        {
            if (_patternDictionary != null)
            {
                return;
            }

            _patternDictionary = new Dictionary<string, LinearPattern>
            {
                {"00", new LinearPattern("NNNNWWWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"01", new LinearPattern("NWNNWNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"02", new LinearPattern("NNNWWNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"03", new LinearPattern("NWNWWNWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"04", new LinearPattern("NNNNWWWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"05", new LinearPattern("NWNNWWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"06", new LinearPattern("NNNWWWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"07", new LinearPattern("NNNNWNWWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"08", new LinearPattern("NWNNWNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"09", new LinearPattern("NNNWWNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"10", new LinearPattern("WNNNNWNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"11", new LinearPattern("WWNNNNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"12", new LinearPattern("WNNWNNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"13", new LinearPattern("WWNWNNNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"14", new LinearPattern("WNNNNWNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"15", new LinearPattern("WWNNNWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"16", new LinearPattern("WNNWNWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"17", new LinearPattern("WNNNNNNWWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"18", new LinearPattern("WWNNNNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"19", new LinearPattern("WNNWNNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"20", new LinearPattern("NNWNNWNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"21", new LinearPattern("NWWNNNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"22", new LinearPattern("NNWWNNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"23", new LinearPattern("NWWWNNNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"24", new LinearPattern("NNWNNWNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"25", new LinearPattern("NWWNNWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"26", new LinearPattern("NNWWNWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"27", new LinearPattern("NNWNNNNWWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"28", new LinearPattern("NWWNNNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"29", new LinearPattern("NNWWNNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"30", new LinearPattern("WNWNNWNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"31", new LinearPattern("WWWNNNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"32", new LinearPattern("WNWWNNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"33", new LinearPattern("WWWWNNNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"34", new LinearPattern("WNWNNWNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"35", new LinearPattern("WWWNNWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"36", new LinearPattern("WNWWNWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"37", new LinearPattern("WNWNNNNWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"38", new LinearPattern("WWWNNNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"39", new LinearPattern("WNWWNNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"40", new LinearPattern("NNNNWWNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"41", new LinearPattern("NWNNWNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"42", new LinearPattern("NNNWWNNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"43", new LinearPattern("NWNWWNNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"44", new LinearPattern("NNNNWWNNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"45", new LinearPattern("NWNNWWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"46", new LinearPattern("NNNWWWNNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"47", new LinearPattern("NNNNWNNWWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"48", new LinearPattern("NWNNWNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"49", new LinearPattern("NNNWWNNWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"50", new LinearPattern("WNNNWWNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"51", new LinearPattern("WWNNWNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"52", new LinearPattern("WNNWWNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"53", new LinearPattern("WWNWWNNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"54", new LinearPattern("WNNNWWNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"55", new LinearPattern("WWNNWWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"56", new LinearPattern("WNNWWWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"57", new LinearPattern("WNNNWNNWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"58", new LinearPattern("WWNNWNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"59", new LinearPattern("WNNWWNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"60", new LinearPattern("NNWNWWNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"61", new LinearPattern("NWWNWNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"62", new LinearPattern("NNWWWNNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"63", new LinearPattern("NWWWWNNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"64", new LinearPattern("NNWNWWNNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"65", new LinearPattern("NWWNWWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"66", new LinearPattern("NNWWWWNNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"67", new LinearPattern("NNWNWNNWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"68", new LinearPattern("NWWNWNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"69", new LinearPattern("NNWWWNNWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"70", new LinearPattern("NNNNNWWWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"71", new LinearPattern("NWNNNNWNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"72", new LinearPattern("NNNWNNWNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"73", new LinearPattern("NWNWNNWNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"74", new LinearPattern("NNNNNWWNWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"75", new LinearPattern("NWNNNWWNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"76", new LinearPattern("NNNWNWWNWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"77", new LinearPattern("NNNNNNWWWW", BarcodeModuleType.Bar, WideBarRatio)},
                {"78", new LinearPattern("NWNNNNWWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"79", new LinearPattern("NNNWNNWWWN", BarcodeModuleType.Bar, WideBarRatio)},
                {"80", new LinearPattern("WNNNNWWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"81", new LinearPattern("WWNNNNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"82", new LinearPattern("WNNWNNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"83", new LinearPattern("WWNWNNWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"84", new LinearPattern("WNNNNWWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"85", new LinearPattern("WWNNNWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"86", new LinearPattern("WNNWNWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"87", new LinearPattern("WNNNNNWWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"88", new LinearPattern("WWNNNNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"89", new LinearPattern("WNNWNNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"90", new LinearPattern("NNWNNWWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"91", new LinearPattern("NWWNNNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"92", new LinearPattern("NNWWNNWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"93", new LinearPattern("NWWWNNWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"94", new LinearPattern("NNWNNWWNNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"95", new LinearPattern("NWWNNWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"96", new LinearPattern("NNWWNWWNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"97", new LinearPattern("NNWNNNWWNW", BarcodeModuleType.Bar, WideBarRatio)},
                {"98", new LinearPattern("NWWNNNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"99", new LinearPattern("NNWWNNWWNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"START", new LinearPattern("NNNN", BarcodeModuleType.Bar, WideBarRatio)},
                {"STOP", new LinearPattern("WNN", BarcodeModuleType.Bar, WideBarRatio)}
            };
        }
    }
}
