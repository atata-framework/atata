namespace Atata;

[Obsolete("Instead create custom factory implementing Atata.IAggregateAssertionExceptionFactory interface.")] // Obsolete since v4.0.0.
internal sealed class TypeBasedAggregateAssertionExceptionFactory : IAggregateAssertionExceptionFactory
{
    private readonly Type _exceptionType;

    public TypeBasedAggregateAssertionExceptionFactory(Type exceptionType)
    {
        Guard.ThrowIfNull(exceptionType);
        _exceptionType = exceptionType;
    }

    public Exception Create(IEnumerable<AssertionResult> results) =>
        (Exception)Activator.CreateInstance(_exceptionType, results)!;
}
