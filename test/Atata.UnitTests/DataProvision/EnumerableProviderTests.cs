namespace Atata.UnitTests.DataProvision;

[TestFixture]
public class EnumerableProviderTests
{
    private const string ExpectedSourceProviderName = nameof(TestOwner) + "." + nameof(TestOwner.Items);

    [Test]
    public void ProviderName() =>
        CreateSut(() => null)
            .ProviderName.ToSubject().Should.Equal(ExpectedSourceProviderName);

    [Test]
    public void Value_CanThrow()
    {
        var subject = CreateSut(() => throw new NotSupportedException());

        Assert.Throws<NotSupportedException>(() =>
            _ = subject.Value);
    }

    [Test]
    public void Value_CanBeGotWithRetries()
    {
        int tryCount = 0;

        var subject = CreateSut(() =>
            tryCount++ < 3
            ? new int[0]
            : new[] { 1, 2 });

        subject.Should.Not.BeEmpty();

        tryCount.ToSubject(nameof(tryCount)).Should.Equal(4);
    }

    [Test]
    public void Indexer() =>
        CreateSut(() => new[] { 10, 20, 30 })[1]
            .Number.ToSubject().Should.Equal(20);

    [Test]
    public void Indexer_ProviderNameOfItem() =>
        CreateSut(() => new[] { 10, 20, 30 })[1]
            .ProviderName.ToSubject().Should.Equal("[1]");

    [Test]
    public void ItemsAreInSameSequence() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Select(x => x.Number).Should.EqualSequence(10, 20, 30);

    [Test]
    public void ItemsHaveOrderedIndexes() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Select(x => x.Index).Should.EqualSequence(0, 1, 2);

    [Test]
    public void ItemsHaveSourceProviderName() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Select(x => x.SourceProviderName).Should.ContainExactly(3, ExpectedSourceProviderName);

    [Test]
    public void ItemsHaveProviderName() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Select(x => x.ProviderName).Should.EqualSequence("[0]", "[1]", "[2]");

    [Test]
    public void ItemsHaveSourceProviderName_AfterWhere() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Where(x => x.Number != 20)
            .Select(x => x.SourceProviderName).Should.AtOnce.ContainExactly(2, ExpectedSourceProviderName + ".Where(x => x.Number != 20)");

    [Test]
    public void ItemsHaveSourceProviderName_AfterMultipleWhere() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Where(x => x.Number != 20)
            .Where(x => x.Number < 30)
            .Select(x => x.SourceProviderName).Should.AtOnce.ContainExactly(1, ExpectedSourceProviderName + ".Where(x => x.Number != 20).Where(x => x.Number < 30)");

    [Test]
    public void ItemsHaveProviderName_AfterWhere() =>
        CreateSut(() => new[] { 10, 20, 30 })
            .Where(x => x.Number != 20)
            .Select(x => x.ProviderName).Should.EqualSequence("[0]", "[1]");

    private static EnumerableValueProvider<TestItem, TestOwner> CreateSut(Func<IEnumerable<int>> sourceValuesGetFunction) =>
        new TestOwner(sourceValuesGetFunction).Items;

    public class TestOwner : IObjectProvider<TestOwner>
    {
        private readonly Func<IEnumerable<int>> _sourceValuesGetFunction;

        public TestOwner(Func<IEnumerable<int>> sourceValuesGetFunction) =>
            _sourceValuesGetFunction = sourceValuesGetFunction;

        public TestOwner Object => this;

        public string ProviderName => nameof(TestOwner);

        public EnumerableValueProvider<TestItem, TestOwner> Items =>
            new(
                this,
                new DynamicObjectSource<IEnumerable<TestItem>, TestOwner>(
                    this,
                    x => _sourceValuesGetFunction.Invoke()
                        .Select((v, i) => new TestItem(v, i))),
                nameof(Items));
    }

    public class TestItem : IHasProviderName, IHasSourceProviderName
    {
        public TestItem(int number, int index)
        {
            Number = number;
            Index = index;
        }

        public int Number { get; }

        public int Index { get; }

        public string ProviderName { get; set; }

        public string SourceProviderName { get; set; }
    }
}
