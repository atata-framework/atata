using System;

namespace Atata
{
    public class SearchOptions : ICloneable
    {
        public SearchOptions()
        {
            Timeout = RetrySettings.Timeout;
            RetryInterval = RetrySettings.RetryInterval;
            Visibility = ElementVisibility.Visible;
            IsSafely = true;
        }

        public TimeSpan Timeout { get; set; }
        public TimeSpan RetryInterval { get; set; }
        public ElementVisibility Visibility { get; set; }
        public bool IsSafely { get; set; }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public SearchOptions Clone()
        {
            return (SearchOptions)MemberwiseClone();
        }

        public static SearchOptions Safely(bool isSafely = true)
        {
            return new SearchOptions { IsSafely = isSafely };
        }

        public static SearchOptions Invisible()
        {
            return new SearchOptions { Visibility = ElementVisibility.Invisible };
        }

        public static SearchOptions OfAnyVisibility()
        {
            return new SearchOptions { Visibility = ElementVisibility.Any };
        }

        public static SearchOptions WithRetry(TimeSpan timeout)
        {
            return new SearchOptions { Timeout = timeout };
        }

        public static SearchOptions WithRetry(TimeSpan timeout, TimeSpan retryInterval)
        {
            return new SearchOptions { Timeout = timeout, RetryInterval = retryInterval };
        }

        public static SearchOptions Immediately()
        {
            return new SearchOptions { Timeout = TimeSpan.Zero };
        }
    }
}
