using System;

namespace Atata
{
    public interface IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        bool IsNegation { get; }
        TOwner Owner { get; }
        TimeSpan? Timeout { get; }
        TimeSpan? RetryInterval { get; }
    }
}
