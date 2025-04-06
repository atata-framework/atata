namespace Atata.UnitTests;

public static class GenericCollectionAssertionsExtensions
{
    public static AndConstraint<GenericCollectionAssertions<T>> BeSameSequenceAs<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expected)
    {
        var result = assertions.BeEquivalentTo(expected);

        assertions.Subject.SequenceEqual(expected, ReferenceEqualityComparer<T>.Default).Should().BeTrue("sequence should equal");

        return result;
    }

    public static AndConstraint<GenericCollectionAssertions<T>> BeSameSequenceAs<T>(this GenericCollectionAssertions<T> assertions, IEnumerable expected) =>
        assertions.BeSameSequenceAs(expected.Cast<T>());

    public static AndConstraint<GenericCollectionAssertions<T>> BeSameSequenceAs<T>(this GenericCollectionAssertions<T> assertions, params object[] expected) =>
        assertions.BeSameSequenceAs(expected.Cast<T>());

    public class ReferenceEqualityComparer<T> : EqualityComparer<T>
    {
        private static IEqualityComparer<T>? s_defaultComparer;

        public static new IEqualityComparer<T> Default =>
            s_defaultComparer ??= new ReferenceEqualityComparer<T>();

        public override bool Equals(T? x, T? y) =>
            ReferenceEquals(x, y);

        public override int GetHashCode(T obj) =>
            RuntimeHelpers.GetHashCode(obj);
    }
}
