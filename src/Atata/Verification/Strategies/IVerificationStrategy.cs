using System;

namespace Atata
{
    public interface IVerificationStrategy
    {
        string VerificationKind { get; }

        TimeSpan DefaultTimeout { get; }

        TimeSpan DefaultRetryInterval { get; }

        void ReportFailure(string message, Exception exception);
    }
}
