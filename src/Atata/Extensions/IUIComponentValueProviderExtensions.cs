using System;
using System.Linq;
using System.Text;

namespace Atata
{
    public static class IUIComponentValueProviderExtensions
    {
        public static TOwner Get<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, out TValue value)
            where TOwner : PageObject<TOwner>
        {
            value = provider.Get();
            return provider.Owner;
        }

        public static TOwner Verify<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, Action assertAction, string verificationMessage, params object[] verificationMessageArgs)
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

        public static TOwner Verify<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, Action<TValue, string> assertAction, string verificationMessage, params object[] verificationMessageArgs)
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

        public static TOwner VerifyEquals<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, TValue expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.AreEqual(expected, actual, message),
                "equals \"{0}\"",
                provider.ConvertValueToString(expected));
        }

        public static TOwner VerifyDoesNotEqual<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, TValue unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.AreNotEqual(unexpected, actual, message),
                "does not equal \"{0}\"",
                provider.ConvertValueToString(unexpected));
        }

        public static TOwner VerifyContains<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.Contains(expected, actual, message),
                "contains \"{0}\"",
                expected);
        }

        public static TOwner VerifyStartsWith<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.StartsWith(expected, actual, message),
                "starts with \"{0}\"",
                expected);
        }

        public static TOwner VerifyEndsWith<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string expected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.EndsWith(expected, actual, message),
                "ends with \"{0}\"",
                expected);
        }

        public static TOwner VerifyMatches<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.IsMatch(pattern, actual, message),
                "matches pattern \"{0}\"",
                pattern);
        }

        public static TOwner VerifyDoesNotContain<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.DoesNotContain(unexpected, actual, message),
                "does not contain \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotStartWith<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.DoesNotStartWith(unexpected, actual, message),
                "does not start with \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotEndWith<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string unexpected)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.DoesNotEndWith(unexpected, actual, message),
                "does not end with \"{0}\"",
                unexpected);
        }

        public static TOwner VerifyDoesNotMatch<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return provider.Verify(
                (actual, message) => Assert.DoesNotMatch(pattern, actual, message),
                "does not match pattern \"{0}\"",
                pattern);
        }

        public static TOwner VerifyUntilMatchesAny<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, TermMatch match, params string[] expectedValues)
            where TOwner : PageObject<TOwner>
        {
            if (expectedValues == null)
                throw new ArgumentNullException("expectedValues");
            if (expectedValues.Length == 0)
                throw ExceptionFactory.CreateForArgumentEmptyCollection("expectedValues");

            string matchAsString = match.ToString(TermFormat.LowerCase);
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
                        string errorMessage = DefaultAsserter.BuildAssertionErrorMessage(
                            "String that {0} {1}\"{2}\"".FormatWith(matchAsString, expectedValues.Length > 1 ? "any of " : null, expectedValuesAsString),
                            string.Format("\"{0}\"", actualText),
                            "{0} {1} doesn't match criteria", provider.ComponentFullName, provider.ProviderName);

                        Assert.IsTrue(containsText, errorMessage);
                    }
                },
                "{0} {1}{2}",
                matchAsString,
                expectedValues.Length > 1 ? "any of " : null,
                expectedValuesAsString);
        }

        public static TOwner VerifyUntilContains<TOwner>(this IUIComponentValueProvider<string, TOwner> provider, params string[] expectedValues)
            where TOwner : PageObject<TOwner>
        {
            if (expectedValues == null)
                throw new ArgumentNullException("expectedValues");

            expectedValues = expectedValues.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (expectedValues.Length == 0)
                throw ExceptionFactory.CreateForArgumentEmptyCollection("expectedValues");

            string matchAsString = TermMatch.Contains.ToString(TermFormat.LowerCase);
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
                        string errorMessage = DefaultAsserter.BuildAssertionErrorMessage(
                            "String that {0} \"{1}\"".FormatWith(matchAsString, notFoundValue),
                            string.Format("\"{0}\"", actualText),
                            "{0} {1} doesn't match criteria", provider.ComponentFullName, provider.ProviderName);

                        Assert.IsTrue(containsText, errorMessage);
                    }
                },
                "{0} {1}",
                matchAsString,
                expectedValuesAsString);
        }

        public static TOwner VerifyUntil<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, Action<TValue, string> assertAction, string verificationMessage, params object[] verificationMessageArgs)
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

        private static string BuildErrorMessage<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider)
            where TOwner : PageObject<TOwner>
        {
            return string.Format("Invalid {0} {1}", provider.ComponentFullName, provider.ProviderName);
        }
    }
}
