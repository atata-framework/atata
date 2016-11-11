using System;
using OpenQA.Selenium;

namespace Atata
{
    public static class ITimeoutsExtensions
    {
        public static ITimeouts SetRetryTimeout(this ITimeouts timeouts, TimeSpan timeToWait)
        {
            RetrySettings.Timeout = timeToWait;
            return timeouts;
        }

        public static ITimeouts SetRetryTimeout(this ITimeouts timeouts, TimeSpan timeToWait, TimeSpan retryInterval)
        {
            RetrySettings.Timeout = timeToWait;
            RetrySettings.Interval = retryInterval;
            return timeouts;
        }
    }
}
