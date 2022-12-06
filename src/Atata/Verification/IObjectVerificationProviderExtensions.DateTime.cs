using System;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime, TOwner> verifier, DateTime expected)
        {
            var equalityComparer = verifier.ResolveEqualityComparer<DateTime>();

            return verifier.Satisfy(
                actual => equalityComparer.Equals(actual.Date, expected.Date),
                "equal date {0}",
                expected);
        }

        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime?, TOwner> verifier, DateTime expected)
        {
            var equalityComparer = verifier.ResolveEqualityComparer<DateTime>();

            return verifier.Satisfy(
                actual => actual != null && equalityComparer.Equals(actual.Value.Date, expected.Date),
                "equal date {0}",
                expected);
        }
    }
}
