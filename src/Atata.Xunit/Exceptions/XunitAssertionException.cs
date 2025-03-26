using Atata;
using Atata.Xunit;

namespace Xunit.Sdk;

[Serializable]
public class XunitAssertionException : AssertionException, IAssertionException
{
    public XunitAssertionException()
    {
    }

    public XunitAssertionException(string message)
        : base(message)
    {
    }

    public XunitAssertionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected XunitAssertionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
