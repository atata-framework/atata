using System;
using OpenQA.Selenium;

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
                Timeout = AtataContext.Current.VerificationTimeout;
                RetryInterval = AtataContext.Current.VerificationRetryInterval;

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

        public TVerificationProvider Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            Timeout = timeout;
            RetryInterval = retryInterval ?? RetryInterval;

            return (TVerificationProvider)this;
        }

        public TVerificationProvider Within(double timeoutSeconds, double? retryIntervalSeconds = null)
        {
            return Within(TimeSpan.FromSeconds(timeoutSeconds), retryIntervalSeconds.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(retryIntervalSeconds.Value) : null);
        }

        string IVerificationProvider<TOwner>.GetShouldText()
        {
            return isNegation ? "should not" : "should";
        }

        RetryOptions IVerificationProvider<TOwner>.GetRetryOptions() => GetRetryOptions();

        protected virtual RetryOptions GetRetryOptions()
        {
            return new RetryOptions
            {
                Timeout = Timeout ?? AtataContext.Current.VerificationTimeout,
                Interval = RetryInterval ?? AtataContext.Current.VerificationRetryInterval
            }.
            IgnoringStaleElementReferenceException().
            IgnoringExceptionType(typeof(NoSuchElementException));
        }
    }
}
