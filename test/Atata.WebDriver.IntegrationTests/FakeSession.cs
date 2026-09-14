namespace Atata.WebDriver.IntegrationTests;

public sealed class FakeSession : AtataSession
{
    protected override Task StartAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public static FakeSessionBuilder CreateBuilder() => new();
}
