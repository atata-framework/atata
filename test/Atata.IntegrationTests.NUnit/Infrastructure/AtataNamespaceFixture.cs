using NUnit.Framework;

namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataNamespaceFixture
{
    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public void SetUpNamespaceAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.NamespaceSuite);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownNamespaceAtataContext() =>
        TestCompletionHandler.CompleteTest(Context);

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
