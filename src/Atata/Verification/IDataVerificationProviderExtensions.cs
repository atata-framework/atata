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

            IUIComponentDataProvider<TData, TOwner> provider = should.DataProvider;

            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("{0} {1}", provider.ComponentFullName, provider.ProviderName);

            if (!string.IsNullOrWhiteSpace(message))
            {
                string[] convertedArgs = args?.Select(x => "\"{0}\"".FormatWith(provider.ConvertValueToString(x) ?? NullString)).ToArray();

                logMessageBuilder.
                    Append($" {ResolveShouldText(should.IsNegation)} ").
                    Append(message.FormatWith(convertedArgs));
            }

            ATContext.Current.Log.StartVerificationSection(logMessageBuilder.ToString());

            TData actual = provider.Get();
            bool doesSatisfy = predicate(actual);

            if (doesSatisfy == should.IsNegation)
            {
                string errorMesage = BuildAssertionErrorMessage(
                    $"Invalid {provider.ComponentFullName} {provider.ProviderName}",
                    actual,
                    $"{ResolveShouldText(should.IsNegation)} {message}",
                    args.Cast<object>().ToArray());
                throw new AssertionException(errorMesage);
            }

            ATContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static string BuildAssertionErrorMessage(string primaryMessage, object actual, string expectedMessage, object[] expectedArgs)
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
            return should.Satisfy(x => Equals(x, expected), "equal {0}", expected);
        }

        public static TOwner StartWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(x => x.StartsWith(expected), "start with {0}", expected);
        }

        public static TOwner EndWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(x => x.EndsWith(expected), "end with {0}", expected);
        }

        public static TOwner Match<TOwner>(this IDataVerificationProvider<string, TOwner> should, string pattern)
            where TOwner : PageObject<TOwner>
        {
            pattern.CheckNotNull(nameof(pattern));

            return should.Satisfy(x => Regex.IsMatch(x, pattern), "match pattern {0}".FormatWith(ObjectToString(pattern)));
        }

        public static TOwner BeNullOrEmpty<TOwner>(this IDataVerificationProvider<string, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(x => string.IsNullOrEmpty(x), "be null or empty");
        }

        public static TOwner BeGreater<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(x => x.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData from, TData to)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(x => x.CompareTo(from) >= 0 || x.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }
    }
}
