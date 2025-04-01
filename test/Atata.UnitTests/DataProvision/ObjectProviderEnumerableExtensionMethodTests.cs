#nullable enable

namespace Atata.UnitTests.DataProvision;

public static class ObjectProviderEnumerableExtensionMethodTests
{
    public sealed class Contains
    {
        private readonly Subject<Subject<int[]>> _sut = new[] { 1, 2, 3, 5, 8, 13 }.ToSubject().ToSutSubject();

        [Test]
        public void WithNull() =>
            _sut.ResultOf(x => x.Contains(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithEmpty() =>
            _sut.ResultOf(x => x.Contains())
                .Should.Throw<ArgumentException>();

        [Test]
        public void WithPresentItem() =>
            _sut.ResultOf(x => x.Contains(5))
                .Should.BeTrue();

        [Test]
        public void WithMissingItem() =>
            _sut.ResultOf(x => x.Contains(4))
                .Should.BeFalse();

        [Test]
        public void WithMultiplePresentItems() =>
            _sut.ResultOf(x => x.Contains(1, 5, 13))
                .Should.BeTrue();

        [Test]
        public void WithPresentAndMissingItems() =>
            _sut.ResultOf(x => x.Contains(1, 5, 12))
                .Should.BeFalse();

        [Test]
        public void WithDuplicatePresentItems() =>
            _sut.ResultOf(x => x.Contains(1, 5, 5))
                .Should.BeTrue();
    }

    public sealed class ContainsAny
    {
        private readonly Subject<Subject<int[]>> _sut = new[] { 1, 2, 3, 5, 8, 13 }.ToSubject().ToSutSubject();

        [Test]
        public void WithNull() =>
            _sut.ResultOf(x => x.ContainsAny(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithEmpty() =>
            _sut.ResultOf(x => x.ContainsAny())
                .Should.Throw<ArgumentException>();

        [Test]
        public void WithPresentItem() =>
            _sut.ResultOf(x => x.ContainsAny(5))
                .Should.BeTrue();

        [Test]
        public void WithMissingItem() =>
            _sut.ResultOf(x => x.ContainsAny(4))
                .Should.BeFalse();

        [Test]
        public void WithMultiplePresentItems() =>
            _sut.ResultOf(x => x.ContainsAny(1, 5, 13))
                .Should.BeTrue();

        [Test]
        public void WithPresentAndMissingItems() =>
            _sut.ResultOf(x => x.ContainsAny(1, 5, 12))
                .Should.BeTrue();

        [Test]
        public void WithDuplicatePresentItems() =>
            _sut.ResultOf(x => x.ContainsAny(5, 5))
                .Should.BeTrue();
    }
}
