using System;

namespace Atata
{
    public abstract class VerificationProvider<TVerificationProvider, TOwner> : IVerificationProvider<TOwner>
        where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
    {
        private readonly bool _isNegation;

        protected VerificationProvider(bool isNegation = false) =>
            _isNegation = isNegation;

        bool IVerificationProvider<TOwner>.IsNegation => _isNegation;

        protected IVerificationStrategy Strategy { get; set; } = new AssertionVerificationStrategy();

        IVerificationStrategy IVerificationProvider<TOwner>.Strategy
        {
            get => Strategy;
            set => Strategy = value;
        }

        TOwner IVerificationProvider<TOwner>.Owner => Owner;

        protected abstract TOwner Owner { get; }

        protected internal TimeSpan? Timeout { get; internal set; }

        protected internal TimeSpan? RetryInterval { get; internal set; }

        TimeSpan? IVerificationProvider<TOwner>.Timeout
        {
            get => Timeout;
            set => Timeout = value;
        }

        TimeSpan? IVerificationProvider<TOwner>.RetryInterval
        {
            get => RetryInterval;
            set => RetryInterval = value;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TVerificationProvider WithRetry
        {
            get
            {
                Timeout = Strategy.DefaultTimeout;
                RetryInterval = Strategy.DefaultRetryInterval;

                return (TVerificationProvider)this;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TVerificationProvider AtOnce
        {
            get
            {
                Timeout = TimeSpan.Zero;

                return (TVerificationProvider)this;
            }
        }

        public TVerificationProvider Using(IVerificationStrategy strategy)
        {
            Strategy = strategy;

            return (TVerificationProvider)this;
        }

        public TVerificationProvider Using<TVerificationStrategy>()
            where TVerificationStrategy : IVerificationStrategy, new()
        {
            Strategy = new TVerificationStrategy();

            return (TVerificationProvider)this;
        }

        public TVerificationProvider Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            Timeout = timeout;
            RetryInterval = retryInterval ?? RetryInterval;

            return (TVerificationProvider)this;
        }

        [Obsolete("Use " + nameof(WithinSeconds) + " instead.")] // Obsolete since v2.0.0.
        public TVerificationProvider Within(double timeoutSeconds, double? retryIntervalSeconds = null) =>
            WithinSeconds(timeoutSeconds, retryIntervalSeconds);

        public TVerificationProvider WithinSeconds(double timeoutSeconds, double? retryIntervalSeconds = null) =>
            Within(TimeSpan.FromSeconds(timeoutSeconds), retryIntervalSeconds.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(retryIntervalSeconds.Value) : null);

        public TVerificationProvider WithRetryInterval(TimeSpan retryInterval)
        {
            RetryInterval = retryInterval;

            return (TVerificationProvider)this;
        }

        public TVerificationProvider WithRetryIntervalSeconds(double retryIntervalSeconds) =>
            WithRetryInterval(TimeSpan.FromSeconds(retryIntervalSeconds));

        (TimeSpan Timeout, TimeSpan RetryInterval) IVerificationProvider<TOwner>.GetRetryOptions() =>
            GetRetryOptions();

        protected virtual (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions() =>
            (Timeout: Timeout ?? Strategy.DefaultTimeout, RetryInterval: RetryInterval ?? Strategy.DefaultRetryInterval);
    }
}
