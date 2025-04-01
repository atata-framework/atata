#nullable enable

namespace Atata;

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
    /// <returns>The owner instance.</returns>
    public static TOwner Satisfy<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        Func<IEnumerable<TObject>, bool> predicate,
        string message)
    {
        verifier.CheckNotNull(nameof(verifier));
        predicate.CheckNotNull(nameof(predicate));

        string expectedMessage = message;

        void ExecuteVerification()
        {
            IEnumerable<TObject>? actual = null;
            Exception? exception = null;

            bool doesSatisfy = VerificationUtils.ExecuteUntil(
                () =>
                {
                    try
                    {
                        actual = verifier.ObjectProvider.Object?.Select(x => x.Object).ToArray();
                        bool result = predicate(actual!) != verifier.IsNegation;
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
                string? actualMessage = exception is null
                    ? Stringifier.ToString(actual)
                    : null;

                string failureMessage = VerificationUtils.BuildFailureMessage(verifier, expectedMessage, actualMessage);

                verifier.Strategy.ReportFailure(verifier.ExecutionUnit, failureMessage, exception);
            }
        }

        return VerificationUtils.Verify(verifier, ExecuteVerification, expectedMessage);
    }

    public static TOwner BeEmpty<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>?, TOwner> verifier) =>
        verifier.Satisfy(actual => actual != null && !actual.Any(), "be empty");

    public static TOwner HaveCount<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<TObject>?, TOwner> verifier, int expected) =>
        verifier.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");

    public static TOwner BeEquivalent<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, params TItem[] expected) =>
        verifier.BeEquivalent(expected?.AsEnumerable()!);

    public static TOwner BeEquivalent<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, IEnumerable<TItem> expected)
    {
        expected.CheckNotNull(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null || actual.Count() != expected.Count())
                    return false;

                var expectedList = new List<TItem>(expected);

                return actual.All(x => RemoveFromListIfContains(expectedList, x, equalityComparer));
            },
            VerificationMessage.Of($"be equivalent to {Stringifier.ToString(expected)}", equalityComparer));
    }

    public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
        verifier.BeEquivalent(expected?.AsEnumerable()!);

    public static TOwner BeEquivalent<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
    {
        expected.CheckNotNull(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null || actual.Count() != expected.Count())
                    return false;

                var expectedList = new List<TObject>(expected);

                return actual.All(x => RemoveFromListIfContains(expectedList, x, equalityComparer));
            },
            VerificationMessage.Of($"be equivalent to {Stringifier.ToString(expected)}", equalityComparer));
    }

    public static TOwner EqualSequence<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, params TItem[] expected) =>
        verifier.EqualSequence(expected?.AsEnumerable()!);

    public static TOwner EqualSequence<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, IEnumerable<TItem> expected)
    {
        expected.CheckNotNull(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.SequenceEqual(expected, equalityComparer),
            VerificationMessage.Of($"equal sequence {Stringifier.ToString(expected)}", equalityComparer));
    }

    public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, params TObject[] expected) =>
        verifier.EqualSequence(expected?.AsEnumerable()!);

    public static TOwner EqualSequence<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, IEnumerable<TObject> expected)
    {
        expected.CheckNotNull(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.SequenceEqual(expected, equalityComparer),
            VerificationMessage.Of($"equal sequence {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection contains only a single item.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ContainSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier) =>
        verifier.Satisfy(actual => actual != null && actual.Count() == 1, $"contain single item");

    /// <summary>
    /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ContainSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, TItem expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(x => equalityComparer.Equals(x, expected)) == 1,
            VerificationMessage.Of($"contain single {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
    /// </summary>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected object value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ContainSingle<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, TObject expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(x => equalityComparer.Equals(x, expected)) == 1,
            VerificationMessage.Of($"contain single {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection contains a single item matching <paramref name="predicateExpression"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="predicateExpression">The predicate expression.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ContainSingle<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        Expression<Func<TItem, bool>> predicateExpression)
    {
        var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(predicate) == 1,
            $"contain single {Stringifier.ToString(predicateExpression)} item");
    }

    /// <inheritdoc cref="ContainSingle{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Expression{Func{TItem, bool}})"/>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ContainSingle<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        Expression<Func<TObject, bool>> predicateExpression)
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
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        int expectedCount,
        TItem expectedValue)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(x => equalityComparer.Equals(x, expectedValue)) == expectedCount,
            VerificationMessage.Of(
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} item{(expectedCount != 1 ? "s" : null)}",
                equalityComparer));
    }

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
        TObject expectedValue)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(x => equalityComparer.Equals(x, expectedValue)) == expectedCount,
            VerificationMessage.Of(
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} item{(expectedCount != 1 ? "s" : null)}",
                equalityComparer));
    }

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
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        int expectedCount,
        Expression<Func<TItem, bool>> predicateExpression)
    {
        expectedCount.CheckGreaterOrEqual(nameof(expectedCount), 0);
        var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

        return verifier.Satisfy(
            actual => actual != null && actual.Count(predicate) == expectedCount,
            $"contain exactly {expectedCount} {Stringifier.ToString(predicateExpression)} item{(expectedCount != 1 ? "s" : null)}");
    }

    /// <inheritdoc cref="ContainExactly{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, int, Expression{Func{TItem, bool}})"/>
    /// <typeparam name="TObject">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ContainExactly<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        int expectedCount,
        Expression<Func<TObject, bool>> predicateExpression)
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
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.Contain(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection contains expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner Contain<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && verifier.IsNegation
                ? actual.Intersect(expected, equalityComparer).Any()
                : actual.Intersect(expected, equalityComparer).Count() == expected.Distinct(equalityComparer).Count(),
            VerificationMessage.Of($"contain {Stringifier.ToStringInFormOfOneOrMany(expected)}", equalityComparer));
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
        verifier.Contain(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && verifier.IsNegation
                ? actual.Intersect(expected, equalityComparer).Any()
                : actual.Intersect(expected, equalityComparer).Count() == expected.Distinct(equalityComparer).Count(),
            VerificationMessage.Of($"contain {Stringifier.ToStringInFormOfOneOrMany(expected)}", equalityComparer));
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
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
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
        this IObjectVerificationProvider<IEnumerable<string>?, TOwner> verifier,
        TermMatch match,
        params string[] expected)
        =>
        verifier.Contain(match, expected?.AsEnumerable()!);

    public static TOwner Contain<TOwner>(
        this IObjectVerificationProvider<IEnumerable<string>?, TOwner> verifier,
        TermMatch match,
        IEnumerable<string> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var predicate = match.GetPredicate(verifier.ResolveStringComparison());

        return verifier.Satisfy(
            actual => actual != null && verifier.IsNegation
                ? expected.Any(expectedValue => actual.Any(actualValue => predicate(actualValue, expectedValue)))
                : expected.All(expectedValue => actual.Any(actualValue => predicate(actualValue, expectedValue))),
            VerificationMessage.Of(
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToStringInFormOfOneOrMany(expected)}",
                verifier.ResolveEqualityComparer<string>()));
    }

    public static TOwner Contain<TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> verifier,
        TermMatch match,
        params string[] expected)
        =>
        verifier.Contain(match, expected?.AsEnumerable()!);

    public static TOwner Contain<TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> verifier,
        TermMatch match,
        IEnumerable<string> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var predicate = match.GetPredicate(verifier.ResolveStringComparison());

        return verifier.Satisfy(
            actual => actual != null && verifier.IsNegation
                ? expected.Any(expectedValue => actual.Any(actualValue => predicate(actualValue, expectedValue)))
                : expected.All(expectedValue => actual.Any(actualValue => predicate(actualValue, expectedValue))),
            VerificationMessage.Of(
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToStringInFormOfOneOrMany(expected)}",
                verifier.ResolveEqualityComparer<string>()));
    }

    /// <inheritdoc cref="ContainAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
    public static TOwner ContainAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.ContainAny(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection contains any of the expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ContainAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Intersect(expected, equalityComparer).Any(),
            VerificationMessage.Of($"contain any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="ContainAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
    public static TOwner ContainAny<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params TObject[] expected)
        =>
        verifier.ContainAny(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Intersect(expected, equalityComparer).Any(),
            VerificationMessage.Of($"contain any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="StartWith{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
    public static TOwner StartWith<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.StartWith(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection starts sequentially with the expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner StartWith<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() >= expected.Count() && actual.Zip(expected, (a, e) => equalityComparer.Equals(a, e)).All(x => x),
            VerificationMessage.Of($"start with {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="StartWith{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
    public static TOwner StartWith<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params TObject[] expected)
        =>
        verifier.StartWith(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() >= expected.Count() && actual.Zip(expected, (a, e) => equalityComparer.Equals(a, e)).All(x => x),
            VerificationMessage.Of($"start with {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="StartWithAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
    public static TOwner StartWithAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.StartWithAny(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection starts with any of the expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner StartWithAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null)
                    return false;

                using var actualEnumerator = actual.GetEnumerator();

                return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current, equalityComparer);
            },
            VerificationMessage.Of($"start with any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="StartWithAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
    public static TOwner StartWithAny<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params TObject[] expected)
        =>
        verifier.StartWithAny(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null)
                    return false;

                using var actualEnumerator = actual.GetEnumerator();

                return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current, equalityComparer);
            },
            VerificationMessage.Of($"start with any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="EndWith{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
    public static TOwner EndWith<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.EndWith(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection ends sequentially with the expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner EndWith<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() >= expected.Count() && actual.Reverse().Zip(expected.Reverse(), (a, e) => equalityComparer.Equals(a, e)).All(x => x),
            VerificationMessage.Of($"ends with {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="EndWith{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
    public static TOwner EndWith<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params TObject[] expected)
        =>
        verifier.EndWith(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() >= expected.Count() && actual.Reverse().Zip(expected.Reverse(), (a, e) => equalityComparer.Equals(a, e)).All(x => x),
            VerificationMessage.Of($"ends with {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="EndWithAny{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, IEnumerable{TItem})"/>
    public static TOwner EndWithAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params TItem[] expected)
        =>
        verifier.EndWithAny(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that collection ends with any of the expected items.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner EndWithAny<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        IEnumerable<TItem> expected)
    {
        expected.CheckNotNullOrEmpty(nameof(expected));

        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null)
                    return false;

                using var actualEnumerator = actual.Reverse().GetEnumerator();

                return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current, equalityComparer);
            },
            VerificationMessage.Of($"end with any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <inheritdoc cref="EndWithAny{TObject, TOwner}(IObjectVerificationProvider{IEnumerable{IObjectProvider{TObject}}, TOwner}, IEnumerable{TObject})"/>
    public static TOwner EndWithAny<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params TObject[] expected)
        =>
        verifier.EndWithAny(expected?.AsEnumerable()!);

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

        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual =>
            {
                if (actual is null)
                    return false;

                using var actualEnumerator = actual.Reverse().GetEnumerator();

                return actualEnumerator.MoveNext() && expected.Contains(actualEnumerator.Current, equalityComparer);
            },
            VerificationMessage.Of($"end with any of {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection is sorted in ascending order.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner BeInAscendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier)
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
    public static TOwner BeInAscendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem?>?, TOwner> verifier)
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
    public static TOwner BeInDescendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier)
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
    public static TOwner BeInDescendingOrder<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem?>?, TOwner> verifier)
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
    /// Verifies that collection consists of items that match the <paramref name="predicateExpressions"/> in an arbitrary order.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="predicateExpressions">The predicate expressions.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOf<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        params Expression<Func<TItem, bool>>[] predicateExpressions)
    {
        var predicates = predicateExpressions.CheckNotNullOrEmpty(nameof(predicateExpressions))
            .Select(x => x.Compile())
            .ToArray();

        return verifier.Satisfy(
            actual => actual != null && DoItemsMatchPredicates((actual as IReadOnlyList<TItem>) ?? actual.ToArray(), predicates),
            $"consist of {Stringifier.ToString(predicateExpressions)} items");
    }

    /// <inheritdoc cref="ConsistOf{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Expression{Func{TItem, bool}}[])"/>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ConsistOf<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        params Expression<Func<TObject, bool>>[] predicateExpressions)
    {
        var predicates = predicateExpressions.CheckNotNullOrEmpty(nameof(predicateExpressions))
            .Select(x => x.Compile())
            .ToArray();

        return verifier.Satisfy(
            actual => actual != null && DoItemsMatchPredicates((actual as IReadOnlyList<TObject>) ?? actual.ToArray(), predicates),
            $"consist of {Stringifier.ToString(predicateExpressions)} items");
    }

    /// <summary>
    /// Verifies that collection consists only of items that match the <paramref name="predicateExpression"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="predicateExpression">The predicate expression.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOnlyOf<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        Expression<Func<TItem, bool>> predicateExpression)
    {
        var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => predicate(x)),
            $"consist only of {Stringifier.ToString(predicateExpression)} items");
    }

    /// <inheritdoc cref="ConsistOnlyOf{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Expression{Func{TItem, bool}})"/>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ConsistOnlyOf<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        Expression<Func<TObject, bool>> predicateExpression)
    {
        var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => predicate(x)),
            $"consist only of {Stringifier.ToString(predicateExpression)} items");
    }

    /// <summary>
    /// Verifies that collection consists only of items that match the <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <param name="message">The message that should sound in a way of "{Collection} should consist of items that {message}".</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOnlyOf<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        Func<TItem, bool> predicate,
        string message)
    {
        predicate.CheckNotNull(nameof(predicate));

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => predicate(x)),
            $"consist only of items that {message ?? "match predicate"}");
    }

    /// <inheritdoc cref="ConsistOnlyOf{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, Func{TItem, bool}, string)"/>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ConsistOnlyOf<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        Func<TObject, bool> predicate,
        string message)
    {
        predicate.CheckNotNull(nameof(predicate));

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => predicate(x)),
            $"consist only of items that {message ?? "match predicate"}");
    }

    /// <summary>
    /// Verifies that collection consists only of items that equal to the <paramref name="expected"/> value.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">The expected value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOnlyOf<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
        TItem expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => equalityComparer.Equals(x, expected)),
            VerificationMessage.Of($"consist only of {Stringifier.ToString(expected)} items", equalityComparer));
    }

    /// <inheritdoc cref="ConsistOnlyOf{TItem, TOwner}(IObjectVerificationProvider{IEnumerable{TItem}, TOwner}, TItem)"/>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public static TOwner ConsistOnlyOf<TObject, TOwner>(
        this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier,
        TObject expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.All(x => equalityComparer.Equals(x, expected)),
            VerificationMessage.Of($"consist only of {Stringifier.ToString(expected)} items", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection consists of a single item and the item equals to the <paramref name="expected"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected item value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOfSingle<TItem, TOwner>(this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier, TItem expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TItem>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() == 1 && equalityComparer.Equals(actual.FirstOrDefault(), expected),
            VerificationMessage.Of($"consist of single {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection consists of a single item and the item equals to the <paramref name="expected"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the collection item object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">An expected object value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOfSingle<TObject, TOwner>(this IObjectVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> verifier, TObject expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => actual != null && actual.Count() == 1 && equalityComparer.Equals(actual.FirstOrDefault(), expected),
            VerificationMessage.Of($"consist of single {Stringifier.ToString(expected)}", equalityComparer));
    }

    /// <summary>
    /// Verifies that collection consists of a single item and the item matches the <paramref name="predicateExpression"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the collection item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="predicateExpression">The predicate expression.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner ConsistOfSingle<TItem, TOwner>(
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
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
        this IObjectVerificationProvider<IEnumerable<TItem>?, TOwner> verifier,
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

    private static bool RemoveFromListIfContains<T>(List<T> list, T item, IEqualityComparer<T> equalityComparer)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (equalityComparer.Equals(list[i], item))
            {
                list.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    private static bool DoItemsMatchPredicates<T>(IReadOnlyList<T> items, Func<T, bool>[] predicates)
    {
        if (items.Count != predicates.Length)
            return false;

        List<List<int>> predicatePassers = [.. Enumerable.Repeat(0, items.Count).Select(_ => new List<int>())];

        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < predicates.Length; j++)
            {
                if (predicates[j].Invoke(items[i]))
                    predicatePassers[j].Add(i);
            }
        }

        predicatePassers.Sort((a, b) => a.Count.CompareTo(b.Count));

        return predicatePassers[0].Count != 0 && SortOut(predicatePassers, []);

        static bool SortOut(IEnumerable<List<int>> numbers, List<int> excludedNumbers)
        {
            int[] nonExcludedCurrentNumbers = [.. numbers.First().Except(excludedNumbers)];

            if (numbers.Count() == 1)
                return nonExcludedCurrentNumbers.Length > 0;

            foreach (int number in nonExcludedCurrentNumbers)
            {
                excludedNumbers.Add(number);
                if (SortOut(numbers.Skip(1), excludedNumbers))
                    return true;
            }

            return false;
        }
    }
}
