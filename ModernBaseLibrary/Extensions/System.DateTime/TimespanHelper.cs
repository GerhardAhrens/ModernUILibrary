namespace ModernBaseLibrary.Extension
{
    public static class TimespanHelper
    {
        public static TimeSpan Forever { get; } = TimeSpan.FromMilliseconds(-1);

        public static bool IsForever(this TimeSpan timeSpan) => timeSpan.Equals(TimespanHelper.Forever);
    }
}
