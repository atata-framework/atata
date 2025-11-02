namespace Atata.UnitTests.Sessions;

public sealed class AtataSessionEventHandlerTests
{
    [Test]
    public void WhenPool_OnAtataSessionInitCompletedEvent_SessionSetAsCurrent()
    {
        // Arrange
        var builder = AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.Add<FakeSessionBuilder>(x => x
                .UseAsPool(x => x.WithInitialCapacity(1))
                .EventSubscriptions.Add<AtataSessionInitCompletedEvent>(
                    e => e.Session.SetAsCurrent()));

        // Act & Assert
        AtataContext? context = null;

        Assert.DoesNotThrow(() =>
            context = builder.Build());

        context?.Dispose();
    }

    [Test]
    public void WhenPool_OnAtataSessionInitCompletedEvent_ThrowException()
    {
        // Arrange
        AtataSession? session = null;

        var builder = AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.Add<FakeSessionBuilder>(x => x
                .UseAsPool(x => x.WithInitialCapacity(1))
                .EventSubscriptions.Add<AtataSessionInitCompletedEvent>(
                    eventData =>
                    {
                        session = eventData.Session;
                        throw new InvalidOperationException("Intentional exception.");
                    }));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            builder.Build());

        session.Should().NotBeNull();
        session.IsActive.Should().BeFalse();
    }
}
