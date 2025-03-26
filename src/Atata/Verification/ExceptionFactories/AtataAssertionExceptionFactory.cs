#nullable enable

namespace Atata;

public sealed class AtataAssertionExceptionFactory : IAssertionExceptionFactory
{
    public static AtataAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(string message, Exception? innerException) =>
        new AssertionException(message, innerException);
}
