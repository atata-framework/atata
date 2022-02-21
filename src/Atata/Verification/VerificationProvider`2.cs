using System;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class VerificationProvider<TVerificationProvider, TOwner> : IVerificationProvider<TOwner>
        where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
    {
        private readonly bool _isNegation;

        protected VerificationProvider(bool isNegation = false)
        {
            _isNegation = isNegation;
        }

        bool IVerificationProvider<TOwner>.IsNegation => _isNegation;

        protected IVerificationStrategy Strategy { get; set; } = new AssertionVerificationStrategy();

        IVerificationStrategy IVerificationProvider<TOwner>.Strategy
        {
            get => Strategy;
            set => Strategy = value;
        }

        TOwner IVerificationProvider<TOwner>.Owner
        {
            get { return Owner; }
        }

        protected abstract TOwner Owner { get; }

        string IVerificationProvider<TOwner>.VerificationKind => Strategy.VerificationKind;

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

        public TVerificationProvider WithRetry
        {
            get
            {
                Timeout = Strategy.DefaultTimeout;
                RetryInterval = Strategy.DefaultRetryInterval;

                return (TVerificationProvider)this;
            }
        }

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

        string IVerificationProvider<TOwner>.GetShouldText() => GetShouldText();

        protected string GetShouldText() =>
            _isNegation ? "should not" : "should";

        RetryOptions IVerificationProvider<TOwner>.GetRetryOptions() => GetRetryOptions();

        protected virtual RetryOptions GetRetryOptions()
        {
            return new RetryOptions
            {
                Timeout = Timeout ?? Strategy.DefaultTimeout,
                Interval = RetryInterval ?? Strategy.DefaultRetryInterval
            }.
            IgnoringStaleElementReferenceException().
            IgnoringExceptionType(typeof(NoSuchElementException));
        }

        void IVerificationProvider<TOwner>.ReportFailure(string message, Exception exception) =>
            ReportFailure(message, exception);

        protected void ReportFailure(string message, Exception exception) =>
            Strategy.ReportFailure(message, exception);
    }
}
