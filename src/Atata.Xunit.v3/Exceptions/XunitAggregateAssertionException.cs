using Atata;
using Atata.Xunit;

namespace Xunit.Sdk;

public class XunitAggregateAssertionException : AggregateAssertionException, IAssertionException
{
    public XunitAggregateAssertionException()
    {
    }

    public XunitAggregateAssertionException(string? message)
        : base(message)
    {
    }

    public XunitAggregateAssertionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public XunitAggregateAssertionException(IEnumerable<AssertionResult> results)
        : base(results)
    {
    }
}
