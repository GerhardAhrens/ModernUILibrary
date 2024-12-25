//-----------------------------------------------------------------------
// <copyright file="ByteExtension.cs" company="Lifeprojects.de">
//     Class: ByteExtension
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.06.2020</date>
//
// <summary>
//      Extensions Class for byte Type
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{

    public static class ByteExtension
    {
        /// <summary>
        /// Returns the larger of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static Byte Max(this Byte val1, Byte val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        /// Returns the smaller of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static Byte Min(this Byte val1, Byte val2)
        {
            return Math.Min(val1, val2);
        }
    }
}
