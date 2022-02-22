using System;

namespace Atata
{
    public interface IVerificationStrategy
    {
        /// <summary>
        /// Gets the text describing the kind of the verification.
        /// </summary>
        string VerificationKind { get; }

        /// <summary>
        /// Gets the default timeout.
        /// </summary>
        TimeSpan DefaultTimeout { get; }

        /// <summary>
        /// Gets the default retry interval.
        /// </summary>
        TimeSpan DefaultRetryInterval { get; }

        /// <summary>
        /// Reports the failure.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void ReportFailure(string message, Exception exception);
    }
}
