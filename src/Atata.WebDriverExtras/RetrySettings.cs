using System;

namespace Atata
{
    public static class RetrySettings
    {
        /// <summary>
        /// Gets the timeout. The default value is 10 seconds.
        /// </summary>
        public static TimeSpan Timeout { get; internal set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the retry interval. The default value is 500 milliseconds.
        /// </summary>
        public static TimeSpan RetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);
    }
}
