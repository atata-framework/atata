using System;

namespace Atata
{
    /// <summary>
    /// Represents the result of assertion.
    /// </summary>
    public class AssertionResult
    {
        /// <summary>
        /// Gets the status of assertion.
        /// </summary>
        public AssertionStatus Status { get; private set; }

        /// <summary>
        /// Gets the failure message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the stack trace of assertion.
        /// </summary>
        public string StackTrace { get; private set; }

        public static AssertionResult ForFailure(string message, string stackTrace) =>
            new()
            {
                Status = AssertionStatus.Failed,
                Message = message,
                StackTrace = stackTrace
            };

        public static AssertionResult ForWarning(string message, string stackTrace) =>
            new()
            {
                Status = AssertionStatus.Warning,
                Message = message,
                StackTrace = stackTrace
            };

        public static AssertionResult ForException(Exception exception) =>
            new()
            {
                Status = AssertionStatus.Exception,
                Message = exception.ToString()
            };
    }
}
