using Xunit.Sdk;

namespace Atata.Xunit;

public sealed class XunitAssertionExceptionFactory : IAssertionExceptionFactory
{
    public static XunitAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(string message, Exception? innerException) =>
        new XunitAssertionException(message, innerException);
}
