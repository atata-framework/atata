using NUnit.Framework;

namespace Atata.NUnit;

[Parallelizable(ParallelScope.Self)]
public abstract class AtataTestSuite
{
    protected AtataContext SuiteContext { get; private set; }

    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public void SetUpSuiteAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite);

        ConfigureSuiteAtataContext(builder);

        SuiteContext = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownSuiteAtataContext() =>
        SuiteContext?.Dispose();

    [SetUp]
    public void SetUpTestAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [TearDown]
    public void TearDownTestAtataContext() =>
        TestCompletionHandler.CompleteTest(Context);

    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
