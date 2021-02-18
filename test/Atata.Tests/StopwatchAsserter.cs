using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Atata.Tests
{
    public sealed class StopwatchAsserter : IDisposable
    {
        private readonly TimeSpan expectedTime;

        private readonly TimeSpan upperToleranceTime;

        private readonly Stopwatch watch;

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime)
        {
            this.expectedTime = expectedTime;
            this.upperToleranceTime = upperToleranceTime;

            watch = Stopwatch.StartNew();
        }

        public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1)
        {
            return new StopwatchAsserter(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(upperToleranceSeconds));
        }

        public void Dispose()
        {
            watch.Stop();
            Assert.That(watch.Elapsed, Is.InRange(expectedTime, expectedTime + upperToleranceTime));
        }
    }
}
