namespace Atata;

public static class VerificationUtils
{
    public static Exception CreateAssertionException(AtataContext context, string message, Exception innerException = null)
    {
        Type exceptionType = context?.AssertionExceptionType;

        if (exceptionType is null || exceptionType == typeof(AssertionException))
            return new AssertionException(message, innerException);
        else if (exceptionType.FullName == NUnitAdapter.AssertionExceptionTypeName)
            return (Exception)Activator.CreateInstance(exceptionType, AppendExceptionToFailureMessage(message, innerException));
        else
            return (Exception)Activator.CreateInstance(exceptionType, message, innerException);
    }

    public static Exception CreateAggregateAssertionException(AtataContext context, IEnumerable<AssertionResult> assertionResults)
    {
        Type exceptionType = context?.AggregateAssertionExceptionType;

        return exceptionType is null || exceptionType == typeof(AggregateAssertionException)
            ? new AggregateAssertionException(assertionResults)
            : (Exception)Activator.CreateInstance(exceptionType, assertionResults);
    }

    public static string BuildExpectedMessage(string message, object[] args) =>
        args?.Length > 0
            ? message.FormatWith(args.Select(Stringifier.ToString).ToArray())
            : message;

    public static string BuildConstraintMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string message, params TData[] args)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            string formattedMessage;

            if (args?.Length > 0)
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
                .Append(" ---> ")
                .Append(exception.ToString());

            return builder.ToString();
        }
        else
        {
            return message;
        }
    }

    internal static string AppendStackTraceToFailureMessage(string message, string stackTrace)
    {
        if (!string.IsNullOrEmpty(stackTrace))
        {
            StringBuilder builder = new StringBuilder(message)
                .AppendLine()
                .Append(stackTrace);

            return builder.ToString();
        }
        else
        {
            return message;
        }
    }

    internal static string CreateStackTraceForAssertionFailiure()
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

    internal static TOwner Verify<TData, TOwner>(
        IObjectVerificationProvider<TData, TOwner> verifier,
        Action verificationAction,
        string expectedMessage,
        params TData[] arguments)
    {
        var executionUnit = verifier.ExecutionUnit ?? AtataContext.Current?.ExecutionUnit;

        if (executionUnit is null)
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

            executionUnit.Log.ExecuteSection(logSection, verificationAction);
        }

        return verifier.Owner;
    }

    public static string ResolveShouldText<TOwner>(IVerificationProvider<TOwner> verifier) =>
        verifier.IsNegation ? "should not" : "should";
}
