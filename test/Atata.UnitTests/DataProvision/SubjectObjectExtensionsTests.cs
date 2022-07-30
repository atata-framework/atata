namespace Atata.UnitTests.DataProvision;

public static class SubjectObjectExtensionsTests
{
    [TestFixture]
    public static class ToSubject
    {
        [Test]
        public static void WithName() =>
            42.ToSubject("some name")
                .ProviderName.Should().Be("some name");

        [Test]
        public static void DefaultName() =>
            42.ToSubject()
                .ProviderName.Should().Be("subject");

        [Test]
        public static void ReferenceType() =>
            new Uri("/", UriKind.Relative).ToSubject()
                .Object.Should().NotBeNull();

        [Test]
        public static void ValueType() =>
            42.ToSubject()
                .Object.Should().Be(42);

        [Test]
        public static void NullValue() =>
            (null as Uri).ToSubject()
                .Object.Should().BeNull();
    }
}
