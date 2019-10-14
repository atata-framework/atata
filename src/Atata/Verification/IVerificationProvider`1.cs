using System;

namespace Atata
{
    public interface IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        bool IsNegation { get; }

        TOwner Owner { get; }

        string VerificationKind { get; }

        // TODO: Should be non-nullable.
        TimeSpan? Timeout { get; set; }

        // TODO: Should be non-nullable.
        TimeSpan? RetryInterval { get; set; }

        string GetShouldText();

        RetryOptions GetRetryOptions();

        void ReportFailure(string message, Exception exception = null);
    }
}
