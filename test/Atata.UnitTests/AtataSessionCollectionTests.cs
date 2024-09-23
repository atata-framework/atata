namespace Atata.UnitTests;

public sealed class AtataSessionCollectionTests
{
    private DisposableSubject<AtataSessionCollection> _sut;

    [SetUp]
    public void SetUp() =>
        _sut = new([]);

    [TearDown]
    public void TearUp() =>
        _sut?.Dispose();

    [Test]
    public void Get_WithExistingIndex()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });
        _sut.Object.Add(new FakeSession { Name = "B" });

        _sut.ResultOf(x => x.Get<FakeSession>(1))
            .ValueOf(x => x.Name).Should.Be("B");
    }

    [Test]
    public void Get_WithInvalidIndex()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });

        _sut.Invoking(x => x.Get<FakeSession>(2))
            .Should.Throw<AtataSessionNotFoundException>()
            .ValueOf(x => x.Message).Should.Be(
                "Failed to find session of type Atata.UnitTests.FakeSession with index 2 in AtataContext. There was 1 session of such type.");
    }

    [Test]
    public void Get_WithNegativeIndex() =>
        _sut.Invoking(x => x.Get<FakeSession>(-2))
            .Should.Throw<ArgumentOutOfRangeException>();
}
