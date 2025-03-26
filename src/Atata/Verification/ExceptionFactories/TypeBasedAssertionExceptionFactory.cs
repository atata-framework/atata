#nullable enable

namespace Atata;

[Obsolete("Instead create custom factory implementing Atata.IAssertionExceptionFactory interface.")] // Obsolete since v4.0.0.
public sealed class TypeBasedAssertionExceptionFactory : IAssertionExceptionFactory
{
    private readonly Type _exceptionType;

    public TypeBasedAssertionExceptionFactory(Type exceptionType) =>
        _exceptionType = exceptionType.CheckNotNull(nameof(exceptionType));

    public Exception Create(string message, Exception? innerException) =>
        (Exception)Activator.CreateInstance(_exceptionType, message, innerException);
}
