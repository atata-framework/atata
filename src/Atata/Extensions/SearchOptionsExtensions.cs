using System;

namespace Atata
{
    public static class SearchOptionsExtensions
    {
        public static SearchOptions Safely(this SearchOptions options, bool isSafely = true)
        {
            SearchOptions clone = options.Clone();
            clone.IsSafely = isSafely;
            return clone;
        }

        public static SearchOptions Invisible(this SearchOptions options)
        {
            SearchOptions clone = options.Clone();
            clone.Visibility = ElementVisibility.Invisible;
            return clone;
        }

        public static SearchOptions OfAnyVisibility(this SearchOptions options)
        {
            SearchOptions clone = options.Clone();
            clone.Visibility = ElementVisibility.Any;
            return clone;
        }

        public static SearchOptions WithRetry(this SearchOptions options, TimeSpan timeout)
        {
            SearchOptions clone = options.Clone();
            clone.Timeout = timeout;
            return clone;
        }

        public static SearchOptions WithRetry(this SearchOptions options, TimeSpan timeout, TimeSpan retryInterval)
        {
            SearchOptions clone = options.Clone();
            clone.Timeout = timeout;
            clone.RetryInterval = retryInterval;
            return clone;
        }

        public static SearchOptions Immediately(this SearchOptions options)
        {
            SearchOptions clone = options.Clone();
            clone.Timeout = TimeSpan.Zero;
            return clone;
        }
    }
}
