namespace Atata.IntegrationTests;

public sealed class FakeSession : AtataSession
{
    protected internal override Task StartAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
