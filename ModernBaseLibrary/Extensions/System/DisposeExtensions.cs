﻿
namespace ModernBaseLibrary.Extension
{
    public static class DisposeExtensions
    {
        public static void DisposeSafely<T>(ref T disposable) where T : IDisposable
        {
            var tmp = disposable;
            disposable = default;
            tmp?.Dispose();
        }

        public static void DisposeSafely<T>(this T disposable, Action setNull) where T : IDisposable
        {
            var tmp = disposable;
            setNull();
            tmp?.Dispose();
        }
    }
}
