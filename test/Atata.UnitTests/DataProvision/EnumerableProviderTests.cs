﻿namespace Atata.UnitTests.DataProvision;

public sealed class EnumerableProviderTests
{
    private const string ExpectedSourceProviderName = nameof(TestOwner) + "." + nameof(TestOwner.Items);

    [Test]
    public void ProviderName() =>
        CreateSut(() => [])
            .ProviderName.ToSubject().Should.Be(ExpectedSourceProviderName);

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
            : [1, 2]);

        subject.Should.Not.BeEmpty();

        tryCount.ToSubject(nameof(tryCount)).Should.Be(4);
    }

    [Test]
    public void Indexer() =>
        CreateSut(() => [10, 20, 30])[1]
            .Number.ToSubject().Should.Be(20);

    [Test]
    public void Indexer_ProviderNameOfItem() =>
        CreateSut(() => [10, 20, 30])[1]
            .ProviderName.ToSubject().Should.Be("[1]");

    [Test]
    public void ItemsAreInSameSequence() =>
        CreateSut(() => [10, 20, 30])
            .Select(x => x.Number).Should.EqualSequence(10, 20, 30);

    [Test]
    public void ItemsHaveOrderedIndexes() =>
        CreateSut(() => [10, 20, 30])
            .Select(x => x.Index).Should.EqualSequence(0, 1, 2);

    [Test]
    public void ItemsHaveSourceProviderName() =>
        CreateSut(() => [10, 20, 30])
            .Select(x => x.SourceProviderName).Should.ContainExactly(3, ExpectedSourceProviderName);

    [Test]
    public void ItemsHaveProviderName() =>
        CreateSut(() => [10, 20, 30])
            .Select(x => x.ProviderName).Should.EqualSequence("[0]", "[1]", "[2]");

    [Test]
    public void ItemsHaveSourceProviderName_AfterWhere() =>
        CreateSut(() => [10, 20, 30])
            .Where(x => x.Number != 20)
            .Select(x => x.SourceProviderName).Should.AtOnce.ContainExactly(2, ExpectedSourceProviderName + ".Where(x => x.Number != 20)");

    [Test]
    public void ItemsHaveSourceProviderName_AfterMultipleWhere() =>
        CreateSut(() => [10, 20, 30])
            .Where(x => x.Number != 20)
            .Where(x => x.Number < 30)
            .Select(x => x.SourceProviderName).Should.AtOnce.ContainExactly(1, ExpectedSourceProviderName + ".Where(x => x.Number != 20).Where(x => x.Number < 30)");

    [Test]
    public void ItemsHaveProviderName_AfterWhere() =>
        CreateSut(() => [10, 20, 30])
            .Where(x => x.Number != 20)
            .Select(x => x.ProviderName).Should.EqualSequence("[0]", "[1]");

    private static EnumerableValueProvider<TestItem, TestOwner> CreateSut(Func<IEnumerable<int>> sourceValuesGetFunction) =>
        new TestOwner(sourceValuesGetFunction).Items;

    public sealed class TestOwner : IObjectProvider<TestOwner>
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
                    _ => _sourceValuesGetFunction.Invoke()
                        .Select((v, i) => new TestItem(v, i))),
                nameof(Items));

        public IAtataExecutionUnit? ExecutionUnit => null;
    }

    public sealed class TestItem : IHasProviderName, IHasSourceProviderName
    {
        public TestItem(int number, int index)
        {
            Number = number;
            Index = index;
        }

        public int Number { get; }

        public int Index { get; }

        public string ProviderName { get; set; } = string.Empty;

        public string? SourceProviderName { get; set; }
    }
}
