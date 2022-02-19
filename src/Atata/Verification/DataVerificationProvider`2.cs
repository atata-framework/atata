using System;

namespace Atata
{
    public class DataVerificationProvider<TData, TOwner> :
        VerificationProvider<DataVerificationProvider<TData, TOwner>, TOwner>,
        IDataVerificationProvider<TData, TOwner>
    {
        public DataVerificationProvider(IObjectProvider<TData, TOwner> dataProvider)
        {
            DataProvider = dataProvider;
        }

        protected IObjectProvider<TData, TOwner> DataProvider { get; }

        IObjectProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

        protected override TOwner Owner
        {
            get { return DataProvider.Owner; }
        }

        public NegationDataVerificationProvider Not => new NegationDataVerificationProvider(DataProvider, this);

        protected override RetryOptions GetRetryOptions() =>
            DataProvider.IsValueDynamic
                ? base.GetRetryOptions()
                : new RetryOptions { Timeout = TimeSpan.Zero, Interval = TimeSpan.Zero };

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
                            actual = DataProvider.Value;

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
                Subject.BuildExceptionName(DataProvider.ProviderName));
        }

        public class NegationDataVerificationProvider :
            NegationVerificationProvider<NegationDataVerificationProvider, TOwner>,
            IDataVerificationProvider<TData, TOwner>
        {
            internal NegationDataVerificationProvider(IObjectProvider<TData, TOwner> dataProvider, IVerificationProvider<TOwner> verificationProvider)
                : base(verificationProvider)
            {
                DataProvider = dataProvider;
            }

            protected IObjectProvider<TData, TOwner> DataProvider { get; }

            IObjectProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

            protected override TOwner Owner => DataProvider.Owner;

            protected override RetryOptions GetRetryOptions() =>
                DataProvider.IsValueDynamic
                    ? base.GetRetryOptions()
                    : new RetryOptions { Timeout = TimeSpan.Zero, Interval = TimeSpan.Zero };

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
                                var actual = DataProvider.Value;

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
