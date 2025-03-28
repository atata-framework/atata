#nullable enable

namespace Atata;

public static partial class IObjectVerificationProviderExtensions
{
    public static TOwner BeTrue<TOwner>(this IObjectVerificationProvider<bool, TOwner> verifier) =>
        verifier.Be(true);

    public static TOwner BeTrue<TOwner>(this IObjectVerificationProvider<bool?, TOwner> verifier) =>
        verifier.Be(true);

    public static TOwner BeFalse<TOwner>(this IObjectVerificationProvider<bool, TOwner> verifier) =>
        verifier.Be(false);

    public static TOwner BeFalse<TOwner>(this IObjectVerificationProvider<bool?, TOwner> verifier) =>
        verifier.Be(false);
}
