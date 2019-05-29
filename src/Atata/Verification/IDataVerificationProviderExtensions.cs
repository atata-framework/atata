using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    public static class IDataVerificationProviderExtensions
    {
        private const string NullString = "null";

        public static TOwner Satisfy<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, Predicate<TData> predicate, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            string verificationConstraintMessage = BuildVerificationConstraintMessage(should, message, args);

            AtataContext.Current.Log.Start(new VerificationLogSection(should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage));

            TData actual = default(TData);
            Exception exception = null;

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    try
                    {
                        actual = should.DataProvider.Value;
                        bool result = predicate(actual) != should.IsNegation;
                        exception = null;
                        return result;
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        return false;
                    }
                },
                should.GetRetryOptions());

            if (!doesSatisfy)
            {
                string expectedMessage = BuildExpectedMessage(message, args?.Cast<object>().ToArray());
                throw should.CreateAssertionException(expectedMessage, ObjectToString(actual), exception);
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static TOwner Satisfy<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, Predicate<IEnumerable<TData>> predicate, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            string expectedMessage = (args != null && args.Any()) ? message?.FormatWith(CollectionToString(args)) : message;
            string verificationConstraintMessage = $"{should.GetShouldText()} {expectedMessage}";

            AtataContext.Current.Log.Start(new VerificationLogSection(should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage));

            IEnumerable<TData> actual = null;
            Exception exception = null;

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    try
                    {
                        actual = should.DataProvider.Value?.Select(x => x.Value).ToArray();
                        bool result = predicate(actual) != should.IsNegation;
                        exception = null;
                        return result;
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        return false;
                    }
                },
                should.GetRetryOptions());

            if (!doesSatisfy)
                throw should.CreateAssertionException(expectedMessage, CollectionToString(actual), exception);

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        private static string BuildVerificationConstraintMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string message, params TData[] args)
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

        private static string BuildExpectedMessage(string message, object[] args)
        {
            return args != null && args.Any()
                ? message.FormatWith(args.Select(x => ObjectToString(x)).ToArray())
                : message;
        }

        internal static Exception CreateAssertionException<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, string expected, string actual, Exception exception)
            where TOwner : PageObject<TOwner>
        {
            StringBuilder builder = new StringBuilder().
                Append($"Invalid {should.DataProvider.Component.ComponentFullName} {should.DataProvider.ProviderName}.").
                AppendLine().
                Append($"Expected: {should.GetShouldText()} {expected}");

            if (exception == null)
                builder.AppendLine().Append($"Actual: {actual}");

            string errorMessage = builder.ToString();

            var exceptionType = AtataContext.Current.AssertionExceptionType;

            return exceptionType != null
                ? (Exception)Activator.CreateInstance(exceptionType, errorMessage, exception)
                : new AssertionException(errorMessage, exception);
        }

        private static string CollectionToString(IEnumerable collection)
        {
            return CollectionToString(collection?.Cast<object>());
        }

        private static string CollectionToString(IEnumerable<object> collection)
        {
            if (collection == null)
                return NullString;
            if (!collection.Any())
                return "<empty>";
            else if (collection.Count() == 1)
                return ObjectToString(collection.First());
            else
                return "<{0}>".FormatWith(string.Join(", ", collection.Select(ObjectToString).ToArray()));
        }

        private static string ObjectToString(object value)
        {
            if (Equals(value, null))
                return NullString;
            else if (value is string)
                return "\"{0}\"".FormatWith(value);
            else if (value is ValueType)
                return value.ToString();
            else if (value is IEnumerable enumerableValue)
                return CollectionToString(enumerableValue);
            else
                return "{{{0}}}".FormatWith(value.ToString());
        }

        public static IDataVerificationProvider<TData, TOwner> WithSettings<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, IVerificationProvider<TOwner> sourceVerificationProvider)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            sourceVerificationProvider.CheckNotNull(nameof(sourceVerificationProvider));

            IDataVerificationProvider<TData, TOwner> resultVerificationProvider =
                sourceVerificationProvider.IsNegation && !should.IsNegation
                    ? GetNegationVerificationProvider(should)
                    : should;

            resultVerificationProvider.Timeout = sourceVerificationProvider.Timeout;
            resultVerificationProvider.RetryInterval = sourceVerificationProvider.RetryInterval;

            return resultVerificationProvider;
        }

        private static IDataVerificationProvider<TData, TOwner> GetNegationVerificationProvider<TData, TOwner>(IDataVerificationProvider<TData, TOwner> verificationProvider)
            where TOwner : PageObject<TOwner>
        {
            if (verificationProvider is DataVerificationProvider<TData, TOwner> dataVerificationProvider)
                return dataVerificationProvider.Not;
            else
                return (IDataVerificationProvider<TData, TOwner>)verificationProvider.GetType().GetPropertyWithThrowOnError("Not").GetValue(verificationProvider);
        }

        public static TOwner Equal<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => Equals(actual, expected), "equal {0}", expected);
        }

        public static TOwner BeTrue<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual, "be true");
        }

        public static TOwner BeTrue<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual == true, "be true");
        }

        public static TOwner BeFalse<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => !actual, "be false");
        }

        public static TOwner BeFalse<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual == false, "be false");
        }

        public static TOwner BeNull<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => Equals(actual, null), "be null");
        }

        public static TOwner BeNullOrEmpty<TOwner>(this IDataVerificationProvider<string, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => string.IsNullOrEmpty(actual), "be null or empty");
        }

        public static TOwner BeNullOrWhiteSpace<TOwner>(this IDataVerificationProvider<string, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => string.IsNullOrWhiteSpace(actual), "be null or white-space");
        }

        public static TOwner EqualIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => string.Equals(expected, actual, StringComparison.CurrentCultureIgnoreCase), "equal {0} ignoring case", expected);
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.Contains(expected), "contain {0}", expected);
        }

        public static TOwner ContainIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.ToUpper().Contains(expected.ToUpper()), "contain {0} ignoring case", expected);
        }

        public static TOwner StartWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.StartsWith(expected), "start with {0}", expected);
        }

        public static TOwner StartWithIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase), "start with {0} ignoring case", expected);
        }

        public static TOwner EndWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.EndsWith(expected), "end with {0}", expected);
        }

        public static TOwner EndWithIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase), "end with {0} ignoring case", expected);
        }

        public static TOwner Match<TOwner>(this IDataVerificationProvider<string, TOwner> should, string pattern)
            where TOwner : PageObject<TOwner>
        {
            pattern.CheckNotNull(nameof(pattern));

            return should.Satisfy(actual => actual != null && Regex.IsMatch(actual, pattern), $"match pattern \"{pattern}\"");
        }

        public static TOwner BeGreater<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreater<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreaterOrEqual<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeGreaterOrEqual<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeLess<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLess<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLessOrEqual<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeLessOrEqual<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData from, TData to)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(from) >= 0 && actual.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData from, TData to)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(from) >= 0 && actual.Value.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner EqualDate<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => Equals(actual.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner EqualDate<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && Equals(actual.Value.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner MatchAny<TOwner>(this IDataVerificationProvider<string, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate();

            string message = new StringBuilder().
                Append($"{match.GetShouldText()} ").
                AppendIf(expected.Length > 1, "any of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.Satisfy(actual => actual != null && expected.Any(x => predicate(actual, x)), message, expected);
        }

        public static TOwner ContainAll<TOwner>(this IDataVerificationProvider<string, TOwner> should, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            string message = new StringBuilder().
                Append($"contain ").
                AppendIf(expected.Length > 1, "all of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.Satisfy(actual => actual != null && expected.All(x => actual.Contains(x)), message, expected);
        }

        public static TOwner BeEmpty<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && !actual.Any(), "be empty");
        }

        public static TOwner HaveCount<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, int expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {CollectionToString(expected)}");
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {CollectionToString(expected)}");
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {CollectionToString(expected)}");
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {CollectionToString(expected)}");
        }

        public static TOwner ContainSingle<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Count() == 1, $"contain single item");
        }

        public static TOwner ContainSingle<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, TData expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(
                actual => actual != null && actual.Count(x => Equals(x, expected)) == 1,
                $"contain single {ObjectToString(expected)}");
        }

        public static TOwner ContainSingle<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, TData expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(
                actual => actual != null && actual.Count((TData x) => Equals(x, expected)) == 1,
                $"contain single {ObjectToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains a single item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, Expression<Func<TItem, bool>> predicateExpression)
            where TOwner : PageObject<TOwner>
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();
            string expressionAsText = ObjectExpressionStringBuilder.ExpressionToString(predicateExpression);

            return should.Satisfy(
                actual => actual != null && actual.Count(predicate) == 1,
                $"contain single \"{expressionAsText}\" {GetObjectTypeName(typeof(TItem))}");
        }

        public static TOwner Contain<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {CollectionToString(expected)}");
        }

        public static TOwner Contain<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {CollectionToString(expected)}");
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<string>, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<string, TOwner>>, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        public static TOwner Contain<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, Expression<Func<TControl, bool>> predicateExpression)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));
            var predicate = predicateExpression.Compile();

            // TODO: Change code: UIComponentResolver.ResolveControlTypeName<TControl>().
            return should.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain \"{UIComponentResolver.ResolveControlName<TControl, TOwner>(predicateExpression)}\" {UIComponentResolver.ResolveControlTypeName<TControl>()}");
        }

        public static TOwner ContainHavingContent<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, TermMatch match, params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return should.IsNegation
                        ? expected.Any(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                        : expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain having content that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        private static string GetObjectTypeName(Type type)
        {
            return type.IsInheritedFromOrIs(typeof(Control<>))
                ? UIComponentResolver.ResolveControlTypeName(type)
                : "item";
        }
    }
}
