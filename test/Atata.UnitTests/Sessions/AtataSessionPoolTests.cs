namespace Atata.UnitTests.Sessions;

public sealed class AtataSessionPoolTests
{
    private const int PoolMaxCapacity = 5;

    private AtataContext _context = null!;

    private FakeSessionBuilder _sessionBuilder = null!;

    private AtataSessionPool _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _context = AtataContext.CreateDefaultNonScopedBuilder().Build();

        _sessionBuilder = new();
        ((IAtataSessionBuilder)_sessionBuilder).TargetContext = _context;
        _sut = new(_sessionBuilder, PoolMaxCapacity);
    }

    [TearDown]
    public void TearDown() =>
        _context?.Dispose();

    [Test]
    public async Task GetAsync_WhenEmpty()
    {
        // Act
        await using var session = await _sut.GetAsync();

        // Assert
        session.Should().NotBeNull();
    }

    [Test]
    public async Task GetAsync_WhenNotEmpty()
    {
        // Arrange
        await using var initialSession1 = await _sut.GetAsync();
        await using var initialSession2 = await _sut.GetAsync();
        await using var initialSession3 = await _sut.GetAsync();
        _sut.Return(initialSession1);
        _sut.Return(initialSession3);
        _sut.Return(initialSession2);

        // Act
        var session1 = await _sut.GetAsync();
        var session2 = await _sut.GetAsync();
        var session3 = await _sut.GetAsync();

        // Assert
        session1.Should().Be(initialSession1);
        session2.Should().Be(initialSession3);
        session3.Should().Be(initialSession2);
    }

    [Test]
    public async Task GetAsync_WhenFilledCompletely()
    {
        // Arrange
        await _sut.FillAsync(PoolMaxCapacity);

        // Act
        var session = await _sut.GetAsync();

        // Assert
        session.Should().NotBeNull();
    }

    [Test]
    public async Task GetAsync_WhenFull_WaitedAndGot()
    {
        // Arrange
        await using var initialSession1 = await _sut.GetAsync();
        await using var initialSession2 = await _sut.GetAsync();
        await using var initialSession3 = await _sut.GetAsync();
        await using var initialSession4 = await _sut.GetAsync();
        await using var initialSession5 = await _sut.GetAsync();

        // Act
        var sessionTask = _sut.GetAsync();

        await Task.Delay(10);
        sessionTask.IsCompleted.Should().BeFalse();

        _sut.Return(initialSession3);

        var session = await sessionTask;

        // Assert
        session.Should().Be(initialSession3);
    }

    [Test]
    public async Task GetAsync_WhenFull_Cancels()
    {
        // Arrange
        await using var initialSession1 = await _sut.GetAsync();
        await using var initialSession2 = await _sut.GetAsync();
        await using var initialSession3 = await _sut.GetAsync();
        await using var initialSession4 = await _sut.GetAsync();
        await using var initialSession5 = await _sut.GetAsync();

        using CancellationTokenSource cancellationTokenSource = new(10);

        // Act
        Func<Task<AtataSession>> action = async () => await _sut.GetAsync(cancellationTokenSource.Token);

        // Assert
        await action.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    public async Task GetAsync_WhenFull_TimedOut()
    {
        // Arrange
        _sessionBuilder.SessionWaitingTimeout = TimeSpan.FromMilliseconds(10);
        _sessionBuilder.SessionWaitingRetryInterval = TimeSpan.FromMilliseconds(1);

        await using var initialSession1 = await _sut.GetAsync();
        await using var initialSession2 = await _sut.GetAsync();
        await using var initialSession3 = await _sut.GetAsync();
        await using var initialSession4 = await _sut.GetAsync();
        await using var initialSession5 = await _sut.GetAsync();

        // Act
        Func<Task<AtataSession>> action = async () => await _sut.GetAsync();

        // Assert
        await action.Should().ThrowExactlyAsync<TimeoutException>()
            .WithMessage("*waiting for FakeSession from a session pool.");
    }

    [Test]
    public async Task Return()
    {
        // Arrange
        await using var session = await _sut.GetAsync();

        // Act
        _sut.Return(session);

        // Assert
        _sut.QueuedCount.Should().Be(1);
    }

    [Test]
    public void Return_WithNull()
    {
        // Act
        Action action = () => _sut.Return(null!);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public async Task FillAsync_WithZero()
    {
        // Act
        var call = _sut.Invoking(x => x.FillAsync(0));

        // Assert
        await call.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task FillAsync_WithValueGreaterThanMaxCapacity()
    {
        // Act
        var call = _sut.Invoking(x => x.FillAsync(PoolMaxCapacity + 1));

        // Assert
        await call.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task QueuedCount_AfterGetAsync()
    {
        // Arrange
        await using var session = await _sut.GetAsync();

        // Act // Assert
        _sut.QueuedCount.Should().Be(0);
    }
}
