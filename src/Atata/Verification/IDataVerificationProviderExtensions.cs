using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            TData actual = should.DataProvider.Get();
            bool doesSatisfy = predicate(actual);

            if (doesSatisfy == should.IsNegation)
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
                    Append($" {ResolveShouldText(should.IsNegation)} ").
                    Append(message.FormatWith(convertedArgs));
            }
            return logMessageBuilder.ToString();
        }

        private static AssertionException CreateAssertionException<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, TData actual, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            string errorMesage = BuildAssertionErrorMessage(
                    $"Invalid {should.DataProvider.ComponentFullName} {should.DataProvider.ProviderName}",
                    actual,
                    $"{ResolveShouldText(should.IsNegation)} {message}",
                    args.Cast<object>().ToArray());
            return new AssertionException(errorMesage);
        }

        private static string BuildAssertionErrorMessage(string primaryMessage, object actual, string expectedMessage, object[] expectedArgs)
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(primaryMessage))
                builder.Append(primaryMessage).AppendLine();

            return builder.
                AppendFormat("Expected: {0}", expectedMessage.FormatWith(expectedArgs?.Select(x => ObjectToString(x)).ToArray())).
                AppendLine().
                AppendFormat("But was: {0}", ObjectToString(actual)).
                ToString();
        }

        private static string ResolveShouldText(bool isNegation)
        {
            return isNegation ? "should not" : "should";
        }

        private static string CollectionToString(IEnumerable<object> collection)
        {
            if (!collection.Any())
                return "<empty>";

            return "< {0} >".FormatWith(string.Join(", ", collection.Select(ObjectToString).ToArray()));
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
    }
}
