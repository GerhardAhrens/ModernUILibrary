//-----------------------------------------------------------------------
// <copyright file="Elf32.cs" company="Lifeprojects.de">
//     Class: Elf32
//     Copyright � Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.09.2019</date>
//
// <summary>
//  Mit dieser Klasse kann ein ELF Hash Alogorithmus erstellt werden
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    /// <summary>
    /// Implements a 32-bit ELF hash algorithm compatible with ELF binary format.
    /// </summary>
    public sealed class Elf32 : HashAlgorithm
    {
        UInt32 hash;

        public Elf32()
        {
            hash = 0;
        }

        public override void Initialize()
        {
            hash = 0;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = CalculateHash(hash, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32ToBigEndianBytes(hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 32; } }

        public static UInt32 Compute(byte[] buffer)
        {
            return CalculateHash(0, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return CalculateHash(seed, buffer, 0, buffer.Length);
        }

        static UInt32 CalculateHash(UInt32 seed, IList<byte> buffer, int start, int size)
        {
            var hash = seed;
            for (var i = start; i < start + size; i++)
            {
                hash = (hash << 4) + buffer[i];
                var work = hash & 0xf0000000u;
                hash ^= work >> 24;
                hash &= ~work;
            }
            return hash;
        }

        static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
    }
}