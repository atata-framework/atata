namespace Atata.UnitTests.Sessions;

public sealed class AtataSessionEventHandlerTests
{
    [Test]
    public void OnAtataSessionInitCompletedEvent_SessionSetAsCurrent()
    {
        // Arrange
        var builder = AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.Add<FakeSessionBuilder>(x => x
                .UseAsPool(x => x.WithInitialCapacity(1))
                .EventSubscriptions.Add<AtataSessionInitCompletedEvent>(e => e.Session.SetAsCurrent()));

        // Act & Assert
        AtataContext? context = null;

        Assert.DoesNotThrow(() =>
            context = builder.Build());

        context?.Dispose();
    }
}
