using System;

namespace Atata
{
    public interface IVerificationStrategy
    {
        /// <summary>
        /// Gets the text describing the kind of the verification.
        /// </summary>
        string VerificationKind { get; }

        TimeSpan DefaultTimeout { get; }

        TimeSpan DefaultRetryInterval { get; }

        void ReportFailure(string message, Exception exception);
    }
}
