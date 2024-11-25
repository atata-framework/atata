namespace Atata.UnitTests;

public sealed class AtataSessionCollectionTests
{
    private DisposableSubject<AtataSessionCollection> _sut;

    [SetUp]
    public void SetUp() =>
        _sut = new(new AtataSessionCollection(null));

    [TearDown]
    public void TearUp() =>
        _sut?.Dispose();

    [Test]
    public void Get_ByIndex_WithExistingIndex()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });
        _sut.Object.Add(new FakeSession { Name = "B" });

        _sut.ResultOf(x => x.Get<FakeSession>(1))
            .ValueOf(x => x.Name).Should.Be("B");
    }

    [Test]
    public void Get_ByIndex_WithInvalidIndex()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });

        _sut.Invoking(x => x.Get<FakeSession>(2))
            .Should.Throw<AtataSessionNotFoundException>()
            .ValueOf(x => x.Message).Should.Be(
                "Failed to find session of type Atata.UnitTests.FakeSession with index 2 in AtataContext. There was 1 session of such type.");
    }

    [Test]
    public void Get_ByIndex_WithNegativeIndex() =>
        _sut.Invoking(x => x.Get<FakeSession>(-2))
            .Should.Throw<ArgumentOutOfRangeException>();

    [Test]
    public void Get_ByName_WithExistingName()
    {
        var sessionA = new FakeSession { Name = "A" };
        var sessionB = new FakeSession { Name = "B" };

        _sut.Object.Add(sessionA);
        _sut.Object.Add(sessionB);

        _sut.ResultOf(x => x.Get<FakeSession>("B"))
            .ValueOf(x => x.Id).Should.Be(sessionB.Id);
    }

    [Test]
    public void Get_ByName_WhenThereAreDuplicates()
    {
        var sessionA1 = new FakeSession { Name = "A" };
        var sessionA2 = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA1);
        _sut.Object.Add(sessionA2);

        _sut.ResultOf(x => x.Get<FakeSession>("A"))
            .ValueOf(x => x.Id).Should.Be(sessionA1.Id);
    }

    [Test]
    public void Get_ByName_WhenItIsMissing()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });

        _sut.Invoking(x => x.Get<FakeSession>("B"))
            .Should.Throw<AtataSessionNotFoundException>()
            .ValueOf(x => x.Message).Should.Be(
                "Failed to find session of type Atata.UnitTests.FakeSession with name \"B\" in AtataContext. There was 1 session of such type, but none with such name.");
    }

    [Test]
    public void Get_ByName_WithNull_WhenItIsMissing() =>
        _sut.Invoking(x => x.Get<FakeSession>(null))
            .Should.Throw<AtataSessionNotFoundException>()
            .ValueOf(x => x.Message).Should.Be(
                "Failed to find session of type Atata.UnitTests.FakeSession with name \"\" in AtataContext. There were 0 sessions of such type.");
}
