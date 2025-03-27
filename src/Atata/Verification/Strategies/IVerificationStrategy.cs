#nullable enable

namespace Atata;

public interface IVerificationStrategy
{
    /// <summary>
    /// Gets the text describing the kind of the verification.
    /// </summary>
    string VerificationKind { get; }

    /// <summary>
    /// Gets the default timeout.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <returns>A timeout.</returns>
    TimeSpan GetDefaultTimeout(IAtataExecutionUnit? executionUnit);

    /// <summary>
    /// Gets the default retry interval.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <returns>A retry interval.</returns>
    TimeSpan GetDefaultRetryInterval(IAtataExecutionUnit? executionUnit);

    /// <summary>
    /// Reports the failure.
    /// </summary>
    /// <param name="executionUnit">The execution unit.</param>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void ReportFailure(IAtataExecutionUnit? executionUnit, string message, Exception? exception);
}
