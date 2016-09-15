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
            IsSafely = false;
        }

        public TimeSpan Timeout { get; set; }

        public TimeSpan RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the search element. The default value is Visible.
        /// </summary>
        public ElementVisibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search element is safely searching. If it is true then null is returned after the search, otherwise an exception is thrown. The default value is false.
        /// </summary>
        public bool IsSafely { get; set; }

        public static SearchOptions Safely(bool isSafely = true)
        {
            return new SearchOptions { IsSafely = isSafely };
        }

        public static SearchOptions Unsafely()
        {
            return new SearchOptions { IsSafely = false };
        }

        public static SearchOptions SafelyAndImmediately(bool isSafely = true)
        {
            return new SearchOptions { IsSafely = isSafely, Timeout = TimeSpan.Zero };
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

        public static SearchOptions WithRetry(double timeoutSeconds)
        {
            return new SearchOptions { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };
        }

        public static SearchOptions WithRetry(double timeoutSeconds, double retryIntervalSeconds)
        {
            return new SearchOptions { Timeout = TimeSpan.FromSeconds(timeoutSeconds), RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds) };
        }

        public static SearchOptions Immediately()
        {
            return new SearchOptions { Timeout = TimeSpan.Zero };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public SearchOptions Clone()
        {
            return (SearchOptions)MemberwiseClone();
        }
    }
}
