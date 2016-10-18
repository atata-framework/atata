using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Atata.Tests
{
    public class StopwatchAsserter : IDisposable
    {
        private readonly Stopwatch watch;
        private readonly TimeSpan expectedTime;
        private readonly TimeSpan toleranceTime;

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan toleranceTime)
        {
            this.expectedTime = expectedTime;
            this.toleranceTime = toleranceTime;

            watch = Stopwatch.StartNew();
        }

        public static StopwatchAsserter Within(TimeSpan time, TimeSpan toleranceTime)
        {
            return new StopwatchAsserter(time, toleranceTime);
        }

        public static StopwatchAsserter Within(double seconds, double toleranceSeconds = 0.25)
        {
            return new StopwatchAsserter(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(toleranceSeconds));
        }

        public void Dispose()
        {
            watch.Stop();
            Assert.That(watch.Elapsed, Is.EqualTo(expectedTime).Within(toleranceTime));
        }
    }
}
