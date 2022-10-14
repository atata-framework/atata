using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of verification extension methods.
    /// </summary>
    public static class IObjectVerificationProviderExtensions
    {
        /// <summary>
        /// Verifies that object satisfies the specified predicate expression.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Satisfy<TObject, TOwner>(
            this IObjectVerificationProvider<TObject, TOwner> verifier,
            Expression<Func<TObject, bool>> predicateExpression)
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));

            var predicate = predicateExpression.Compile();
            string expressionAsString = Stringifier.ToString(predicateExpression);

            return verifier.Satisfy(predicate, $"satisfy {expressionAsString}");
        }

        /// <summary>
        /// Verifies that object satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message that should sound in a way of "{Something} should {message}".</param>
        /// <param name="args">The message arguments.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Satisfy<TObject, TOwner>(
            this IObjectVerificationProvider<TObject, TOwner> verifier,
            Func<TObject, bool> predicate,
            string message,
            params TObject[] args)
        {
            verifier.CheckNotNull(nameof(verifier));
            predicate.CheckNotNull(nameof(predicate));

            void ExecuteVerification()
            {
                TObject actual = default;
                Exception exception = null;

                bool doesSatisfy = VerificationUtils.ExecuteUntil(
                    () =>
                    {
                        try
                        {
                            actual = verifier.ObjectProvider.Object;
                            bool result = predicate(actual) != verifier.IsNegation;
                            exception = null;
                            return result;
                        }
                        catch (Exception e)
                        {
                            exception = e;
                            return false;
                        }
                    },
                    verifier.GetRetryOptions());

                if (!doesSatisfy)
                {
                    string shortenExpectedMessage = null;
                    bool isExpectedMessageShorten = !verifier.IsNegation && TryShortenExpectedMessage(message, out shortenExpectedMessage);

                    string expectedMessage = VerificationUtils.BuildExpectedMessage(
                        isExpectedMessageShorten ? shortenExpectedMessage : message,
                        args?.Cast<object>().ToArray());

                    string actualMessage = exception == null ? Stringifier.ToString(actual) : null;

                    string failureMessage = VerificationUtils.BuildFailureMessage(verifier, expectedMessage, actualMessage, !isExpectedMessageShorten);

                    verifier.Strategy.ReportFailure(failureMessage, exception);
                }
            }

            return VerificationUtils.Verify(verifier, ExecuteVerification, message, args);
        }

        /// <summary>
        /// Verifies that collection satisfies the specified predicate expression.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Satisfy<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            Expression<Func<IEnumerable<TObject>, bool>> predicateExpression)
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));

            var predicate = predicateExpression.Compile();
            string expressionAsString = Stringifier.ToString(predicateExpression);

            return verifier.Satisfy(predicate, $"satisfy {expressionAsString}");
        }

        /// <summary>
        /// Verifies that collection satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message that should sound in a way of "{Something} should {message}".</param>
        /// <param name="args">The message arguments.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Satisfy<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            Func<IEnumerable<TObject>, bool> predicate,
            string message,
            params TObject[] args)
        {
            verifier.CheckNotNull(nameof(verifier));
            predicate.CheckNotNull(nameof(predicate));

            string expectedMessage = (args != null && args.Any())
                ? message?.FormatWith(Stringifier.ToString(args))
                : message;

            void ExecuteVerification()
            {
                IEnumerable<TObject> actual = null;
                Exception exception = null;

                bool doesSatisfy = VerificationUtils.ExecuteUntil(
                    () =>
                    {
                        try
                        {
                            actual = verifier.ObjectProvider.Object?.Select(x => x.Object).ToArray();
                            bool result = predicate(actual) != verifier.IsNegation;
                            exception = null;
                            return result;
                        }
                        catch (Exception e)
                        {
                            exception = e;
                            return false;
                        }
                    },
                    verifier.GetRetryOptions());

                if (!doesSatisfy)
                {
                    string actualMessage = exception == null ? Stringifier.ToString(actual) : null;

                    string failureMessage = VerificationUtils.BuildFailureMessage(verifier, expectedMessage, actualMessage);

                    verifier.Strategy.ReportFailure(failureMessage, exception);
                }
            }

            return VerificationUtils.Verify(verifier, ExecuteVerification, expectedMessage);
        }

        private static bool TryShortenExpectedMessage(string originalMessage, out string resultMessage)
        {
            if (originalMessage == "equal {0}" || originalMessage == "be {0}")
            {
                resultMessage = "{0}";
                return true;
            }
            else if (originalMessage == "be null")
            {
                resultMessage = "null";
                return true;
            }
            else
            {
                resultMessage = originalMessage;
                return false;
            }
        }

        public static IObjectVerificationProvider<TObject, TOwner> WithSettings<TObject, TOwner>(
            this IObjectVerificationProvider<TObject, TOwner> verifier,
            IVerificationProvider<TOwner> sourceVerificationProvider)
        {
            verifier.CheckNotNull(nameof(verifier));
            sourceVerificationProvider.CheckNotNull(nameof(sourceVerificationProvider));

            IObjectVerificationProvider<TObject, TOwner> resultVerificationProvider =
                sourceVerificationProvider.IsNegation && !verifier.IsNegation
                    ? GetNegationVerificationProvider(verifier)
                    : verifier;

            resultVerificationProvider.Strategy = sourceVerificationProvider.Strategy;
            resultVerificationProvider.Timeout = sourceVerificationProvider.Timeout;
            resultVerificationProvider.RetryInterval = sourceVerificationProvider.RetryInterval;

            return resultVerificationProvider;
        }

        private static IObjectVerificationProvider<TObject, TOwner> GetNegationVerificationProvider<TObject, TOwner>(IObjectVerificationProvider<TObject, TOwner> verificationProvider) =>
            verificationProvider is ObjectVerificationProvider<TObject, TOwner> objectVerificationProvider
                ? objectVerificationProvider.Not
                : (IObjectVerificationProvider<TObject, TOwner>)verificationProvider.GetType().GetPropertyWithThrowOnError("Not").GetValue(verificationProvider);

        /// <summary>
        /// Verifies that object is equal to <paramref name="expected"/> value.
        /// The method does the same as <see cref="Be{TObject, TOwner}(IObjectVerificationProvider{TObject, TOwner}, TObject)"/> method,
        /// and the second one is preferable to use.
        /// This method will be removed in future.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Equal<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected) =>
            verifier.Satisfy(actual => Equals(actual, expected), "equal {0}", expected);

        /// <summary>
        /// Verifies that object is equal to <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Be<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected) =>
            verifier.Satisfy(actual => Equals(actual, expected), "be {0}", expected);

        public static TOwner BeTrue<TOwner>(this IObjectVerificationProvider<bool, TOwner> verifier) =>
            verifier.Be(true);

        public static TOwner BeTrue<TOwner>(this IObjectVerificationProvider<bool?, TOwner> verifier) =>
            verifier.Be(true);

        public static TOwner BeFalse<TOwner>(this IObjectVerificationProvider<bool, TOwner> verifier) =>
            verifier.Be(false);

        public static TOwner BeFalse<TOwner>(this IObjectVerificationProvider<bool?, TOwner> verifier) =>
            verifier.Be(false);

        public static TOwner BeNull<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier) =>
            verifier.Satisfy(actual => Equals(actual, null), "be null");

        public static TOwner BeNullOrEmpty<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier) =>
            verifier.Satisfy(actual => string.IsNullOrEmpty(actual), "be null or empty");

        public static TOwner BeNullOrWhiteSpace<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier) =>
            verifier.Satisfy(actual => string.IsNullOrWhiteSpace(actual), "be null or white-space");

        public static TOwner EqualIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected) =>
            verifier.Satisfy(actual => string.Equals(expected, actual, StringComparison.CurrentCultureIgnoreCase), "equal {0} ignoring case", expected);

        public static TOwner Contain<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(actual => actual != null && actual.Contains(expected), "contain {0}", expected);
        }

        public static TOwner ContainIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.ToUpper(CultureInfo.CurrentCulture).Contains(expected.ToUpper(CultureInfo.CurrentCulture)),
                "contain {0} ignoring case",
                expected);
        }

        public static TOwner StartWith<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.StartsWith(expected, StringComparison.CurrentCulture),
                "start with {0}",
                expected);
        }

        public static TOwner StartWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "start with {0} ignoring case",
                expected);
        }

        public static TOwner EndWith<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.EndsWith(expected, StringComparison.CurrentCulture),
                "end with {0}",
                expected);
        }

        public static TOwner EndWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "end with {0} ignoring case",
                expected);
        }

        public static TOwner Match<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, string pattern)
        {
            pattern.CheckNotNull(nameof(pattern));

            return verifier.Satisfy(actual => actual != null && Regex.IsMatch(actual, pattern), $"match pattern \"{pattern}\"");
        }

        public static TOwner BeGreater<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
            where TObject : IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.CompareTo(expected) > 0, "be greater than {0}", expected);

        public static TOwner BeGreater<TObject, TOwner>(this IObjectVerificationProvider<TObject?, TOwner> verifier, TObject expected)
            where TObject : struct, IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) > 0, "be greater than {0}", expected);

        public static TOwner BeGreaterOrEqual<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
            where TObject : IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);

        public static TOwner BeGreaterOrEqual<TObject, TOwner>(this IObjectVerificationProvider<TObject?, TOwner> verifier, TObject expected)
            where TObject : struct, IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);

        public static TOwner BeLess<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
            where TObject : IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.CompareTo(expected) < 0, "be less than {0}", expected);

        public static TOwner BeLess<TObject, TOwner>(this IObjectVerificationProvider<TObject?, TOwner> verifier, TObject expected)
            where TObject : struct, IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) < 0, "be less than {0}", expected);

        public static TOwner BeLessOrEqual<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
            where TObject : IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);

        public static TOwner BeLessOrEqual<TObject, TOwner>(this IObjectVerificationProvider<TObject?, TOwner> verifier, TObject expected)
            where TObject : struct, IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);

        public static TOwner BeInRange<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject from, TObject to)
            where TObject : IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.CompareTo(from) >= 0 && actual.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);

        public static TOwner BeInRange<TObject, TOwner>(this IObjectVerificationProvider<TObject?, TOwner> verifier, TObject from, TObject to)
            where TObject : struct, IComparable<TObject>, IComparable =>
            verifier.Satisfy(actual => actual != null && actual.Value.CompareTo(from) >= 0 && actual.Value.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);

        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime, TOwner> verifier, DateTime expected) =>
            verifier.Satisfy(actual => Equals(actual.Date, expected.Date), "equal date {0}", expected);

        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime?, TOwner> verifier, DateTime expected) =>
            verifier.Satisfy(actual => actual != null && Equals(actual.Value.Date, expected.Date), "equal date {0}", expected);

        public static TOwner MatchAny<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, TermMatch match, params string[] expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate();

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

            return verifier.Satisfy(actual => actual != null && expected.All(x => actual.Contains(x)), message, expected);
        }

        public static TOwner BeEmpty<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier) =>
            verifier.Satisfy(actual => actual != null && !actual.Any(), "be empty");

        public static TOwner HaveCount<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, int expected) =>
            verifier.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");

        public static TOwner HaveLength<TOwner>(this IObjectVerificationProvider<string, TOwner> verifier, int expected) =>
            verifier.Satisfy(actual => actual != null && actual.Length == expected, $"have length of {expected}");

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, params TObject[] expected) =>
            verifier.BeEquivalent(expected.AsEnumerable());

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
            verifier.BeEquivalent(expected.AsEnumerable());

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, params TObject[] expected) =>
            verifier.EqualSequence(expected.AsEnumerable());

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
            verifier.EqualSequence(expected.AsEnumerable());

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains only a single item.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier) =>
            verifier.Satisfy(actual => actual != null && actual.Count() == 1, $"contain single item");

        /// <summary>
        /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier, TItem expected) =>
            verifier.Satisfy(
                actual => actual != null && actual.Count(x => Equals(x, expected)) == 1,
                $"contain single {Stringifier.ToString(expected)}");

        /// <summary>
        /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, TObject expected) =>
            verifier.Satisfy(
                actual => actual != null && actual.Count((TObject x) => Equals(x, expected)) == 1,
                $"contain single {Stringifier.ToString(expected)}");

        /// <summary>
        /// Verifies that collection contains a single item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier, Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Count(predicate) == 1,
                $"contain single {Stringifier.ToString(predicateExpression)} item");
        }

        /// <summary>
        /// Verifies that collection contains exact count of items equal to <paramref name="expectedValue"/> parameter.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="expectedValue">An expected item value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            int expectedCount,
            TItem expectedValue)
            =>
            verifier.Satisfy(
                actual => actual != null && actual.Count(x => Equals(x, expectedValue)) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} item{(expectedCount != 1 ? "s" : null)}");

        /// <summary>
        /// Verifies that collection contains exact count of items equal to <paramref name="expectedValue"/> parameter.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="expectedValue">An expected object value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            int expectedCount,
            TObject expectedValue) =>
            verifier.Satisfy(
                actual => actual != null && actual.Count((TObject x) => Equals(x, expectedValue)) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} item{(expectedCount != 1 ? "s" : null)}");

        /// <summary>
        /// Verifies that collection contains exact count of items matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            int expectedCount,
            Expression<Func<TItem, bool>> predicateExpression)
        {
            expectedCount.CheckGreaterOrEqual(nameof(expectedCount), 0);
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Count(predicate) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(predicateExpression)} item{(expectedCount != 1 ? "s" : null)}");
        }

        /// <summary>
        /// Verifies that collection contains expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.Contain(expected.AsEnumerable());

        /// <summary>
        /// Verifies that collection contains expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && verifier.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains items equal to expected values.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.Contain(expected.AsEnumerable());

        /// <summary>
        /// Verifies that collection contains items equal to expected values.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && verifier.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains at least one item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain {Stringifier.ToString(predicateExpression)} item");
        }

        /// <summary>
        /// Verifies that collection contains at least one item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            Expression<Func<TObject, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain {Stringifier.ToString(predicateExpression)} item");
        }

        public static TOwner Contain<TOwner>(
            this IObjectVerificationProvider<IEnumerable<string>, TOwner> verifier,
            TermMatch match,
            params string[] expected)
            =>
            verifier.Contain(match, expected.AsEnumerable());

        public static TOwner Contain<TOwner>(
            this IObjectVerificationProvider<IEnumerable<string>, TOwner> verifier,
            TermMatch match,
            IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && verifier.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        public static TOwner Contain<TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> verifier,
            TermMatch match,
            params string[] expected)
            =>
            verifier.Contain(match, expected.AsEnumerable());

        public static TOwner Contain<TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> verifier,
            TermMatch match,
            IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && verifier.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        public static TOwner ContainHavingContent<TControl, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TControl>, TOwner> verifier,
            TermMatch match,
            params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            verifier.ContainHavingContent(match, expected.AsEnumerable());

        public static TOwner ContainHavingContent<TControl, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TControl>, TOwner> verifier,
            TermMatch match,
            IEnumerable<string> expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return verifier.IsNegation
                        ? expected.Any(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                        : expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain having content that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier)
            where TItem : IComparable<TItem> =>
            verifier.Satisfy(
                actual => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item of nullable value (struct) type.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem?>, TOwner> verifier)
            where TItem : struct, IComparable<TItem> =>
            verifier.Satisfy(
                actual => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier) =>
            verifier.Satisfy(
                (IEnumerable<TObject> actual) => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier)
            where TItem : IComparable<TItem>
            =>
            verifier.Satisfy(
                actual => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item of nullable value (struct) type.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem?>, TOwner> verifier)
            where TItem : struct, IComparable<TItem>
            =>
            verifier.Satisfy(
                actual => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier) =>
            verifier.Satisfy(
                (IEnumerable<TObject> actual) => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");
    }
}
