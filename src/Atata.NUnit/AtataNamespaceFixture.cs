namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataNamespaceFixture
{
    protected AtataContext Context { get; private set; } = null!;

    [OneTimeSetUp]
    public void SetUpNamespaceAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Namespace)
            .UseDefaultCancellationToken(TestContext.CurrentContext.CancellationToken);

        ConfigureNamespaceAtataContext(builder);

        Context = builder.Build();
    }

    [OneTimeTearDown]
    public void TearDownNamespaceAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    protected virtual void ConfigureNamespaceAtataContext(AtataContextBuilder builder)
    {
    }
}
