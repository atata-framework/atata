namespace Atata.IntegrationTests;

#pragma warning disable S6966 // Awaitable method should be used

public static class AtataSessionBuilderTests
{
    public sealed class Build
    {
        [Test]
        public async Task WithoutContext()
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
        public async Task WithinCurrentContext_ThenDisposeSession()
        {
            using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder().Build();
            AtataSession session = await new FakeSessionBuilder().BuildAsync();

            await using (session)
            {
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            context.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithinCurrentContext_ThenDisposeContext()
        {
            AtataContext context = AtataContext.CreateDefaultNonScopedBuilder().Build();
            AtataSession session = null;

            await using (context)
            {
                session = await new FakeSessionBuilder().BuildAsync();

                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            session.IsActive.Should().BeFalse();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithContext_ThenDisposeSession()
        {
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            AtataSession session = await new FakeSessionBuilder().BuildAsync(context);

            await using (session)
            {
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            context.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithContext_ThenDisposeContext()
        {
            AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            AtataSession session = null;

            await using (context)
            {
                session = await new FakeSessionBuilder().BuildAsync(context);

                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            session.IsActive.Should().BeFalse();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task AfterAddToContextSessions()
        {
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            var sessionBuidler = context.Sessions.Add<FakeSessionBuilder>();

            await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

            await using AtataSession session = await sessionBuidler.BuildAsync();

            session.Context.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
        }
    }

    public sealed class Start_WithBorrowMode
    {
        [Test]
        public async Task WithoutContext()
        {
            var builder = new FakeSessionBuilder()
                .UseStartMode(AtataSessionStartMode.Borrow);

            await Assert.ThatAsync(
                () => builder.StartAsync(),
                Throws.InvalidOperationException.With.Message.EqualTo(
                    "Cannot start session with StartMode value Borrow without AtataContext."));
        }

        [Test]
        public async Task WithinCurrentContext_WhichDoesNotHaveSession()
        {
            using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder().Build();
            var builder = new FakeSessionBuilder()
                .UseStartMode(AtataSessionStartMode.Borrow);

            await Assert.ThatAsync(
                () => builder.StartAsync(),
                Throws.TypeOf<AtataSessionNotFoundException>().With.Message.StartsWith(
                    "Failed to find FakeSession to borrow for AtataContext"));
        }

        [Test]
        public async Task WithinCurrentContext_ThenDisposeSession()
        {
            using AtataContext parentContext = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .Build();

            using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Build();

            AtataSession session = await new FakeSessionBuilder()
                .UseStartMode(AtataSessionStartMode.Borrow)
                .StartAsync();

            await using (session)
            {
                session.IsBorrowed.Should().BeTrue();
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().BeNull();
            context.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithinCurrentContext_ThenDisposeContext()
        {
            using AtataContext parentContext = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .Build();

            using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Build();

            AtataSession session = null;

            await using (context)
            {
                session = await new FakeSessionBuilder()
                    .UseStartMode(AtataSessionStartMode.Borrow)
                    .StartAsync();

                session.IsBorrowed.Should().BeTrue();
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WithinContextBuild_ThenReturnToOwnerContext()
        {
            await using AtataContext parentContext = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .Build();

            await using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Add<FakeSessionBuilder>(
                    x => x.UseStartMode(AtataSessionStartMode.Borrow))
                .Build();

            AtataSession session = context.Sessions.Get<FakeSession>();

            session.IsBorrowed.Should().BeTrue();
            session.Context.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.Should().BeEquivalentTo([session]);

            session.ReturnToOwnerContext();

            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }
    }
}

#pragma warning restore S6966 // Awaitable method should be used
