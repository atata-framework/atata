using System;

namespace Atata
{
    public static class RetrySettings
    {
        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <value>
        /// The timeout. The default is 10 seconds.
        /// </value>
        public static TimeSpan Timeout { get; internal set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the retry interval.
        /// </summary>
        /// <value>
        /// The retry interval. The default is 500 milliseconds.
        /// </value>
        public static TimeSpan RetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);
    }
}
