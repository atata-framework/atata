namespace Atata.UnitTests;

public static class IObjectVerificationProviderExtensionsTests
{
    public static class Enumerable
    {
        [TestFixture]
        public class ConsistOf
        {
            [Test]
            public void Ok() =>
                new[] { 1 }.ToSutSubject()
                    .Should.ConsistOfSingle(x => x == 1);

            [Test]
            public void WhenSingleButDoesNotMatch() =>
                Assert.Throws<AssertionException>(() =>
                    new[] { 1 }.ToSutSubject()
                        .Should.ConsistOfSingle(x => x == 2));

            [Test]
            public void Not_WhenSingleButDoesNotMatch() =>
                new[] { 1 }.ToSutSubject()
                    .Should.Not.ConsistOfSingle(x => x == 2);

            [Test]
            public void Not_WhenMultipleAndOneMatches() =>
                new[] { 1, 2 }.ToSutSubject()
                    .Should.Not.ConsistOfSingle(x => x == 2);
        }
    }
}
