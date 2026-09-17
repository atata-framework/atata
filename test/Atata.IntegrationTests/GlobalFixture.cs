namespace Atata.IntegrationTests;

[SetUpFixture]
public sealed class GlobalFixture
{
    [OneTimeSetUp]
    public async Task GlobalSetUpAsync()
    {
        ThreadPool.SetMinThreads(Environment.ProcessorCount * 4, Environment.ProcessorCount);

        AtataContext.GlobalProperties.UseRootNamespaceOf<GlobalFixture>();
    }
}
