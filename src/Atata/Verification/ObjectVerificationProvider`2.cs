using System;

namespace Atata
{
    public class ObjectVerificationProvider<TObject, TOwner> :
        VerificationProvider<ObjectVerificationProvider<TObject, TOwner>, TOwner>,
        IObjectVerificationProvider<TObject, TOwner>
    {
        public ObjectVerificationProvider(IObjectProvider<TObject, TOwner> objectProvider)
        {
            ObjectProvider = objectProvider;
        }

        protected IObjectProvider<TObject, TOwner> ObjectProvider { get; }

        IObjectProvider<TObject, TOwner> IObjectVerificationProvider<TObject, TOwner>.ObjectProvider =>
            ObjectProvider;

        protected override TOwner Owner =>
            ObjectProvider.Owner;

        public NegationObjectVerificationProvider Not => new NegationObjectVerificationProvider(ObjectProvider, this);

        protected override (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions() =>
            ObjectProvider.IsDynamic
                ? base.GetRetryOptions()
                : (Timeout: TimeSpan.Zero, RetryInterval: TimeSpan.Zero);

        public Subject<TException> Throw<TException>()
            where TException : Exception
        {
            string expectedMessage = $"throw exception of {typeof(TException).FullName} type";

            Exception exception = null;

            void ExecuteVerification()
            {
                object actual = default;

                bool doesSatisfy = VerificationUtils.ExecuteUntil(
                    () =>
                    {
                        try
                        {
                            actual = ObjectProvider.Object;

                            if (actual is Action actualAsAction)
                            {
                                actualAsAction.Invoke();
                                actual = null;
                            }

                            exception = null;

                            return false;
                        }
                        catch (Exception e)
                        {
                            exception = e;
                            return e is TException;
                        }
                    },
                    GetRetryOptions());

                if (!doesSatisfy)
                {
                    string actualMessage = exception is null ? "no exception" : exception.ToString();

                    string failureMessage = VerificationUtils.BuildFailureMessage(this, expectedMessage, actualMessage);

                    ReportFailure(failureMessage, null);
                }
            }

            VerificationUtils.Verify(this, ExecuteVerification, expectedMessage);

            return new Subject<TException>(
                exception as TException,
                Subject.BuildExceptionName(ObjectProvider.ProviderName));
        }

        public class NegationObjectVerificationProvider :
            NegationVerificationProvider<NegationObjectVerificationProvider, TOwner>,
            IObjectVerificationProvider<TObject, TOwner>
        {
            internal NegationObjectVerificationProvider(IObjectProvider<TObject, TOwner> objectProvider, IVerificationProvider<TOwner> verificationProvider)
                : base(verificationProvider)
            {
                ObjectProvider = objectProvider;
            }

            protected IObjectProvider<TObject, TOwner> ObjectProvider { get; }

            IObjectProvider<TObject, TOwner> IObjectVerificationProvider<TObject, TOwner>.ObjectProvider =>
                ObjectProvider;

            protected override TOwner Owner =>
                ObjectProvider.Owner;

            protected override (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions() =>
                ObjectProvider.IsDynamic
                    ? base.GetRetryOptions()
                    : (Timeout: TimeSpan.Zero, RetryInterval: TimeSpan.Zero);

            public TOwner Throw()
            {
                string expectedMessage = $"throw exception";

                void ExecuteVerification()
                {
                    Exception exception = null;

                    bool doesSatisfy = VerificationUtils.ExecuteUntil(
                        () =>
                        {
                            try
                            {
                                var actual = ObjectProvider.Object;

                                if (actual is Action actualAsAction)
                                    actualAsAction.Invoke();

                                exception = null;

                                return true;
                            }
                            catch (Exception e)
                            {
                                exception = e;
                                return false;
                            }
                        },
                        GetRetryOptions());

                    if (!doesSatisfy)
                    {
                        string actualMessage = exception.ToString();

                        string failureMessage = VerificationUtils.BuildFailureMessage(this, expectedMessage, actualMessage);

                        ReportFailure(failureMessage, null);
                    }
                }

                return VerificationUtils.Verify(this, ExecuteVerification, expectedMessage);
            }
        }
    }
}
