namespace Atata.MSTest;

public sealed class MSTestAssertionExceptionFactory : IAssertionExceptionFactory
{
    public static MSTestAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(string message, Exception? innerException) =>
        innerException is null
            ? new AssertFailedException(message)
            : new AssertFailedException(message, innerException);
}
