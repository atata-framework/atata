using OpenQA.Selenium;
using System;

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
            RetrySettings.RetryInterval = retryInterval;
            return timeouts;
        }
    }
}
