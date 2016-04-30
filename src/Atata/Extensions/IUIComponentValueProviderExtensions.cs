using System;
using System.Linq;
using System.Text;

namespace Atata
{
    public static class IUIComponentValueProviderExtensions
    {
        public static TOwner Verify<TValue, TOwner>(this IUIComponentValueProvider<TValue, TOwner> provider, Action assertAction, string verificationMessage, params object[] verificationMessageArgs)
            where TOwner : PageObject<TOwner>
        {
            StringBuilder logMessageBuilder = new StringBuilder(provider.ComponentFullName);
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

                    string errorMessage = string.Format("Invalid {0} {1}", provider.ComponentFullName, provider.ProviderName);
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
                "is equal to '{0}'",
                provider.ConvertValueToString(expected));
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

                        Assert.That(containsText, errorMessage);
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

                        Assert.That(containsText, errorMessage);
                    }
                },
                "{0} {1}",
                matchAsString,
                expectedValuesAsString);
        }
    }
}
