using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
        private const StringComparison DefaultIgnoreCaseComparison = StringComparison.OrdinalIgnoreCase;

        public static TOwner BeNullOrEmpty<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier) =>
            verifier.Satisfy(actual => string.IsNullOrEmpty(actual), "be null or empty");

        public static TOwner BeNullOrWhiteSpace<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier) =>
            verifier.Satisfy(actual => string.IsNullOrWhiteSpace(actual), "be null or white-space");

        public static TOwner EqualIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected) =>
            verifier.Satisfy(actual => string.Equals(expected, actual, StringComparison.CurrentCultureIgnoreCase), "equal {0} ignoring case", expected);

        public static TOwner Contain<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(actual => actual != null && actual.IndexOf(expected, verifier.ResolveStringComparison()) != -1, "contain {0}", expected);
        }

        public static TOwner ContainIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.IndexOf(expected, DefaultIgnoreCaseComparison) != -1,
                "contain {0} ignoring case",
                expected);
        }

        public static TOwner StartWith<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.StartsWith(expected, verifier.ResolveStringComparison()),
                "start with {0}",
                expected);
        }

        public static TOwner StartWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.StartsWith(expected, DefaultIgnoreCaseComparison),
                "start with {0} ignoring case",
                expected);
        }

        public static TOwner EndWith<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.EndsWith(expected, verifier.ResolveStringComparison()),
                "end with {0}",
                expected);
        }

        public static TOwner EndWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.EndsWith(expected, DefaultIgnoreCaseComparison),
                "end with {0} ignoring case",
                expected);
        }

        public static TOwner Match<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string pattern)
        {
            pattern.CheckNotNull(nameof(pattern));

            return verifier.Satisfy(actual => actual != null && Regex.IsMatch(actual, pattern), $"match pattern \"{pattern}\"");
        }

        public static TOwner MatchAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, TermMatch match, params string[] expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate(verifier.ResolveStringComparison());

            string message = new StringBuilder()
                .Append($"{match.GetShouldText()} ")
                .AppendIf(expected.Length > 1, "any of: ")
                .AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}"))
                .ToString();

            return verifier.Satisfy(actual => actual != null && expected.Any(x => predicate(actual, x)), message, expected);
        }

        public static TOwner ContainAll<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, params string[] expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            string message = new StringBuilder()
                .Append($"contain ")
                .AppendIf(expected.Length > 1, "all of: ")
                .AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}"))
                .ToString();

            StringComparison stringComparison = verifier.ResolveStringComparison();

            return verifier.Satisfy(actual => actual != null && expected.All(x => actual.IndexOf(x, stringComparison) != -1), message, expected);
        }

        public static TOwner HaveLength<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, int expected) =>
            verifier.Satisfy(actual => actual != null && actual.Length == expected, $"have length of {expected}");

        /// <inheritdoc cref="StartWithAny{TOwner}(IObjectVerificationProvider{string, TOwner}, IEnumerable{string})"/>
        public static TOwner StartWithAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, params string[] expected) =>
            verifier.StartWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that a string starts with any of the expected strings.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">The expected values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner StartWithAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            StringComparison stringComparison = verifier.ResolveStringComparison();

            return verifier.Satisfy(
                actual => actual != null && expected.Any(x => actual.StartsWith(x, stringComparison)),
                $"start with any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="EndWithAny{TOwner}(IObjectVerificationProvider{string, TOwner}, IEnumerable{string})"/>
        public static TOwner EndWithAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, params string[] expected) =>
            verifier.EndWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that a string ends with any of the expected strings.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">The expected values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner EndWithAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            StringComparison stringComparison = verifier.ResolveStringComparison();

            return verifier.Satisfy(
                actual => actual != null && expected.Any(x => actual.EndsWith(x, stringComparison)),
                $"end with any of {Stringifier.ToString(expected)}");
        }
    }
}
