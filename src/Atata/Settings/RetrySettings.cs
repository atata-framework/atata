using Humanizer;
using System;

namespace Atata
{
    public static class RetrySettings
    {
        static RetrySettings()
        {
            Timeout = 10.Seconds();
            RetryInterval = 500.Milliseconds();
        }

        public static TimeSpan Timeout { get; internal set; }
        public static TimeSpan RetryInterval { get; internal set; }
    }
}
