namespace Atata.UnitTests;

public sealed class VerificationMessageTests
{
    [Test]
    public void Ctor() =>
        new VerificationMessage("abc")
            .ToString()
            .Should().Be("abc");

    [Test]
    public void Append() =>
        new VerificationMessage("abc")
            .Append("def")
            .ToString()
            .Should().Be("abcdef");

    [Test]
    public void With_DefaultEqualityComparer() =>
        new VerificationMessage("abc")
            .With(EqualityComparer<int>.Default)
            .ToString()
            .Should().Be("abc");

    [Test]
    public void With_StringComparer_InvariantCulture() =>
        new VerificationMessage("abc")
            .With(StringComparer.InvariantCulture)
            .ToString()
            .Should().Be("abc");

    [Test]
    public void With_StringComparer_InvariantCultureIgnoreCase() =>
        new VerificationMessage("abc")
            .With(StringComparer.InvariantCultureIgnoreCase)
            .ToString()
            .Should().Be("abc ignoring case");

    [Test]
    public void With_CustomComparerImplementingIDescribesComparison() =>
        new VerificationMessage("abc")
            .With(new CustomEqualityComparer())
            .ToString()
            .Should().Be("abc using custom comparison");

    public class CustomEqualityComparer : IEqualityComparer<int>, IDescribesComparison
    {
        public bool Equals(int x, int y) =>
            x == y;

        public int GetHashCode(int obj) =>
            obj.GetHashCode();

        public string GetComparisonDescription() =>
            "using custom comparison";
    }
}
