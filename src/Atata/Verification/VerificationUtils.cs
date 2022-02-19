using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Atata
{
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

        public static string BuildExpectedMessage(string message, object[] args)
        {
            return args != null && args.Any()
                ? message.FormatWith(args.Select(x => Stringifier.ToString(x)).ToArray())
                : message;
        }

        public static string BuildConstraintMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string message, params TData[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                string formattedMessage;

                if (args != null && args.Any())
                {
                    string[] convertedArgs = args
                        .Select(x => x is bool
                            ? x.ToString().ToLowerInvariant()
                            : $"\"{ConvertValueToString(should.DataProvider, x) ?? Stringifier.NullString}\"")
                        .ToArray();

                    formattedMessage = message.FormatWith(convertedArgs);
                }
                else
                {
                    formattedMessage = message;
                }

                return $"{should.GetShouldText()} {formattedMessage}";
            }
            else
            {
                return null;
            }
        }

        private static string ConvertValueToString<TData, TOwner>(IObjectProvider<TData, TOwner> provider, TData value) =>
            provider is IConvertsValueToString<TData> providerAsConverter
                ? providerAsConverter.ConvertValueToString(value)
                : TermResolver.ToString(value);

        public static string BuildFailureMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string expected, string actual) =>
            BuildFailureMessage(should, expected, actual, true);

        public static string BuildFailureMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string expected, string actual, bool prependShouldTextToExpected)
        {
            StringBuilder builder = new StringBuilder()
                .Append($"{should.DataProvider.ProviderName}")
                .AppendLine()
                .Append("Expected: ");

            if (prependShouldTextToExpected)
                builder.Append(should.GetShouldText()).Append(' ');

            builder.Append(expected);

            if (actual != null)
                builder.AppendLine().Append($"  Actual: {actual}");

            return builder.ToString();
        }

        internal static string AppendExceptionToFailureMessage(string message, Exception exception)
        {
            if (exception != null)
            {
                StringBuilder builder = new StringBuilder(message).
                    AppendLine().
                    Append("  ----> ").
                    Append(exception.ToString());

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

        internal static bool ExecuteUntil(Func<bool> condition, RetryOptions retryOptions)
        {
            var wait = CreateSafeWait(retryOptions);
            return wait.Until(_ => condition());
        }

        private static SafeWait<object> CreateSafeWait(RetryOptions options)
        {
            var wait = new SafeWait<object>(string.Empty)
            {
                Timeout = options.Timeout,
                PollingInterval = options.Interval
            };

            foreach (Type exceptionType in options.IgnoredExceptionTypes)
                wait.IgnoreExceptionTypes(exceptionType);

            return wait;
        }

        internal static TOwner Verify<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, Action verificationAction, string expectedMessage, params TData[] arguments)
        {
            if (AtataContext.Current is null)
            {
                verificationAction.Invoke();
            }
            else
            {
                string verificationConstraintMessage = BuildConstraintMessage(should, expectedMessage, arguments);

                LogSection logSection = new VerificationLogSection(
                    should.Strategy.VerificationKind,
                    should.DataProvider.ProviderName,
                    verificationConstraintMessage);

                AtataContext.Current.Log.ExecuteSection(logSection, verificationAction);
            }

            return should.Owner;
        }
    }
}
