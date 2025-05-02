﻿namespace Atata.Testcontainers.IntegrationTests;

[Explicit("Requires Docker to be installed and running.")]
public sealed class ContainerSessionTests : AtataTestSuite
{
    [Test]
    public async Task ExtractFileToArtifactsAsync()
    {
        // Arrange
        await using var containerSession = await Context.Sessions.AddContainer()
            .Use(x => x.WithImage("hello-world:latest"))
            .BuildAsync();

        // Act
        FileSubject file = await containerSession.ExtractFileToArtifactsAsync("hello");

        // Assert
        file.Should.Exist()
            .Length.Should.BeGreater(5_000L);
    }
}
