using System;

namespace Atata
{
    public static class RetrySettings
    {
        static RetrySettings()
        {
            Timeout = TimeSpan.FromSeconds(10);
            RetryInterval = TimeSpan.FromSeconds(0.5);
        }

        public static TimeSpan Timeout { get; internal set; }
        public static TimeSpan RetryInterval { get; internal set; }
    }
}
