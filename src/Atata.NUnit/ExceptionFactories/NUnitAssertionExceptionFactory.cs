namespace Atata.NUnit;

public class NUnitAssertionExceptionFactory : IAssertionExceptionFactory
{
    public static NUnitAssertionExceptionFactory Instance { get; } = new();

    public Exception Create(string message, Exception? innerException)
    {
        string completeMessageWithInnerException = VerificationUtils.AppendExceptionToFailureMessage(message, innerException);
        return new global::NUnit.Framework.AssertionException(completeMessageWithInnerException);
    }
}
