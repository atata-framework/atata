using System;

namespace Atata
{
    public abstract class VerificationProvider<TVerificationProvider, TOwner> : IVerificationProvider<TOwner>
        where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly bool isNegation;

        protected VerificationProvider(bool isNegation = false)
        {
            this.isNegation = isNegation;
        }

        bool IVerificationProvider<TOwner>.IsNegation => isNegation;

        TOwner IVerificationProvider<TOwner>.Owner
        {
            get { return Owner; }
        }

        protected abstract TOwner Owner { get; }

        protected TimeSpan? Timeout { get; private set; }
        protected TimeSpan? RetryInterval { get; private set; }

        TimeSpan? IVerificationProvider<TOwner>.Timeout => Timeout;

        TimeSpan? IVerificationProvider<TOwner>.RetryInterval => RetryInterval;

        public TVerificationProvider WithRetry
        {
            get
            {
                return (TVerificationProvider)this;
            }
        }

        public TVerificationProvider Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            return (TVerificationProvider)this;
        }

        public TVerificationProvider WithinSeconds(double timeoutSeconds, double? retryIntervalSeconds = null)
        {
            return (TVerificationProvider)this;
        }
    }
}
