using System;

namespace Atata
{
    public class SearchOptions : ICloneable
    {
        public SearchOptions()
        {
            Timeout = RetrySettings.Timeout;
            RetryInterval = RetrySettings.Interval;
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

        public static SearchOptions SafelyAtOnce(bool isSafely = true)
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

        public static SearchOptions Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            return new SearchOptions { Timeout = timeout, RetryInterval = retryInterval ?? RetrySettings.Interval };
        }

        public static SearchOptions Within(double timeoutSeconds, double? retryIntervalSeconds)
        {
            return new SearchOptions { Timeout = TimeSpan.FromSeconds(timeoutSeconds), RetryInterval = retryIntervalSeconds.HasValue ? TimeSpan.FromSeconds(retryIntervalSeconds.Value) : RetrySettings.Interval };
        }

        public static SearchOptions SafelyWithin(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            return new SearchOptions { IsSafely = true, Timeout = timeout, RetryInterval = retryInterval ?? RetrySettings.Interval };
        }

        public static SearchOptions SafelyWithin(double timeoutSeconds, double? retryIntervalSeconds)
        {
            return new SearchOptions { IsSafely = true, Timeout = TimeSpan.FromSeconds(timeoutSeconds), RetryInterval = retryIntervalSeconds.HasValue ? TimeSpan.FromSeconds(retryIntervalSeconds.Value) : RetrySettings.Interval };
        }

        public static SearchOptions AtOnce()
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
