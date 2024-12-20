//-----------------------------------------------------------------------
// <copyright file="QREnums.cs" company="Lifeprojects.de">
//     Class: QRModuleType
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Enum Class for QRModuleType (QRCode Barcode)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Barcode
{
    using System.ComponentModel;

    /// <summary>
    /// Type of an individual module (pixel) of a QR symbol.
    /// </summary>
    public enum QRModuleType : int
    {
        [Description("Heller QR Barcode")]
        Light,
        [Description("Dunkler QR Barcode")]
        Dark
    }

    /// <summary>
    /// QR encoding modes
    /// </summary>
    public enum QRMode
    {
        ECI = 0,
        Numeric = 1,
        AlphaNumeric = 2,
        Byte = 3,
        Kanji = 4,
        StructuredAppend = 5,
        FNC1_FirstPosition = 6,
        FNC1_SecondPosition = 7,
        Terminator = 8
    }

    /// <summary>
    /// QR symbol types
    /// </summary>
    public enum QRSymbolType
    {
        Micro,
        Normal
    }

    /// <summary>
    /// QR error correction modes
    /// </summary>
    public enum QRErrorCorrection
    {
        [Description("Error-Detection Only")]
        None = 0,
        [Description("L (7%)")]
        L = 1,
        [Description("M (15%)")]
        M = 2,
        [Description("Q (25%)")]
        Q = 3,
        [Description("H (30%)")]
        H = 4
    }
}
