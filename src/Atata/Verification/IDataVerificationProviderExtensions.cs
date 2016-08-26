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

            string logMessage = BuildLogMessage(should, message, args);

            ATContext.Current.Log.StartVerificationSection(logMessage);

            TData actual = default(TData);

            bool doesSatisfy = ATContext.Current.Driver.Try().Until(_ =>
            {
                actual = should.DataProvider.Get();
                return predicate(actual) != should.IsNegation;
            }, should.Timeout, should.RetryInterval);

            if (!doesSatisfy)
                throw CreateAssertionException(should, actual, message, args);

            ATContext.Current.Log.EndSection();

            return should.Owner;
        }

        private static string BuildLogMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            StringBuilder logMessageBuilder = new StringBuilder().
                Append($"{should.DataProvider.ComponentFullName} {should.DataProvider.ProviderName}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                string[] convertedArgs = args?.
                    Select(x => "\"{0}\"".FormatWith(should.DataProvider.ConvertValueToString(x) ?? NullString)).
                    ToArray();

                logMessageBuilder.
                    Append($" {should.GetShouldText()} ").
                    Append(message.FormatWith(convertedArgs));
            }
            return logMessageBuilder.ToString();
        }

        private static AssertionException CreateAssertionException<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, TData actual, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            return should.CreateAssertionException(
                message.FormatWith(args?.Select(x => ObjectToString(x)).ToArray()),
                ObjectToString(actual));
        }

        internal static AssertionException CreateAssertionException<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, string expected, string actual)
            where TOwner : PageObject<TOwner>
        {
            string errorMessage = new StringBuilder().
                AppendLine($"Invalid {should.DataProvider.ComponentFullName} {should.DataProvider.ProviderName}.").
                AppendLine($"Expected: {should.GetShouldText()} {expected}").
                AppendLine($"But was: {actual}").
                ToString();

            return new AssertionException(errorMessage);
        }

        private static string CollectionToString(IEnumerable<object> collection)
        {
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
            else if (value is IEnumerable)
                return CollectionToString(((IEnumerable)value).Cast<object>());
            else
                return "{{{0}}}".FormatWith(value.ToString());
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

            return should.Satisfy(actual => actual != null && Regex.IsMatch(actual, pattern), "match pattern {0}".FormatWith(ObjectToString(pattern)));
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
            return should.Satisfy(actual => actual != null && (actual.CompareTo(from) >= 0 || actual.CompareTo(to) <= 0), "be in range {0} - {1}", from, to);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData from, TData to)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && (actual.Value.CompareTo(from) >= 0 || actual.Value.CompareTo(to) <= 0), "be in range {0} - {1}", from, to);
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
            match.CheckNotEquals(nameof(match), TermMatch.Inherit);
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
            return should.Satisfy(actual => actual != null && actual.Any(), "be empty");
        }

        public static TOwner HaveCount<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, int expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");
        }

        public static TOwner ContainHavingContent<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, TermMatch match, params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            match.CheckNotEquals(nameof(match), TermMatch.Inherit);
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain {UIComponentResolver.ResolveControlTypeName<TControl>()} having content that {match.ToString(TermCase.Lower)} {CollectionToString(expected)}");
        }

        public static TOwner Contain<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.Select(x => x.Get()).Intersect(expected).Count() == expected.Count(),
                $"contain {CollectionToString(expected.Cast<object>())}");
        }

        public static TOwner Contain<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, Expression<Func<TControl, bool>> predicateExpression)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));
            var predicate = predicateExpression.Compile();

            return should.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain \"{UIComponentResolver.ResolveControlName<TControl, TOwner>(predicateExpression)}\" {UIComponentResolver.ResolveControlTypeName<TControl>()}");
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<string, TOwner>>, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            match.CheckNotEquals(nameof(match), TermMatch.Inherit);
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Value).ToArray();
                    return expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain having value that {match.ToString(TermCase.Lower)} {CollectionToString(expected)}");
        }
    }
}
