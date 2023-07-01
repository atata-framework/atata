namespace Atata;

/// <summary>
/// Represents an interface for aggregate assertion strategy.
/// </summary>
public interface IAggregateAssertionStrategy
{
    /// <summary>
    /// Executes the specified action in aggregate assertion mode.
    /// </summary>
    /// <param name="action">The action.</param>
    void Assert(Action action);

    /// <summary>
    /// Reports the assertion failure.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="stackTrace">The stack trace.</param>
    void ReportFailure(string message, string stackTrace);
}
