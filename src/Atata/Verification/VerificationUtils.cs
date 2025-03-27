#nullable enable

namespace Atata;

public static class VerificationUtils
{
    public static Exception CreateAssertionException(AtataContext? context, string message, Exception? innerException = null)
    {
        IAssertionExceptionFactory exceptionFactory = context?.AssertionExceptionFactory
            ?? AtataAssertionExceptionFactory.Instance;

        return exceptionFactory.Create(message, innerException);
    }

    public static Exception CreateAggregateAssertionException(AtataContext? context, IEnumerable<AssertionResult> assertionResults)
    {
        IAggregateAssertionExceptionFactory exceptionFactory = context?.AggregateAssertionExceptionFactory
            ?? AtataAggregateAssertionExceptionFactory.Instance;

        return exceptionFactory.Create(assertionResults);
    }

    public static string BuildExpectedMessage(string message, object[] args) =>
        args?.Length > 0
            ? message.FormatWith([.. args.Select(Stringifier.ToString)])
            : message;

    public static string? BuildConstraintMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string message, params TData[] args)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            string formattedMessage;

            if (args?.Length > 0)
            {
                string[] convertedArgs = [.. args.Select(x => ConvertValueToString(verifier.ObjectProvider, x))];

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

    public static string BuildFailureMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string expected, string? actual) =>
        BuildFailureMessage(verifier, expected, actual, true);

    public static string BuildFailureMessage<TData, TOwner>(IObjectVerificationProvider<TData, TOwner> verifier, string expected, string? actual, bool prependShouldTextToExpected)
    {
        var builder = new StringBuilder()
            .Append($"{verifier.ObjectProvider.ProviderName}")
            .AppendLine()
            .Append("Expected: ");

        if (prependShouldTextToExpected)
            builder.Append(ResolveShouldText(verifier)).Append(' ');

        builder.Append(expected);

        if (actual is not null)
            builder.AppendLine().Append($"  Actual: {actual}");

        return builder.ToString();
    }

    public static string AppendExceptionToFailureMessage(string message, Exception? exception) =>
        exception is not null
            ? new StringBuilder(message)
                .AppendLine()
                .Append(" ---> ")
                .Append(exception.ToString())
                .ToString()
            : message;

    internal static string AppendStackTraceToFailureMessage(string message, string? stackTrace) =>
        stackTrace?.Length > 0
            ? new StringBuilder(message)
                .AppendLine()
                .Append(stackTrace)
                .ToString()
            : message;

    internal static string CreateStackTraceForAssertionFailiure()
    {
        string stackTrace = new StackTrace(1, true).ToString();

        return StackTraceFilter.TakeBeforeInvokeMethodOfRuntimeMethodHandle(stackTrace);
    }

    internal static bool ExecuteUntil(Func<bool> condition, (TimeSpan Timeout, TimeSpan RetryInterval) retryOptions)
    {
        RetryWait retryWait = new(retryOptions.Timeout, retryOptions.RetryInterval);

        return retryWait.Until(condition);
    }

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
            string? verificationConstraintMessage = BuildConstraintMessage(verifier, expectedMessage, arguments);

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
