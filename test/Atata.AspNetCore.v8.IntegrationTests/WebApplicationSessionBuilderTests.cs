namespace Atata.AspNetCore.IntegrationTests;

public class WebApplicationSessionBuilderTests : AtataTestSuite
{
    protected static async Task TestSessionAsync(WebApplicationSession session)
    {
        var client = session.CreateClient();
        var response = await client.GetAsync("/ping");

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.ToResultSubject()
            .Should.Be("pong");
    }

    protected async Task<Subject<WebApplicationSession>> BuildWebApplicationSessionAsSubjectAsync(Action<WebApplicationSessionBuilder> configure)
    {
        var builder = Context.Sessions.AddWebApplication()
            .Use<Program>();

        configure.Invoke(builder);

        var session = await builder.BuildAsync();
        return session.ToSubject(nameof(session));
    }

    public sealed class AddToAtataSessionsBuilder : WebApplicationSessionBuilderTests
    {
        [Test]
        public async Task StandardWebApplicationSession_UseProgram()
        {
            // Act
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.AddWebApplication(x => x
                    .Use<Program>())
                .BuildAsync();
            var session = context.Sessions.Get<WebApplicationSession>();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task StandardWebApplicationSession_UseWebApplicationFactoryAsFunc()
        {
            // Act
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.AddWebApplication(x => x
                    .Use(() => new WebApplicationFactory<Program>()))
                .BuildAsync();
            var session = context.Sessions.Get<WebApplicationSession>();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task StandardWebApplicationSession_UseCustomWebApplicationFactory()
        {
            // Act
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.AddWebApplication(x => x
                    .Use(new CustomWebApplicationFactory()))
                .BuildAsync();
            var session = context.Sessions.Get<WebApplicationSession>();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task CustomWebApplicationSession_UseProgram()
        {
            // Act
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.AddWebApplication<CustomWebApplicationSession>(x => x
                    .Use<Program>())
                .BuildAsync();
            var session = context.Sessions.Get<CustomWebApplicationSession>();

            // Assert
            session.ToResultSubject()
                .ValueOf(x => x.CallsCountOfConfigureWebHost).Should.Be(1);
            await TestSessionAsync(session);
        }
    }

    public sealed class AddToAtataSessionCollection : WebApplicationSessionBuilderTests
    {
        [Test]
        public async Task StandardWebApplicationSession_UseProgram()
        {
            // Act
            var session = await Context.Sessions.AddWebApplication()
                .Use<Program>()
                .BuildAsync();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task StandardWebApplicationSession_UseWebApplicationFactoryAsFunc()
        {
            // Act
            var session = await Context.Sessions.AddWebApplication()
                .Use(() => new WebApplicationFactory<Program>())
                .BuildAsync();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task StandardWebApplicationSession_UseCustomWebApplicationFactory()
        {
            // Act
            var session = await Context.Sessions.AddWebApplication()
                .Use(new CustomWebApplicationFactory())
                .BuildAsync();

            // Assert
            await TestSessionAsync(session);
        }

        [Test]
        public async Task CustomWebApplicationSession_UseProgram()
        {
            // Act
            var session = await Context.Sessions.AddWebApplication<CustomWebApplicationSession>()
                .Use<Program>()
                .BuildAsync();

            // Assert
            session.ToResultSubject()
                .ValueOf(x => x.CallsCountOfConfigureWebHost).Should.Be(1);
            await TestSessionAsync(session);
        }
    }

    public sealed class UseCollectApplicationLogs : WebApplicationSessionBuilderTests
    {
        [Test]
        public async Task WithTrue()
        {
            // Act
            var sut = await BuildWebApplicationSessionAsSubjectAsync(x => x
                .UseCollectApplicationLogs(true));

            // Assert
            await TestSessionAsync(sut);
            sut.SubjectOf(x => x.FakeLogCollector)
                .Should.Not.BeNull()
                .ResultOf(x => x.GetSnapshot(false)).Select(x => x.Message).Should.EqualSequence(
                    "information message",
                    "warning message",
                    "error message",
                    "critical message");
        }

        [Test]
        public async Task WithFalse()
        {
            // Act
            var sut = await BuildWebApplicationSessionAsSubjectAsync(x => x
                .UseCollectApplicationLogs(false));

            // Assert
            await TestSessionAsync(sut);
            sut.ValueOf(x => x.FakeLogCollector).Should.BeNull();
        }
    }

    public sealed class UseDefaultApplicationLogLevel : WebApplicationSessionBuilderTests
    {
        [Test]
        public async Task WithDebug()
        {
            // Act
            var sut = await BuildWebApplicationSessionAsSubjectAsync(x => x
                .UseDefaultApplicationLogLevel(MSLogLevel.Debug));

            // Assert
            await TestSessionAsync(sut);
            sut.SubjectOf(x => x.FakeLogCollector)
                .Should.Not.BeNull()
                .ResultOf(x => x.GetSnapshot(false)).Select(x => x.Message).Should.EndWith(
                    "debug message",
                    "information message",
                    "warning message",
                    "error message",
                    "critical message");
        }

        [Test]
        public async Task WithWarning()
        {
            // Act
            var sut = await BuildWebApplicationSessionAsSubjectAsync(x => x
                .UseDefaultApplicationLogLevel(MSLogLevel.Warning));

            // Assert
            await TestSessionAsync(sut);
            sut.SubjectOf(x => x.FakeLogCollector)
                .Should.Not.BeNull()
                .ResultOf(x => x.GetSnapshot(false)).Select(x => x.Message).Should.EqualSequence(
                    "warning message",
                    "error message",
                    "critical message");
        }
    }

    public sealed class UseMinimumApplicationLogLevel : WebApplicationSessionBuilderTests
    {
        [Test]
        public async Task WithWarning()
        {
            // Act
            var sut = await BuildWebApplicationSessionAsSubjectAsync(x => x
                .UseConfiguration(x => x.UseEnvironment("Test"))
                .UseMinimumApplicationLogLevel(MSLogLevel.Warning));

            // Assert
            await TestSessionAsync(sut);
            sut.SubjectOf(x => x.FakeLogCollector)
                .Should.Not.BeNull()
                .ResultOf(x => x.GetSnapshot(false)).Select(x => x.Message).Should.EqualSequence(
                    "warning message",
                    "error message",
                    "critical message");
        }
    }
}
