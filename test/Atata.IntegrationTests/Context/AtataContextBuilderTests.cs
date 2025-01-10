namespace Atata.IntegrationTests.Context;

public sealed class AtataContextBuilderTests : TestSuiteBase
{
    [Test]
    public void UseParentContext()
    {
        // Arrange
        using var parentContext = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .Build();

        // Act
        using var childContext1 = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseParentContext(parentContext)
            .Build();

        using var childContext2 = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseParentContext(parentContext)
            .Build();

        // Assert
        parentContext.AggregateAssert(() =>
        {
            parentContext.ToSubject().ValueOf(x => x.ChildContexts).Should.EqualSequence(childContext1, childContext2);
            childContext1.ToSubject().ValueOf(x => x.ParentContext).Should.Be(parentContext);
            childContext2.ToSubject().ValueOf(x => x.ParentContext).Should.Be(parentContext);
        });
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

    [Test]
    public void WhenThrowsOnBuild()
    {
        // Arrange
        var builder = ConfigureSessionlessAtataContext();
        builder.EventSubscriptions.Add<AtataContextInitCompletedEvent>(
            () => throw new InvalidOperationException("some"));

        // Act
        var act = builder.Invoking(x => x.Build());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("some");

        CurrentLog.GetSnapshotOfLevel(LogLevel.Error).
            Should().ContainSingle();
        CurrentLog.LatestRecord.Level.Should().Be(LogLevel.Debug);
        CurrentLog.LatestRecord.Message.Should().StartWith("Finished");
    }
}
