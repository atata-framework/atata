using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
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

        public static TOwner BeEmpty<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier) =>
            verifier.Satisfy(actual => actual != null && !actual.Any(), "be empty");

        public static TOwner HaveCount<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, int expected) =>
            verifier.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, params TObject[] expected) =>
            verifier.BeEquivalent(expected?.AsEnumerable());

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
            verifier.BeEquivalent(expected?.AsEnumerable());

        public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, params TObject[] expected) =>
            verifier.EqualSequence(expected?.AsEnumerable());

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNull(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
            verifier.EqualSequence(expected?.AsEnumerable());

        public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
        {
            expected.CheckNotNull(nameof(expected));

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
            verifier.Contain(expected?.AsEnumerable());

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
                    : actual.Intersect(expected).Count() == expected.Distinct().Count(),
                $"contain {Stringifier.ToStringInFormOfOneOrMany(expected)}");
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
            verifier.Contain(expected?.AsEnumerable());

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
                    : actual.Intersect(expected).Count() == expected.Distinct().Count(),
                $"contain {Stringifier.ToStringInFormOfOneOrMany(expected)}");
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
            verifier.Contain(match, expected?.AsEnumerable());

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
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToStringInFormOfOneOrMany(expected)}");
        }

        public static TOwner Contain<TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> verifier,
            TermMatch match,
            params string[] expected)
            =>
            verifier.Contain(match, expected?.AsEnumerable());

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
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToStringInFormOfOneOrMany(expected)}");
        }

        /// <inheritdoc cref="ContainAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
        public static TOwner ContainAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.ContainAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection contains any of the expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Intersect(expected).Any(),
                $"contain any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="ContainAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
        public static TOwner ContainAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.ContainAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection contains any of the expected items.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Intersect(expected).Any(),
                $"contain any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="StartWith{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
        public static TOwner StartWith<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.StartWith(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection starts sequentially with the expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner StartWith<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() >= expected.Count() && actual.Zip(expected, (a, e) => EqualityComparer<TItem>.Default.Equals(a, e)).All(x => x),
                $"start with {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="StartWith{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
        public static TOwner StartWith<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.StartWith(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection starts sequentially with the expected items.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner StartWith<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() >= expected.Count() && actual.Zip(expected, (a, e) => EqualityComparer<TObject>.Default.Equals(a, e)).All(x => x),
                $"start with {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="StartWithAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
        public static TOwner StartWithAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.StartWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection starts with any of the expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner StartWithAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual =>
                {
                    if (actual is null)
                        return false;

                    using (var actualEnumerator = actual.GetEnumerator())
                    {
                        return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current);
                    }
                },
                $"start with any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="StartWithAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
        public static TOwner StartWithAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.StartWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection starts with any of the expected items.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner StartWithAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual =>
                {
                    if (actual is null)
                        return false;

                    using (var actualEnumerator = actual.GetEnumerator())
                    {
                        return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current);
                    }
                },
                $"start with any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="EndWith{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
        public static TOwner EndWith<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.EndWith(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection ends sequentially with the expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner EndWith<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() >= expected.Count() && actual.Reverse().Zip(expected.Reverse(), (a, e) => EqualityComparer<TItem>.Default.Equals(a, e)).All(x => x),
                $"ends with {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="EndWith{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
        public static TOwner EndWith<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.EndWith(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection ends sequentially with the expected items.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner EndWith<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual => actual != null && actual.Count() >= expected.Count() && actual.Reverse().Zip(expected.Reverse(), (a, e) => EqualityComparer<TObject>.Default.Equals(a, e)).All(x => x),
                $"ends with {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="EndWithAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
        public static TOwner EndWithAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params TItem[] expected)
            =>
            verifier.EndWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection ends with any of the expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner EndWithAny<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual =>
                {
                    if (actual is null)
                        return false;

                    using (var actualEnumerator = actual.Reverse().GetEnumerator())
                    {
                        return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current);
                    }
                },
                $"end with any of {Stringifier.ToString(expected)}");
        }

        /// <inheritdoc cref="EndWithAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
        public static TOwner EndWithAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params TObject[] expected)
            =>
            verifier.EndWithAny(expected?.AsEnumerable());

        /// <summary>
        /// Verifies that collection ends with any of the expected items.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="expected">An expected object values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner EndWithAny<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            IEnumerable<TObject> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return verifier.Satisfy(
                actual =>
                {
                    if (actual is null)
                        return false;

                    using (var actualEnumerator = actual.Reverse().GetEnumerator())
                    {
                        return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current);
                    }
                },
                $"end with any of {Stringifier.ToString(expected)}");
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

        /// <summary>
        /// Verifies that collection consists of a single item and the item matches the <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ConsistOfSingle<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == 1 && predicate(actual.Single()),
                $"consist of single {Stringifier.ToString(predicateExpression)} item");
        }

        /// <inheritdoc cref="ConsistOfSingle{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Expression{Func{TItem, bool}})"/>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        public static TOwner ConsistOfSingle<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            Expression<Func<TObject, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == 1 && predicate(actual.Single()),
                $"consist of single {Stringifier.ToString(predicateExpression)} item");
        }

        /// <summary>
        /// Verifies that collection consists sequentially of items that match one by one the <paramref name="predicateExpressions"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="predicateExpressions">The predicate expressions.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ConsistSequentiallyOf<TItem, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TItem>, TOwner> verifier,
            params Expression<Func<TItem, bool>>[] predicateExpressions)
        {
            var predicates = predicateExpressions.CheckNotNullOrEmpty(nameof(predicateExpressions))
                .Select(x => x.Compile())
                .ToArray();

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == predicates.Length && actual.Select((x, index) => predicates[index](x)).All(x => x),
                $"consist sequentially of {Stringifier.ToString(predicateExpressions)} items");
        }

        /// <inheritdoc cref="ConsistSequentiallyOf{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Expression{Func{TItem, bool}}[])"/>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        public static TOwner ConsistSequentiallyOf<TObject, TOwner>(
            this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
            params Expression<Func<TObject, bool>>[] predicateExpressions)
        {
            var predicates = predicateExpressions.CheckNotNullOrEmpty(nameof(predicateExpressions))
                .Select(x => x.Compile())
                .ToArray();

            return verifier.Satisfy(
                actual => actual != null && actual.Count() == predicates.Length && actual.Select((x, index) => predicates[index](x)).All(x => x),
                $"consist sequentially of {Stringifier.ToString(predicateExpressions)} items");
        }
    }
}
