using System;
using System.Linq;
using System.Text;

namespace Atata
{
    public static class IDataProviderExtensions
    {
        public static TOwner Get<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, out TValue value)
            where TOwner : PageObject<TOwner>
        {
            value = provider.Get();
            return provider.Owner;
        }

        internal static string ConvertValueToString<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue value)
            where TOwner : PageObject<TOwner>
        {
            return TermResolver.ToString(value, provider.ValueTermOptions);
        }

        public static TOwner Verify<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, Action assertAction, string verificationMessage, params object[] verificationMessageArgs)
            where TOwner : PageObject<TOwner>
        {
            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("{0} {1}", provider.ComponentFullName, provider.ProviderName);

            if (!string.IsNullOrWhiteSpace(verificationMessage))
                logMessageBuilder.Append(" ").AppendFormat(verificationMessage, verificationMessageArgs);

            ATContext.Current.Log.StartVerificationSection(logMessageBuilder.ToString());

            assertAction();

            ATContext.Current.Log.EndSection();

            return provider.Owner;
        }

        public static TOwner Verify<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, Action<TValue, string> assertAction, string verificationMessage, params object[] verificationMessageArgs)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                () =>
                {
                    TValue actual = provider.Get();

                    string errorMessage = BuildErrorMessage(provider);
                    assertAction(actual, errorMessage);
                },
                verificationMessage,
                verificationMessageArgs);
        }

        public static TOwner VerifyIsTrue<TOwner>(this IDataProvider<bool, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsTrue(actual, message),
                "is {0}",
                bool.TrueString);
        }

        public static TOwner VerifyIsTrue<TOwner>(this IDataProvider<bool?, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsTrue(actual, message),
                "is {0}",
                bool.TrueString);
        }

        public static TOwner VerifyIsFalse<TOwner>(this IDataProvider<bool, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsFalse(actual, message),
                "is {0}",
                bool.FalseString);
        }

        public static TOwner VerifyIsFalse<TOwner>(this IDataProvider<bool?, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsFalse(actual, message),
                "is {0}",
                bool.FalseString);
        }

        public static TOwner VerifyIsNull<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsNull(actual, message),
                "is null");
        }

        public static TOwner VerifyIsNotNull<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsNotNull(actual, message),
                "is not null");
        }

        public static TOwner VerifyIsNullOrEmpty<TOwner>(this IDataProvider<string, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsNullOrEmpty(actual, message),
                "is null or empty");
        }

        public static TOwner VerifyIsNotNullOrEmpty<TOwner>(this IDataProvider<string, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsNotNullOrEmpty(actual, message),
                "is not null or empty");
        }

        public static TOwner VerifyEquals<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.AreEqual(expected, actual, message),
                "equals \"{0}\"",
                provider.ConvertValueToString(expected));
        }

        public static TOwner VerifyDoesNotEqual<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.AreNotEqual(unexpected, actual, message),
                "does not equal \"{0}\"",
                provider.ConvertValueToString(unexpected));
        }

        public static TOwner VerifyIsGreaterThan<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue compareTo)
            where TValue : IComparable<TValue>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.Greater(actual, compareTo, message),
                "is greater than {0}",
                provider.ConvertValueToString(compareTo));
        }

        public static TOwner VerifyIsGreaterThanOrEqualTo<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue compareTo)
            where TValue : IComparable<TValue>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.GreaterOrEqual(actual, compareTo, message),
                "is greater than or equal to {0}",
                provider.ConvertValueToString(compareTo));
        }

        public static TOwner VerifyIsLessThan<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue compareTo)
            where TValue : IComparable<TValue>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.Less(actual, compareTo, message),
                "is less than {0}",
                provider.ConvertValueToString(compareTo));
        }

        public static TOwner VerifyIsLessThanOrEqualTo<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue compareTo)
            where TValue : IComparable<TValue>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.LessOrEqual(actual, compareTo, message),
                "is less than or equal to {0}",
                provider.ConvertValueToString(compareTo));
        }

        public static TOwner VerifyIsInRange<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue from, TValue to)
            where TValue : IComparable<TValue>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsInRange(from, to, actual, message),
                "is in range {0} - {1}",
                provider.ConvertValueToString(from),
                provider.ConvertValueToString(to));
        }

        public static TOwner VerifyEqualsIgnoringCase<TOwner>(this IDataProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.AreEqualIgnoringCase(expected, actual, message),
                "equals \"{0}\" ignoring case",
                expected);
        }

        public static TOwner VerifyDoesNotEqualIgnoringCase<TOwner>(this IDataProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.AreNotEqualIgnoringCase(unexpected, actual, message),
                "does not equal \"{0}\" ignoring case",
                unexpected);
        }

        public static TOwner VerifyContains<TOwner>(this IDataProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.Contains(expected, actual, message),
                "contains \"{0}\"",
                expected);
        }

        public static TOwner VerifyStartsWith<TOwner>(this IDataProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.StartsWith(expected, actual, message),
                "starts with \"{0}\"",
                expected);
        }

        public static TOwner VerifyEndsWith<TOwner>(this IDataProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.EndsWith(expected, actual, message),
                "ends with \"{0}\"",
                expected);
        }

        public static TOwner VerifyMatches<TOwner>(this IDataProvider<string, TOwner> provider, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.IsMatch(pattern, actual, message),
                "matches pattern \"{0}\"",
                pattern);
        }

        public static TOwner VerifyDoesNotContain<TOwner>(this IDataProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.DoesNotContain(unexpected, actual, message),
                "does not contain \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotStartWith<TOwner>(this IDataProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.DoesNotStartWith(unexpected, actual, message),
                "does not start with \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotEndWith<TOwner>(this IDataProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.DoesNotEndWith(unexpected, actual, message),
                "does not end with \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotMatch<TOwner>(this IDataProvider<string, TOwner> provider, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.DoesNotMatch(pattern, actual, message),
                "does not match pattern \"{0}\"",
                pattern);
        }

        public static TOwner VerifyDateEquals<TOwner>(this IDataProvider<DateTime, TOwner> provider, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => ATAssert.AreEqual(expected.Date, actual.Date, message),
                "date equals \"{0}\"",
                provider.ConvertValueToString(expected));
        }

        public static TOwner VerifyUntilMatchesAny<TOwner>(this IDataProvider<string, TOwner> provider, TermMatch match, params string[] expectedValues)
            where TOwner : PageObject<TOwner>
        {
            expectedValues.CheckNotNullOrEmpty("expectedValues");

            string matchAsString = match.ToString(TermCase.Lower);
            string expectedValuesAsString = expectedValues.ToQuotedValuesListOfString(true);

            return provider.Verify(
                () =>
                {
                    string actualText = null;

                    bool containsText = ATContext.Current.Driver.Try().Until(_ =>
                    {
                        actualText = provider.Get();
                        return match.IsMatch(actualText, expectedValues);
                    });

                    if (!containsText)
                    {
                        match.Assert(expectedValues, actualText, "{0} {1} doesn't match criteria", provider.ComponentFullName, provider.ProviderName);
                    }
                },
                "{0} {1}{2} (with retry)",
                matchAsString,
                expectedValues.Length > 1 ? "any of " : null,
                expectedValuesAsString);
        }

        public static TOwner VerifyUntilContains<TOwner>(this IDataProvider<string, TOwner> provider, params string[] expectedValues)
            where TOwner : PageObject<TOwner>
        {
            expectedValues.CheckNotNull("expectedValues");

            expectedValues = expectedValues.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            expectedValues.CheckNotNullOrEmpty("expectedValues");

            string expectedValuesAsString = expectedValues.ToQuotedValuesListOfString(true);

            return provider.Verify(
                () =>
                {
                    string actualText = null;
                    string notFoundValue = null;

                    bool containsText = ATContext.Current.Driver.Try().Until(_ =>
                    {
                        actualText = provider.Get();
                        notFoundValue = expectedValues.FirstOrDefault(value => !actualText.Contains(value));
                        return notFoundValue == null;
                    });

                    if (!containsText)
                    {
                        ATAssert.Contains(notFoundValue, actualText, "{0} {1} doesn't match criteria", provider.ComponentFullName, provider.ProviderName);
                    }
                },
                "contains {0} (with retry)",
                expectedValuesAsString);
        }

        public static TOwner VerifyUntil<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, Action<TValue, string> assertAction, string verificationMessage, params object[] verificationMessageArgs)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                () =>
                {
                    string errorMessage = BuildErrorMessage(provider);
                    TValue actual = default(TValue);

                    bool isSuccess = ATContext.Current.Driver.Try().Until(_ =>
                    {
                        actual = provider.Get();
                        try
                        {
                            assertAction(actual, errorMessage);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });

                    if (!isSuccess)
                        assertAction(actual, errorMessage);
                },
                verificationMessage,
                verificationMessageArgs);
        }

        internal static string BuildErrorMessage<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return string.Format("Invalid {0} {1}", provider.ComponentFullName, provider.ProviderName);
        }
    }
}
