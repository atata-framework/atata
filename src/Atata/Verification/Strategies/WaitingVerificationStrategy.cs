using System;

namespace Atata
{
    /// <summary>
    /// Represents the verification strategy for waitings.
    /// Its <see cref="ReportFailure(string, Exception)"/> method throws <see cref="TimeoutException"/>.
    /// </summary>
    public class WaitingVerificationStrategy : IVerificationStrategy
    {
        public string VerificationKind => "Wait";

        public TimeSpan DefaultTimeout => AtataContext.Current.WaitingTimeout;

        public TimeSpan DefaultRetryInterval => AtataContext.Current.WaitingRetryInterval;

        public void ReportFailure(string message, Exception exception)
        {
            string completeMessage = $"Timed out waiting for {message}";
            throw new TimeoutException(completeMessage, exception);
        }
    }
}
