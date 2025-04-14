namespace Atata;

[Obsolete("Instead create custom factory implementing Atata.IAssertionExceptionFactory interface.")] // Obsolete since v4.0.0.
internal sealed class TypeBasedAssertionExceptionFactory : IAssertionExceptionFactory
{
    private readonly Type _exceptionType;

    public TypeBasedAssertionExceptionFactory(Type exceptionType)
    {
        Guard.ThrowIfNull(exceptionType);
        _exceptionType = exceptionType;
    }

    public Exception Create(string message, Exception? innerException) =>
        (Exception)Activator.CreateInstance(_exceptionType, message, innerException);
}
