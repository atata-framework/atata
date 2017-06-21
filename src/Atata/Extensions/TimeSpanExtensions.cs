using System;

namespace Atata
{
    public static class TimeSpanExtensions
    {
        public static string ToIntervalString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes >= 1d)
            {
                double minutes = Math.Floor(timeSpan.TotalMinutes);
                double seconds = Math.Floor(timeSpan.TotalSeconds - (minutes * 60d));
                return $"{minutes}m {seconds}.{timeSpan:fff}s";
            }
            else
            {
                return $"{Math.Floor(timeSpan.TotalSeconds)}.{timeSpan:fff}s";
            }
        }
    }
}
