namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataNamespaceFixture
{
    protected AtataContext Context { get; private set; } = null!;

    [OneTimeSetUp]
    public void SetUpNamespaceAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Namespace);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownNamespaceAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
