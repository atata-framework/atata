namespace Atata.Testcontainers.IntegrationTests;

[Explicit("Requires Docker to be installed and running.")]
public sealed class ContainerSessionBuilderTests : AtataTestSuite
{
    [Test]
    public async Task NonGenericContainerStarts_AddToAtataSessionCollection()
    {
        // Act
        var containerSession = await Context.Sessions.AddContainer()
            .Use(x => x.WithImage("hello-world:latest"))
            .BuildAsync();

        // Assert
        containerSession.Container.ToResultSubject()
            .Should.Not.BeNull()
            .ValueOf(x => x.Name).Should.Not.BeNullOrEmpty();
    }

    [Test]
    public async Task NonGenericContainerStarts_AddToAtataSessionsBuilder()
    {
        // Act
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.AddContainer(x => x
                .Use(x => x.WithImage("hello-world:latest")))
            .BuildAsync();
        var containerSession = context.Sessions.Get<ContainerSession>();

        // Assert
        containerSession.Container.ToResultSubject()
            .Should.Not.BeNull()
            .ValueOf(x => x.Name).Should.Not.BeNullOrEmpty();
    }

    [Test]
    public async Task GenericContainerStarts_AddToAtataSessionCollection()
    {
        // Act
        var containerSession = await Context.Sessions.AddContainer<WebDriverContainer>()
            .Use(() => new WebDriverBuilder()
                .WithBrowser(new WebDriverBrowser("selenium/standalone-chrome:latest")))
            .BuildAsync();

        // Assert
        containerSession.Container.ToResultSubject()
            .Should.Not.BeNull()
            .ValueOf(x => x.Name).Should.Not.BeNullOrEmpty();
    }

    [Test]
    public async Task GenericContainerStarts_AddToAtataSessionsBuilder()
    {
        // Act
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.AddContainer<WebDriverContainer>(x => x
                .Use(() => new WebDriverBuilder()
                    .WithBrowser(new WebDriverBrowser("selenium/standalone-chrome:latest"))))
            .BuildAsync();
        var containerSession = context.Sessions.Get<ContainerSession<WebDriverContainer>>();

        // Assert
        containerSession.Container.ToResultSubject()
            .Should.Not.BeNull()
            .ValueOf(x => x.Name).Should.Not.BeNullOrEmpty();
    }

    [Test]
    public async Task WithUseLogsSaveConfiguration_StdoutFileIncludedIsTrue()
    {
        // Act
        var containerSession = await Context.Sessions.AddContainer()
            .Use(x => x.WithImage("hello-world:latest"))
            .UseLogsSaveConfiguration(x => x.StdoutFileIncluded = true)
            .BuildAsync();

        await containerSession.DisposeAsync();

        // Assert
        Context.Artifacts.Files.Should.ContainSingle()
            .Files[0].Name.Should.Be("hello-world_latest-stdout.log");
    }

    [Test]
    public async Task WithUseLogsSaveConfiguration_StdoutFileIncludedIsFalse()
    {
        // Act
        var containerSession = await Context.Sessions.AddContainer()
            .Use(x => x.WithImage("hello-world:latest"))
            .UseLogsSaveConfiguration(x => x.StdoutFileIncluded = false)
            .BuildAsync();

        await containerSession.DisposeAsync();

        // Assert
        Context.Artifacts.Should.Not.Exist();
    }
}
