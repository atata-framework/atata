namespace Atata.UnitTests;

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

    public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1.95) =>
        new(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(upperToleranceSeconds));

    public static StopwatchAsserter WithinMilliseconds(double milliseconds, double upperToleranceMilliseconds = 1.95) =>
        new(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(upperToleranceMilliseconds));

    public void Dispose()
    {
        _watch.Stop();
        Assert.That(_watch.Elapsed, Is.InRange(_expectedTime, _expectedTime + _upperToleranceTime));
    }
}
