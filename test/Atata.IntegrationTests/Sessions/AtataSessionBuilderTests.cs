namespace Atata.IntegrationTests.Sessions;

#pragma warning disable S6966 // Awaitable method should be used

public static class AtataSessionBuilderTests
{
    public sealed class StartScopes
    {
        [TestCase(AtataContextScope.Global, ExpectedResult = AtataContextScopes.Global)]
        [TestCase(AtataContextScope.Namespace, ExpectedResult = AtataContextScopes.Namespace)]
        [TestCase(AtataContextScope.TestSuiteGroup, ExpectedResult = AtataContextScopes.TestSuiteGroup)]
        [TestCase(AtataContextScope.TestSuite, ExpectedResult = AtataContextScopes.TestSuite)]
        [TestCase(AtataContextScope.Test, ExpectedResult = AtataContextScopes.Test)]
        public async Task<AtataContextScopes> AssociatesWithAtataContextScope(AtataContextScope contextScope)
        {
            // Arrange
            var builder = AtataContext.CreateDefaultBuilder(contextScope)
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataContextScopes.Global)
                    .UseVariable("scopes", AtataContextScopes.Global))
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataContextScopes.Namespace)
                    .UseVariable("scopes", AtataContextScopes.Namespace))
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataContextScopes.TestSuiteGroup)
                    .UseVariable("scopes", AtataContextScopes.TestSuiteGroup))
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataContextScopes.TestSuite)
                    .UseVariable("scopes", AtataContextScopes.TestSuite))
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataContextScopes.Test)
                    .UseVariable("scopes", AtataContextScopes.Test));

            // Act
            await using AtataContext context = await builder.BuildAsync();

            // Assert
            context.Sessions.Should().ContainSingle();
            return (AtataContextScopes)context.Sessions[0].Variables["scopes"]!;
        }
    }

    public sealed class StartCount
    {
        [Test]
        public async Task WithInvalidValue([Values(0, -1)] int count)
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .StartCount = count);

            // Act
            var call = contextBuilder.Invoking(x => x.BuildAsync());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionBuilderValidationException>()
                .WithMessage($"Start count {count} should be a positive value.");
        }

        [Test]
        public async Task WithValidValue([Values(1, 2, 5, 7)] int count)
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .StartCount = count);

            // Act
            using AtataContext context = await contextBuilder.BuildAsync();

            // Assert
            context.Sessions.Should().HaveCount(count);
        }

        [Test]
        public async Task With3_ThenBuild1More()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .StartCount = 3);

            // Act
            using AtataContext context = await contextBuilder.BuildAsync();
            await context.Sessions.BuildAsync<FakeSession>();

            // Assert
            context.Sessions.Should().HaveCount(4);
        }
    }

    public sealed class StartConditions
    {
        [Test]
        public async Task WithConditionThatReturnsFalse()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartCondition(_ => false));

            // Act
            using AtataContext context = await contextBuilder.BuildAsync();

            // Assert
            context.Sessions.Should().BeEmpty();
        }
    }

    public sealed class Build
    {
        [Test]
        public async Task WithoutContext()
        {
            AtataSession session = await new FakeSessionBuilder().BuildAsync();
            AtataContext? context = null;

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
            AtataSession? session = null;

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
        public async Task WithinContext_ThenDisposeSession()
        {
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            var session = context.Sessions.Get<FakeSession>();

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
        public async Task WithinContext_ThenDisposeContext()
        {
            AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            var session = context.Sessions.Get<FakeSession>();

            await using (context)
            {
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
}

#pragma warning restore S6966 // Awaitable method should be used
