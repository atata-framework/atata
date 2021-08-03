using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Atata.Tests
{
    public sealed class StopwatchAsserter : IDisposable
    {
        private readonly TimeSpan _expectedTime;

        private readonly TimeSpan _upperToleranceTime;

        private readonly Stopwatch _watch;

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime)
        {
            _expectedTime = expectedTime;
            _upperToleranceTime = upperToleranceTime;

            _watch = Stopwatch.StartNew();
        }

        public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1)
        {
            return new StopwatchAsserter(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(upperToleranceSeconds));
        }

        public void Dispose()
        {
            _watch.Stop();
            Assert.That(_watch.Elapsed, Is.InRange(_expectedTime, _expectedTime + _upperToleranceTime));
        }
    }
}
