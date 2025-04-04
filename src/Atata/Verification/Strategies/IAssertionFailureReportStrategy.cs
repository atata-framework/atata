namespace Atata;

/// <summary>
/// Represents an interface of strategy for assertion failure reporting.
/// </summary>
public interface IAssertionFailureReportStrategy
{
    /// <summary>
    /// Reports the assertion failure.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <param name="message">The assertion failure message.</param>
    /// <param name="exception">The exception occurred during assertion.</param>
    /// <param name="stackTrace">The stack trace of assertion failure.</param>
    void Report(IAtataExecutionUnit? executionUnit, string message, Exception? exception, string stackTrace);
}
