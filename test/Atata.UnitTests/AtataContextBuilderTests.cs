namespace Atata.UnitTests;

public sealed class AtataContextBuilderTests
{
    [Test]
    [Parallelizable(ParallelScope.None)]
    public void WhenThereIsBaseConfiguration()
    {
        try
        {
            // Arrange
            var baseBuilder = AtataContext.BaseConfiguration;
            baseBuilder.UseVariable("a", 1);
            baseBuilder.EventSubscriptions.Add<AtataContextInitStartedEvent>(() => { });
            baseBuilder.LogConsumers.AddConsole();
            baseBuilder.Sessions.Add<FakeSessionBuilder>(x => x.Name = "session1");
            baseBuilder.BaseRetryTimeout = TimeSpan.FromSeconds(11);

            // Act
            var builder = AtataContext.CreateNonScopedBuilder();
            builder.UseVariable("b", 2);
            builder.EventSubscriptions.Add<AtataContextInitStartedEvent>(() => { });
            builder.LogConsumers.AddDebug();
            builder.Sessions.Add<FakeSessionBuilder>(x => x.Name = "session2");
            builder.Sessions.Add<FakeSessionBuilder>(x => x.Name = "session3");

            // Assert
            builder.Variables.Should().ContainKeys("a", "b");
            builder.EventSubscriptions.Items.Should().HaveCount(2);
            builder.LogConsumers.Items.Should().HaveCount(2);
            builder.Sessions.Providers.Should().HaveCount(3);
            builder.BaseRetryTimeout.Should().Be(TimeSpan.FromSeconds(11));
        }
        finally
        {
            AtataContext.BaseConfiguration.Clear();
        }
    }

    [Test]
    public void Clear()
    {
        // Arrange
        var builder = AtataContext.CreateDefaultBuilder(AtataContextScope.Test);
        builder.Variables.Add("test", "some");
        builder.EventSubscriptions.Add<AtataContextInitCompletedEvent>(() => { });
        builder.Sessions.Add<FakeSessionBuilder>();

        // Act
        var newBuilder = builder.Clear();

        // Assert
        newBuilder.Should().NotBe(builder);
        newBuilder.Scope.Should().Be(builder.Scope);
        newBuilder.Sessions.DefaultStartScopes.Should().Be(builder.Sessions.DefaultStartScopes);
        newBuilder.Variables.Should().BeEmpty();
        newBuilder.EventSubscriptions.Items.Should().BeEmpty();
        newBuilder.Sessions.Providers.Should().BeEmpty();
    }
}
