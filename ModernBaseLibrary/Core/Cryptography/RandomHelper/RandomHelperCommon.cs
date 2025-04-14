namespace ModernBaseLibrary.Cryptography
{
    using System;

    internal static class RandomHelperCommon
    {
        public const UInt64 mask1_64 = 0b00111111_11110000_00000000_00000000__00000000_00000000_00000000_00000000;
        public const UInt64 mask2_64 = 0b00111111_11111111_11111111_11111111__11111111_11111111_11111111_11111111;
        public const UInt32 mask1_32 = 0b00111111_10000000_00000000_00000000;
        public const UInt32 mask2_32 = 0b00111111_11111111_11111111_11111111;

        public static unsafe float UInt32ToFloat(UInt32 i)
        {
            float d;
            do
            {
                // Modified from https://stackoverflow.com/a/52148190/313088
                i = (i | RandomHelperCommon.mask1_32) & RandomHelperCommon.mask2_32;
                d = *(float*)(&i);
                // Unlikely that we'll get 1.0D, but a promise is a promise.
            } while (d >= 2.0D);

            return d - 1;
        }

        public static unsafe double UInt64ToDouble(UInt64 i)
        {
            double d;
            do
            {
                // https://stackoverflow.com/a/52148190/313088
                i = (i | RandomHelperCommon.mask1_64) & RandomHelperCommon.mask2_64;
                d = *(double*)(&i);
                // Unlikely that we'll get 1.0D, but a promise is a promise.
            } while (d >= 2.0D);

            return d - 1;
        }
    }
}
