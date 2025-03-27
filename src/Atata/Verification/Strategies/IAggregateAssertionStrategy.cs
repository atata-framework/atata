#nullable enable

namespace Atata;

/// <summary>
/// Represents an interface for aggregate assertion strategy.
/// </summary>
public interface IAggregateAssertionStrategy
{
    /// <summary>
    /// Executes the specified action in aggregate assertion mode.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <param name="action">The action.</param>
    void Assert(IAtataExecutionUnit? executionUnit, Action action);

    /// <summary>
    /// Reports the assertion failure.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <param name="message">The message.</param>
    /// <param name="stackTrace">The stack trace.</param>
    void ReportFailure(IAtataExecutionUnit? executionUnit, string message, string stackTrace);
}
