using Atata;
using Atata.Xunit;

namespace Xunit.Sdk;

public class XunitAssertionException : AssertionException, IAssertionException
{
    public XunitAssertionException()
    {
    }

    public XunitAssertionException(string? message)
        : base(message)
    {
    }

    public XunitAssertionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
