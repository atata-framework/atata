namespace Atata.UnitTests;

public static class TestInfoTests
{
    public sealed class IsEmpty
    {
        [Test]
        public void WhenAllConstructorParametersAreNull() =>
            new TestInfo(null)
                .IsEmpty.Should().BeTrue();

        [Test]
        public void WhenSuiteTypeIsSet() =>
            new TestInfo(typeof(TestInfoTests))
                .IsEmpty.Should().BeFalse();

        [Test]
        public void WhenTraitsAreSet() =>
            new TestInfo(null, traits: [new("Some", "trait")])
                .IsEmpty.Should().BeFalse();
    }

    public sealed class BelongsToNamespace
    {
        [TestCase("Atata", ExpectedResult = true)]
        [TestCase("Atata.UnitTests", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("X", ExpectedResult = false)]
        [TestCase("AtataX", ExpectedResult = false)]
        [TestCase("Atata.UnitTestsX", ExpectedResult = false)]
        [TestCase("Atata.UnitTests.X", ExpectedResult = false)]
        public bool With(string targetNamespace) =>
            new TestInfo("TestX", typeof(TestInfoTests), "some")
                .BelongsToNamespace(targetNamespace);

        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("X", ExpectedResult = false)]
        public bool WhenNamespaceIsNull_With(string targetNamespace) =>
            new TestInfo("TestX", null, null)
                .BelongsToNamespace(targetNamespace);
    }

    public sealed class FullName
    {
        [Test]
        public void WhenSuiteNameIsNull() =>
            new TestInfo("TestX", typeof(TestInfoTests), null)
                .FullName.Should().Be("Atata.UnitTests.TestInfoTests.TestX");

        [Test]
        public void WhenSuiteNameIsSet() =>
            new TestInfo("TestX", typeof(TestInfoTests), "OverX")
                .FullName.Should().Be("Atata.UnitTests.OverX.TestX");

        [Test]
        public void WhenNameAndSuiteNameAreNull() =>
            new TestInfo(null, typeof(TestInfoTests), null)
                .FullName.Should().Be("Atata.UnitTests.TestInfoTests");

        [Test]
        public void WhenAllArgumentsAreNull() =>
            new TestInfo(null, null, null, null)
                .FullName.Should().BeNull();

        [Test]
        public void WhenAllArgumentsAreSet() =>
            new TestInfo("TestX", typeof(TestInfoTests), "OverX", "Group")
                .FullName.Should().Be("Atata.UnitTests.OverX.TestX");

        [Test]
        public void WhenSuiteNameAndSuiteTypeAreNull() =>
            new TestInfo("TestX", null, null)
                .FullName.Should().Be("TestX");
    }
}
