namespace Atata;

/// <summary>
/// Represents an interface of strategy for warning assertion reporting.
/// </summary>
public interface IWarningReportStrategy
{
    /// <summary>
    /// Reports the assertion failure of warning kind.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="stackTrace">The stack trace.</param>
    void Report(string message, string stackTrace);
}
