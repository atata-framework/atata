namespace Atata.UnitTests;

internal abstract class TestCaseDataSource : IEnumerable
{
    private readonly List<TestCaseData> _items = [];

    protected void Add(params object?[] arguments) =>
        _items.Add(new TestCaseData(arguments));

    public IEnumerator GetEnumerator() =>
        _items.GetEnumerator();
}
