using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Atata
{
    public static class VerificationUtils
    {
        [Obsolete("Use Stringifier.NullString instead.")] // Obsolete since v1.9.0.
        public const string NullString = Stringifier.NullString;

        [Obsolete("Use Stringifier.ToString(...) instead.")] // Obsolete since v1.9.0.
        public static string ToString(IEnumerable collection)
        {
            return Stringifier.ToString(collection);
        }

        [Obsolete("Use Stringifier.ToString(...) instead.")] // Obsolete since v1.9.0.
        public static string ToString<T>(IEnumerable<T> collection)
        {
            return Stringifier.ToString(collection);
        }

        [Obsolete("Use Stringifier.ToString(...) instead.")] // Obsolete since v1.9.0.
        public static string ToString<T>(Expression<Func<T, bool>> predicateExpression)
        {
            return Stringifier.ToString(predicateExpression);
        }

        [Obsolete("Use Stringifier.ToString(...) instead.")] // Obsolete since v1.9.0.
        public static string ToString(Expression expression)
        {
            return Stringifier.ToString(expression);
        }

        [Obsolete("Use Stringifier.ToString(...) instead.")] // Obsolete since v1.9.0.
        public static string ToString(object value)
        {
            return Stringifier.ToString(value);
        }

        [Obsolete("Don't use this method.")] // Obsolete since v1.9.0.
        public static string GetItemTypeName(Type type)
        {
            return type.IsInheritedFromOrIs(typeof(Control<>))
                ? UIComponentResolver.ResolveControlTypeName(type)
                : "item";
        }

        [Obsolete("Use CreateAssertionException(string, Exception) instead.")] // Obsolete since v1.3.0.
        public static Exception CreateAssertionException<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string expected, string actual, Exception exception)
            where TOwner : PageObject<TOwner>
        {
            string message = BuildFailureMessage(should, expected, actual);

            return CreateAssertionException(message, exception);
        }

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
                    string[] convertedArgs = args.
                        Select(x => $"\"{should.DataProvider.ConvertValueToString(x) ?? Stringifier.NullString}\"").
                        ToArray();

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

        public static string BuildFailureMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string expected, string actual)
        {
            StringBuilder builder = new StringBuilder();

            string componentName = should.DataProvider.Component?.ComponentFullName;

            if (componentName != null)
                builder.Append($"{componentName} ");

            builder
                .Append($"{should.DataProvider.ProviderName}.")
                .AppendLine()
                .Append($"Expected: {should.GetShouldText()} {expected}");

            if (actual != null)
                builder.AppendLine().Append($"Actual: {actual}");

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
    }
}
