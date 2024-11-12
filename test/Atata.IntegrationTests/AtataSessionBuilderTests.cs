namespace Atata.IntegrationTests;

public sealed class AtataSessionBuilderTests
{
    [Test]
    public async Task Build_WithoutContext()
    {
        AtataSession session = await new FakeSessionBuilder().BuildAsync();
        AtataContext context = null;

        await using (session)
        {
            context = session.Context;
            context.IsActive.Should().BeTrue();
            context.Test.Should().Be(new TestInfo(null));
        }

        session.Context.Should().BeNull();
        context.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task Build_WithinCurrentContext_ThenDisposeSession()
    {
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        AtataSession session = await new FakeSessionBuilder().BuildAsync();

        await using (session)
        {
            session.Context.Should().Be(context);
            context.Sessions.Should().Contain(session);
        }

        session.Context.Should().BeNull();
        context.IsActive.Should().BeTrue();
    }

    [Test]
    public async Task Build_WithinCurrentContext_ThenDisposeContext()
    {
        AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        AtataSession session = null;

        await using (context)
        {
            session = await new FakeSessionBuilder().BuildAsync();

            session.Context.Should().Be(context);
            context.Sessions.Should().Contain(session);
        }

        session.Context.Should().BeNull();
        session.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task Build_WithContext_ThenDisposeSession()
    {
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        AtataSession session = await new FakeSessionBuilder().BuildAsync(context);

        await using (session)
        {
            session.Context.Should().Be(context);
            context.Sessions.Should().Contain(session);
        }

        session.Context.Should().BeNull();
        context.IsActive.Should().BeTrue();
    }

    [Test]
    public async Task Build_WithContext_ThenDisposeContext()
    {
        AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        AtataSession session = null;

        await using (context)
        {
            session = await new FakeSessionBuilder().BuildAsync(context);

            session.Context.Should().Be(context);
            context.Sessions.Should().Contain(session);
        }

        session.Context.Should().BeNull();
        session.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task Build_AfterAddToContextSessions()
    {
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
        var sessionBuidler = context.Sessions.Add<FakeSessionBuilder>();

        await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

        await using AtataSession session = await sessionBuidler.BuildAsync();

        session.Context.Should().Be(context);
        context.Sessions.Should().Contain(session);
    }
}
