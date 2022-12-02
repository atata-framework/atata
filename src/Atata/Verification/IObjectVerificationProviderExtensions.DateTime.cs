using System;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime, TOwner> verifier, DateTime expected) =>
            verifier.Satisfy(actual => Equals(actual.Date, expected.Date), "equal date {0}", expected);

        public static TOwner EqualDate<TOwner>(this IObjectVerificationProvider<DateTime?, TOwner> verifier, DateTime expected) =>
            verifier.Satisfy(actual => actual != null && Equals(actual.Value.Date, expected.Date), "equal date {0}", expected);
    }
}
