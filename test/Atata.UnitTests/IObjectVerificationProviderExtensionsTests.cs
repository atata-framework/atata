namespace Atata.UnitTests;

public static class IObjectVerificationProviderExtensionsTests
{
    public static class Enumerable
    {
        public sealed class ConsistOf
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

        public sealed class ConsistOfSingle
        {
            [Test]
            public void Ok() =>
                new[] { 1 }.ToSutSubject()
                    .Should.ConsistOfSingle(1);

            [Test]
            public void WhenSingleButDoesNotMatch() =>
                Assert.Throws<AssertionException>(() =>
                    new[] { 1 }.ToSutSubject()
                        .Should.ConsistOfSingle(2));

            [Test]
            public void WhenEmpty() =>
                Assert.Throws<AssertionException>(() =>
                    Array.Empty<int>().ToSutSubject()
                        .Should.ConsistOfSingle(1));

            [Test]
            public void WhenNull() =>
                Assert.Throws<AssertionException>(() =>
                    (null as int[]).ToSutSubject()
                        .Should.ConsistOfSingle(1));

            [Test]
            public void Not_WhenSingleButDoesNotMatch() =>
                new[] { 1 }.ToSutSubject()
                    .Should.Not.ConsistOfSingle(2);

            [Test]
            public void Not_WhenMultipleAndOneMatches() =>
                new[] { 1, 2 }.ToSutSubject()
                    .Should.Not.ConsistOfSingle(2);

            [Test]
            public void Not_WhenEmpty() =>
                Array.Empty<int>().ToSutSubject()
                    .Should.Not.ConsistOfSingle(1);

            [Test]
            public void Not_WhenNull() =>
                (null as int[]).ToSutSubject()
                    .Should.Not.ConsistOfSingle(1);
        }

        public sealed class ConsistSequentiallyOf
        {
            [Test]
            public void Ok() =>
                new[] { 1, 2 }.ToSutSubject()
                    .Should.ConsistSequentiallyOf(x => x == 1, x => x == 2);

            [Test]
            public void WithNull() =>
                Assert.Throws<ArgumentNullException>(() =>
                    new[] { 1, 2 }.ToSutSubject()
                        .Should.ConsistSequentiallyOf(null!));

            [Test]
            public void WithEmpty() =>
                Assert.Throws<ArgumentException>(() =>
                    new[] { 1, 2 }.ToSutSubject()
                        .Should.ConsistSequentiallyOf());

            [Test]
            public void WhenDoesNotMatch() =>
                Assert.Throws<AssertionException>(() =>
                    new[] { 1, 2 }.ToSutSubject()
                        .Should.ConsistSequentiallyOf(x => x == 1, x => x == 3));

            [Test]
            public void Not_WhenDoesNotMatch() =>
                new[] { 1, 2 }.ToSutSubject()
                    .Should.Not.ConsistSequentiallyOf(x => x == 1, x => x == 3);

            [Test]
            public void Not_WhenCountDiffers() =>
                new[] { 1, 2, 3 }.ToSutSubject()
                    .Should.Not.ConsistSequentiallyOf(x => x == 1, x => x == 2);

            [Test]
            public void Not_WhenEmpty() =>
                new int[0].ToSutSubject()
                    .Should.Not.ConsistSequentiallyOf(x => x == 1, x => x == 2);
        }
    }
}
