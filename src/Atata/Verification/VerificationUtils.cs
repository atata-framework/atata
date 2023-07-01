namespace Atata;

public static class VerificationUtils
{
    public static Exception CreateAssertionException(string message, Exception innerException = null)
    {
        var exceptionType = AtataContext.Current?.AssertionExceptionType;

        if (exceptionType == null)
            return new AssertionException(message, innerException);
        else if (exceptionType.FullName == NUnitAdapter.AssertionExceptionTypeName)
            return (Exception)Activator.CreateInstance(exceptionType, AppendExceptionToFailureMessage(message, innerException));
        else
            return (Exception)Activator.CreateInstance(exceptionType, message, innerException);
    }

    public static Exception CreateAggregateAssertionException(IEnumerable<AssertionResult> assertionResults)
    {
        var exceptionType = AtataContext.Current?.AggregateAssertionExceptionType;

        return exceptionType != null
            ? (Exception)Activator.CreateInstance(exceptionType, assertionResults)
            : new AggregateAssertionException(assertionResults);
    }

    public static string BuildExpectedMessage(string message, object[] args) =>
        args != null && args.Any()
            ? message.FormatWith(args.Select(x => Stringifier.ToString(x)).ToArray())
            : message;

    public static string BuildConstraintMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string message, params TData[] args)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            string formattedMessage;

            if (args != null && args.Any())
            {
                string[] convertedArgs = args
                    .Select(x => ConvertValueToString(verifier.ObjectProvider, x))
                    .ToArray();

                formattedMessage = message.FormatWith(convertedArgs);
            }
            else
            {
                formattedMessage = message;
            }

            return $"{ResolveShouldText(verifier)} {formattedMessage}";
        }
        else
        {
            return null;
        }
    }

    private static string ConvertValueToString<TData, TOwner>(IObjectProvider<TData, TOwner> provider, TData value) =>
        provider is IConvertsValueToString<TData> providerAsConverter && value is not bool
            ? $"\"{providerAsConverter.ConvertValueToString(value)}\""
            : Stringifier.ToString(value);

    public static string BuildFailureMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string expected, string actual) =>
        BuildFailureMessage(verifier, expected, actual, true);

    public static string BuildFailureMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string expected, string actual, bool prependShouldTextToExpected)
    {
        StringBuilder builder = new StringBuilder()
            .Append($"{verifier.ObjectProvider.ProviderName}")
            .AppendLine()
            .Append("Expected: ");

        if (prependShouldTextToExpected)
            builder.Append(ResolveShouldText(verifier)).Append(' ');

        builder.Append(expected);

        if (actual != null)
            builder.AppendLine().Append($"  Actual: {actual}");

        return builder.ToString();
    }

    internal static string AppendExceptionToFailureMessage(string message, Exception exception)
    {
        if (exception != null)
        {
            StringBuilder builder = new StringBuilder(message)
                .AppendLine()
                .Append("  ----> ")
                .Append(exception.ToString());

            return builder.ToString();
        }
        else
        {
            return message;
        }
    }

    internal static string BuildStackTraceForAggregateAssertion()
    {
        string stackTrace = new StackTrace(1, true).ToString();

        return StackTraceFilter.TakeBeforeInvokeMethodOfRuntimeMethodHandle(stackTrace);
    }

    internal static bool ExecuteUntil(Func<bool> condition, (TimeSpan Timeout, TimeSpan RetryInterval) retryOptions)
    {
        var wait = CreateSafeWait(retryOptions);
        return wait.Until(_ => condition());
    }

    private static SafeWait<object> CreateSafeWait((TimeSpan Timeout, TimeSpan RetryInterval) retryOptions) =>
        new(string.Empty)
        {
            Timeout = retryOptions.Timeout,
            PollingInterval = retryOptions.RetryInterval
        };

    internal static TOwner Verify<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, Action verificationAction, string expectedMessage, params TData[] arguments)
    {
        if (AtataContext.Current is null)
        {
            verificationAction.Invoke();
        }
        else
        {
            string verificationConstraintMessage = BuildConstraintMessage(verifier, expectedMessage, arguments);

            LogSection logSection = new VerificationLogSection(
                verifier.Strategy.VerificationKind,
                verifier.ObjectProvider.ProviderName,
                verificationConstraintMessage);

            AtataContext.Current.Log.ExecuteSection(logSection, verificationAction);
        }

        return verifier.Owner;
    }

    public static string ResolveShouldText<TOwner>(IVerificationProvider<TOwner> verifier) =>
        verifier.IsNegation ? "should not" : "should";
}
