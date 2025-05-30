using Xunit.Sdk;

namespace Atata.Xunit;

public sealed class XunitAggregateAssertionExceptionFactory : IAggregateAssertionExceptionFactory
{
    public static XunitAggregateAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(IEnumerable<AssertionResult> results) =>
        new XunitAggregateAssertionException(results);
}
