namespace Atata;

public sealed class AtataAggregateAssertionExceptionFactory : IAggregateAssertionExceptionFactory
{
    public static AtataAggregateAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(IEnumerable<AssertionResult> results) =>
        new AggregateAssertionException(results);
}
