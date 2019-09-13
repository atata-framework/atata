using System;

namespace Atata
{
    public class WaitingReportStrategy : IVerificationReportStrategy
    {
        public string VerificationKind => "Wait";

        public void ReportFailure(string message, Exception exception)
        {
            string completeMessage = $"Timed out waiting for {message}";
            throw new TimeoutException(completeMessage, exception);
        }
    }
}
