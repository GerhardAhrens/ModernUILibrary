//-----------------------------------------------------------------------
// <copyright file="BitArrayHelpers.cs" company="Lifeprojects.de">
//     Class: BitArrayHelpers
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Helpers functions for BitArrays (QR Barcode)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Barcode
{
    using System.Collections;
    using System.Text;

    public static class BitArrayHelpers
    {
        public static string ToBitString(this BitArray bits)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bits.Length; i++)
            {
                sb.Append(bits[i] ? "1" : "0");
                if (i % 8 == 7)
                    sb.Append(" ");
            }

            return sb.ToString();
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            byte[] bytes = new byte[(bits.Length - 1) / 8 + 1];
            for (int b = 0; b < bits.Length; b++)
                if (bits[b])
                    bytes[b / 8] |= (byte)(0x80 >> (b % 8));
            return bytes;
        }

        public static BitArray ToBitArray(this byte[] bytes)
        {
            var b = new BitArray(8 * bytes.Length, false);

            for (int i = 0; i < b.Length; i++)
                if ((bytes[i / 8] & (0x80 >> (i % 8))) != 0)
                    b[i] = true;

            return b;
        }

        public static BitArray ToBitArray(this int x, int bits)
        {
            var b = new BitArray(bits, false);
            for (int i = 0; i < bits; i++)
                if ((x & ((1 << (bits - 1)) >> i)) != 0)
                    b[i] = true;
            return b;
        }
    }
}
