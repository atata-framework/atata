using System;

namespace Atata
{
    public class SearchOptions : ICloneable
    {
        private TimeSpan? timeout;

        private TimeSpan? retryInterval;

        private Visibility? visibility;

        private bool? isSafely;

        public TimeSpan Timeout
        {
            get { return timeout ?? RetrySettings.Timeout; }
            set { timeout = value; }
        }

        public bool IsTimeoutSet => timeout.HasValue;

        public TimeSpan RetryInterval
        {
            get { return retryInterval ?? RetrySettings.Interval; }
            set { retryInterval = value; }
        }

        public bool IsRetryIntervalSet => retryInterval.HasValue;

        /// <summary>
        /// Gets or sets the visibility of the search element. The default value is Visible.
        /// </summary>
        public Visibility Visibility
        {
            get { return visibility ?? Visibility.Visible; }
            set { visibility = value; }
        }

        public bool IsVisibilitySet => visibility.HasValue;

        /// <summary>
        /// Gets or sets a value indicating whether the search element is safely searching. If it is true then null is returned after the search, otherwise an exception is thrown. The default value is false.
        /// </summary>
        public bool IsSafely
        {
            get { return isSafely ?? false; }
            set { isSafely = value; }
        }

        public bool IsSafelySet => isSafely.HasValue;

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

        public static SearchOptions OfVisibility(Visibility visibility)
        {
            return new SearchOptions { Visibility = visibility };
        }

        public static SearchOptions Visible()
        {
            return new SearchOptions { Visibility = Visibility.Visible };
        }

        public static SearchOptions Hidden()
        {
            return new SearchOptions { Visibility = Visibility.Hidden };
        }

        public static SearchOptions OfAnyVisibility()
        {
            return new SearchOptions { Visibility = Visibility.Any };
        }

        public static SearchOptions Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            SearchOptions options = new SearchOptions { Timeout = timeout };

            if (retryInterval.HasValue)
                options.RetryInterval = retryInterval.Value;

            return options;
        }

        public static SearchOptions Within(double timeoutSeconds, double? retryIntervalSeconds)
        {
            SearchOptions options = new SearchOptions { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

            if (retryIntervalSeconds.HasValue)
                options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

            return options;
        }

        public static SearchOptions SafelyWithin(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            SearchOptions options = new SearchOptions { IsSafely = true, Timeout = timeout };

            if (retryInterval.HasValue)
                options.RetryInterval = retryInterval.Value;

            return options;
        }

        public static SearchOptions SafelyWithin(double timeoutSeconds, double? retryIntervalSeconds)
        {
            SearchOptions options = new SearchOptions { IsSafely = true, Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

            if (retryIntervalSeconds.HasValue)
                options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

            return options;
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
