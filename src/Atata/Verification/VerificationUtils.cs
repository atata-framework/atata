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
        public const string NullString = "null";

        public static string ToString(IEnumerable collection)
        {
            return ToString(collection?.Cast<object>());
        }

        public static string ToString<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                return NullString;
            if (!collection.Any())
                return "<empty>";
            else if (collection.Count() == 1)
                return ToString(collection.First());
            else
                return "<{0}>".FormatWith(string.Join(", ", collection.Select(x => x.ToString()).ToArray()));
        }

        public static string ToString<T>(Expression<Func<T, bool>> predicateExpression)
        {
            return $"\"{ObjectExpressionStringBuilder.ExpressionToString(predicateExpression)}\" {GetItemTypeName(typeof(T))}";
        }

        public static string ToString(Expression expression)
        {
            return $"\"{ObjectExpressionStringBuilder.ExpressionToString(expression)}\"";
        }

        public static string ToString(object value)
        {
            if (Equals(value, null))
                return NullString;
            else if (value is string)
                return $"\"{value}\"";
            else if (value is ValueType)
                return value.ToString();
            else if (value is IEnumerable enumerableValue)
                return ToString(enumerableValue);
            else if (value is Expression expressionValue)
                return ToString(expressionValue);
            else
                return $"{{{value}}}";
        }

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

            return exceptionType != null
                ? (Exception)Activator.CreateInstance(exceptionType, message, innerException)
                : new AssertionException(message, innerException);
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
                ? message.FormatWith(args.Select(x => ToString(x)).ToArray())
                : message;
        }

        public static string BuildConstraintMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                string formattedMessage;

                if (args != null && args.Any())
                {
                    string[] convertedArgs = args.
                        Select(x => $"\"{should.DataProvider.ConvertValueToString(x) ?? NullString}\"").
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
            where TOwner : PageObject<TOwner>
        {
            StringBuilder builder = new StringBuilder().
                Append($"{should.DataProvider.Component.ComponentFullName} {should.DataProvider.ProviderName}.").
                AppendLine().
                Append($"Expected: {should.GetShouldText()} {expected}");

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
