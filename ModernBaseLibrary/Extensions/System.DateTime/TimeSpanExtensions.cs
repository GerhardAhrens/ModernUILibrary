namespace ModernBaseLibrary.Extension
{
    using System;

    public static class TimeSpanExtensions
    {
        public static bool IsGreaterThanZero(this TimeSpan t)
        {
            return t > TimeSpan.Zero;
        }

        public static DateTime Ago(this TimeSpan t)
        {
            return DateTime.Now - t;
        }

        public static DateTime Hence(this TimeSpan t)
        {
            return DateTime.Now + t;
        }

        public static DateTime Before(this TimeSpan t, DateTime d)
        {
            return d - t;
        }

        public static DateTime After(this TimeSpan t, DateTime d)
        {
            return d + t;
        }

        public static TimeSpan ShorterThan(this TimeSpan t1, TimeSpan t2)
        {
            return t2 - t1;
        }

        public static TimeSpan LongerThan(this TimeSpan t1, TimeSpan t2)
        {
            return t2 + t1;
        }

        public static string ToReadableString(this TimeSpan timeSpan, TimeUnit accuracy = TimeUnit.Second)
        {
            string readableString = "";

            if (timeSpan.TotalDays < 1 && accuracy == TimeUnit.Day)
            {
                return "< 1d";
            }
            if (timeSpan.Days > 0 && accuracy >= TimeUnit.Day)
            {
                readableString += timeSpan.Days + "d ";
            }

            if (timeSpan.TotalHours < 1 && accuracy == TimeUnit.Hour)
            {
                return "< 1h";
            }
            if (timeSpan.Hours > 0 && accuracy >= TimeUnit.Hour)
            {
                readableString += timeSpan.Hours + "h ";
            }

            if (timeSpan.TotalMinutes < 1 && accuracy == TimeUnit.Minute)
            {
                return "< 1min";
            }

            if (timeSpan.Minutes > 0 && accuracy >= TimeUnit.Minute)
            {
                readableString += timeSpan.Minutes + "min ";
            }

            if (timeSpan.TotalSeconds < 1 && accuracy == TimeUnit.Second)
            {
                return "< 1s";
            }
            if (timeSpan.Seconds > 0 && accuracy >= TimeUnit.Second)
            {
                readableString += timeSpan.Seconds + "s ";
            }

            if (timeSpan.Milliseconds > 0 && accuracy >= TimeUnit.Millisecond)
            {
                readableString += timeSpan.Milliseconds + "ms ";
            }

            return readableString.TrimEnd(' ');
        }

    }
}