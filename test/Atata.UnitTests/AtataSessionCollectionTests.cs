namespace Atata.UnitTests;

public sealed class AtataSessionCollectionTests
{
    private AtataContext _context = null!;

    private DisposableSubject<AtataSessionCollection> _sut = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        _context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        _sut = new AtataSessionCollection(_context).ToSutDisposableSubject();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _sut?.Dispose();

        if (_context is not null)
            await _context.DisposeAsync();
    }

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
            .Should.ThrowExactly<AtataSessionNotFoundException>(
                "Failed to find FakeSession with index 2 in AtataContext { * }. There was 1 session of such type.");
    }

    [Test]
    public void Get_ByIndex_WithNegativeIndex() =>
        _sut.Invoking(x => x.Get<FakeSession>(-2))
            .Should.ThrowExactly<ArgumentOutOfRangeException>();

    [Test]
    public void Get_WhenExists()
    {
        var sessionA = new FakeSession { Name = "A" };
        var sessionB = new FakeSession { Name = "B" };

        _sut.Object.Add(sessionA);
        _sut.Object.Add(sessionB);

        _sut.ResultOf(x => x.Get<FakeSession>())
            .ValueOf(x => x.Id).Should.Be(sessionB.Id);
    }

    [Test]
    public void Get_WhenDoesNotExist()
    {
        var sessionA = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA);

        _sut.Invoking(x => x.Get<FakeSession2>())
            .Should.ThrowExactly<AtataSessionNotFoundException>(
                "Failed to find FakeSession2 in AtataContext { * }.");
    }

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
            .ValueOf(x => x.Id).Should.Be(sessionA2.Id);
    }

    [Test]
    public void Get_ByName_WhenItIsMissing()
    {
        _sut.Object.Add(new FakeSession { Name = "A" });

        _sut.Invoking(x => x.Get<FakeSession>("B"))
            .Should.ThrowExactly<AtataSessionNotFoundException>(
                "Failed to find FakeSession { Name=B } in AtataContext { * }. There was 1 session of such type, but none with such name.");
    }

    [Test]
    public void Get_ByName_WithNull_WhenItIsMissing() =>
        _sut.Invoking(x => x.Get<FakeSession>(null))
            .Should.ThrowExactly<AtataSessionNotFoundException>(
                "Failed to find FakeSession in AtataContext { * }. There were 0 sessions of such type.");

    [Test]
    public void Contains_WhenExists()
    {
        var sessionA = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA);

        _sut.ResultOf(x => x.Contains<FakeSession>())
            .Should.BeTrue();
    }

    [Test]
    public void Contains_WhenDoesNotExist()
    {
        var sessionA = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA);

        _sut.ResultOf(x => x.Contains<FakeSession2>())
            .Should.BeFalse();
    }

    [Test]
    public void Contains_ByName_WhenExists()
    {
        var sessionA = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA);

        _sut.ResultOf(x => x.Contains<FakeSession>("A"))
            .Should.BeTrue();
    }

    [Test]
    public void Contains_ByName_WhenDoesNotExist()
    {
        var sessionA = new FakeSession { Name = "A" };

        _sut.Object.Add(sessionA);

        _sut.ResultOf(x => x.Contains<FakeSession2>("A"))
            .Should.BeFalse();
    }

    [Test]
    public void ConfigureBuilder_WhenDoesNotExist()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.Invoking(x => x.ConfigureBuilder<FakeSessionBuilder>(null))
            .Should.ThrowExactly<AtataSessionBuilderNotFoundException>(
                "Failed to find FakeSessionBuilder in AtataContext { * }.");
    }

    [Test]
    public void ConfigureBuilder_WithName_WhenDoesNotExist()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.Invoking(x => x.ConfigureBuilder<FakeSessionBuilder>("B", null))
            .Should.ThrowExactly<AtataSessionBuilderNotFoundException>(
                "Failed to find FakeSessionBuilder { Name=B } in AtataContext { * }.");
    }

    [Test]
    public void ConfigureBuilder_WhenExists()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>());

        _sut.ResultOf(x => x.ConfigureBuilder<FakeSessionBuilder>(null))
            .Should.Not.BeNull();
        _sut.ValueOf(x => x.Builders)
            .Should.ContainSingle();
    }

    [Test]
    public void ConfigureBuilder_WithName_WhenExists()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.ResultOf(x => x.ConfigureBuilder<FakeSessionBuilder>("A", null))
            .Should.Not.BeNull();
        _sut.ValueOf(x => x.Builders)
            .Should.ContainSingle();
    }

    [Test]
    public void ConfigureBuilderCopy_WhenDoesNotExist()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.Invoking(x => x.ConfigureBuilderCopy<FakeSessionBuilder>(null))
            .Should.ThrowExactly<AtataSessionBuilderNotFoundException>(
                "Failed to find FakeSessionBuilder in AtataContext { * }.");
    }

    [Test]
    public void ConfigureBuilderCopy_WithName_WhenDoesNotExist()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.Invoking(x => x.ConfigureBuilderCopy<FakeSessionBuilder>("B", null))
            .Should.ThrowExactly<AtataSessionBuilderNotFoundException>(
                "Failed to find FakeSessionBuilder { Name=B } in AtataContext { * }.");
    }

    [Test]
    public void ConfigureBuilderCopy_WhenExists()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>());

        _sut.ResultOf(x => x.ConfigureBuilderCopy<FakeSessionBuilder>(null))
            .Should.Not.BeNull();
        _sut.ValueOf(x => x.Builders)
            .Should.ContainExactly(2, x => x is FakeSessionBuilder && x.Name == null);
    }

    [Test]
    public void ConfigureBuilderCopy_WithName_WhenExists()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.ResultOf(x => x.ConfigureBuilderCopy<FakeSessionBuilder>("A", null))
            .Should.Not.BeNull();
        _sut.ValueOf(x => x.Builders)
            .Should.ContainExactly(2, x => x is FakeSessionBuilder && x.Name == "A");
    }

    [Test]
    public void ConfigureBuilderCopy_WithName_WhenExists_ChangeName()
    {
        _sut.Arrange(x => x
            .Add<FakeSessionBuilder>(x => x.Name = "A"));

        _sut.ResultOf(x => x.ConfigureBuilderCopy<FakeSessionBuilder>("A", x => x.UseName("B")))
            .Should.Not.BeNull();
        _sut.ValueOf(x => x.Builders)
            .Should.ConsistSequentiallyOf(
                x => x is FakeSessionBuilder && x.Name == "A",
                x => x is FakeSessionBuilder && x.Name == "B");
    }
}
