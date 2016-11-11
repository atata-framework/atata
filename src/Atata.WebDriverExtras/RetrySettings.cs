using System;

namespace Atata
{
    public static class RetrySettings
    {
        [ThreadStatic]
        private static TimeSpan? timeout;

        [ThreadStatic]
        private static TimeSpan? interval;

        /// <summary>
        /// Gets the retry timeout. The default value is 10 seconds.
        /// </summary>
        public static TimeSpan Timeout
        {
            get { return (TimeSpan)(timeout ?? (timeout = TimeSpan.FromSeconds(10))); }
            internal set { timeout = value; }
        }

        /// <summary>
        /// Gets the retry interval. The default value is 500 milliseconds.
        /// </summary>
        public static TimeSpan Interval
        {
            get { return (TimeSpan)(interval ?? (interval = TimeSpan.FromSeconds(0.5))); }
            internal set { interval = value; }
        }
    }
}
