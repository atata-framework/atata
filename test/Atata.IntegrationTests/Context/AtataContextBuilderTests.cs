namespace Atata.IntegrationTests.Context;

public sealed class AtataContextBuilderTests
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
}
