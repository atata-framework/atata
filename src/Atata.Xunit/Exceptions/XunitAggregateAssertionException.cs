using Atata;
using Atata.Xunit;

namespace Xunit.Sdk;

[Serializable]
public class XunitAggregateAssertionException : AggregateAssertionException, IAssertionException
{
    public XunitAggregateAssertionException()
    {
    }

    public XunitAggregateAssertionException(string message)
        : base(message)
    {
    }

    public XunitAggregateAssertionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public XunitAggregateAssertionException(IEnumerable<AssertionResult> results)
        : base(results)
    {
    }

    protected XunitAggregateAssertionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
