namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct SeedParts
    {
        [FieldOffset(0)]
        public UInt32 N1;
        [FieldOffset(4)]
        public UInt32 N2;
        [FieldOffset(0)]
        public UInt64 Seed;
    }
}