namespace Atata;

public static partial class IObjectVerificationProviderExtensions
{
    /// <summary>
    /// Verifies that the <see cref="Uri"/> is equal to the <paramref name="expected"/> value.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">The expected value.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner Be<TOwner>(
        this IObjectVerificationProvider<Uri, TOwner> verifier,
        string expected)
    {
        var equalityComparer = verifier.ResolveEqualityComparer<string>();

        return verifier.Satisfy(
            actual => equalityComparer.Equals(actual.ToString(), expected),
            VerificationMessage.Of($"be {Stringifier.ToString(expected)}", equalityComparer));
    }
}
