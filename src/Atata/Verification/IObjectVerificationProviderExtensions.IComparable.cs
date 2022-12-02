using System;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
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
    }
}
