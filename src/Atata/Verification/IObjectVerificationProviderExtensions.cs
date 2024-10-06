namespace Atata;

/// <summary>
/// Provides a set of verification extension methods.
/// </summary>
public static partial class IObjectVerificationProviderExtensions
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

                verifier.Strategy.ReportFailure(verifier.ExecutionUnit, failureMessage, exception);
            }
        }

        return VerificationUtils.Verify(verifier, ExecuteVerification, message, args);
    }

    private static bool TryShortenExpectedMessage(string originalMessage, out string resultMessage)
    {
        if (originalMessage is "equal {0}" or "be {0}")
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

        if (sourceVerificationProvider.TypeEqualityComparerMap != null)
        {
            if (resultVerificationProvider.TypeEqualityComparerMap is null)
            {
                resultVerificationProvider.TypeEqualityComparerMap = new Dictionary<Type, object>(
                    sourceVerificationProvider.TypeEqualityComparerMap);
            }
            else
            {
                foreach (var item in sourceVerificationProvider.TypeEqualityComparerMap)
                    resultVerificationProvider.TypeEqualityComparerMap[item.Key] = item.Value;
            }
        }

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
    public static TOwner Equal<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => equalityComparer.Equals(actual, expected),
            VerificationMessage.Of("equal {0}", equalityComparer),
            expected);
    }

    /// <summary>
    /// Verifies that object is equal to <paramref name="expected"/> value.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">The expected value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner Be<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier, TObject expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<TObject>();

        return verifier.Satisfy(
            actual => equalityComparer.Equals(actual, expected),
            VerificationMessage.Of("be {0}", equalityComparer),
            expected);
    }

    public static TOwner BeNull<TObject, TOwner>(this IObjectVerificationProvider<TObject, TOwner> verifier) =>
        verifier.Satisfy(actual => Equals(actual, null), "be null");
}
