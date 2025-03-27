#nullable enable

namespace Atata;

public class ObjectVerificationProvider<TObject, TOwner> :
    VerificationProvider<ObjectVerificationProvider<TObject, TOwner>, TOwner>,
    IObjectVerificationProvider<TObject, TOwner>
{
    public ObjectVerificationProvider(IObjectProvider<TObject, TOwner> objectProvider, IAtataExecutionUnit? executionUnit)
        : base(executionUnit) =>
        ObjectProvider = objectProvider;

    protected IObjectProvider<TObject, TOwner> ObjectProvider { get; }

    IObjectProvider<TObject, TOwner> IObjectVerificationProvider<TObject, TOwner>.ObjectProvider =>
        ObjectProvider;

    protected override TOwner Owner =>
        ObjectProvider.Owner;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public NegationObjectVerificationProvider Not =>
        new(ObjectProvider, this);

    protected override (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions() =>
        ObjectProvider.IsDynamic
            ? base.GetRetryOptions()
            : (Timeout: TimeSpan.Zero, RetryInterval: TimeSpan.Zero);

    public Subject<TException> Throw<TException>()
        where TException : Exception
        =>
        Throw<TException>(false, null);

    public Subject<TException> Throw<TException>(string messageWildcardPattern)
        where TException : Exception
        =>
        Throw<TException>(false, messageWildcardPattern);

    public Subject<TException> ThrowExactly<TException>()
        where TException : Exception
        =>
        Throw<TException>(true, null);

    public Subject<TException> ThrowExactly<TException>(string messageWildcardPattern)
        where TException : Exception
        =>
        Throw<TException>(true, messageWildcardPattern);

    private Subject<TException> Throw<TException>(bool exactly, string? messageWildcardPattern)
        where TException : Exception
    {
        StringBuilder expectedMessageBuilder = new("throw ");

        if (exactly)
            expectedMessageBuilder.Append("exactly ");

        expectedMessageBuilder.Append("exception of ")
            .Append(typeof(TException).FullName)
            .Append(" type");

        if (messageWildcardPattern is not null)
            expectedMessageBuilder.Append($" with message matching wildcard pattern \"{messageWildcardPattern}\"");

        string expectedMessage = expectedMessageBuilder.ToString();

        Exception? exception = null;

        void ExecuteVerification()
        {
            object? actual = null;

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
                        return (exactly ? e.GetType() == typeof(TException) : e is TException)
                            && (messageWildcardPattern is null || WildcardPattern.IsMatch(e.Message, messageWildcardPattern, ResolveStringComparison()));
                    }
                },
                GetRetryOptions());

            if (!doesSatisfy)
            {
                string actualMessage = exception is null ? "no exception" : exception.ToString();

                string failureMessage = VerificationUtils.BuildFailureMessage(this, expectedMessage, actualMessage);

                Strategy.ReportFailure(ExecutionUnit, failureMessage, null);
            }
        }

        VerificationUtils.Verify(this, ExecuteVerification, expectedMessage);

        return new Subject<TException>(
            (exception as TException)!,
            Subject.BuildExceptionName(ObjectProvider.ProviderName),
            ExecutionUnit);
    }

    public class NegationObjectVerificationProvider :
        NegationVerificationProvider<NegationObjectVerificationProvider, TOwner>,
        IObjectVerificationProvider<TObject, TOwner>
    {
        internal NegationObjectVerificationProvider(IObjectProvider<TObject, TOwner> objectProvider, IVerificationProvider<TOwner> verificationProvider)
            : base(verificationProvider) =>
            ObjectProvider = objectProvider;

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
                Exception? exception = null;

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
                    string actualMessage = exception!.ToString();

                    string failureMessage = VerificationUtils.BuildFailureMessage(this, expectedMessage, actualMessage);

                    Strategy.ReportFailure(ExecutionUnit, failureMessage, null);
                }
            }

            return VerificationUtils.Verify(this, ExecuteVerification, expectedMessage);
        }
    }
}
