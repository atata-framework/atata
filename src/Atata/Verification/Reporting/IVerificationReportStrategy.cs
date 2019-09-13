using System;

namespace Atata
{
    public interface IVerificationReportStrategy
    {
        string VerificationKind { get; }

        void ReportFailure(string message, Exception exception);
    }
}
