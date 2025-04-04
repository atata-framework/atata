namespace Atata;

public interface IAggregateAssertionExceptionFactory
{
    Exception Create(IEnumerable<AssertionResult> results);
}
