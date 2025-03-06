namespace Atata.NUnit;

[Parallelizable(ParallelScope.Self)]
public abstract class AtataTestSuite
{
    private TestSuiteAtataContextMetadata? _testSuiteContextMetadata;

    protected AtataContext SuiteContext { get; private set; } = null!;

    protected AtataContext Context { get; private set; } = null!;

    [OneTimeSetUp]
    public void SetUpSuiteAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite);

        _testSuiteContextMetadata = TestSuiteAtataContextMetadata.GetForType(GetType());
        _testSuiteContextMetadata.ApplyToTestSuiteBuilder(builder);

        ConfigureSuiteAtataContext(builder);

        SuiteContext = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownSuiteAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(SuiteContext);

    [SetUp]
    public void SetUpTestAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test);

        _testSuiteContextMetadata?.ApplyToTestBuilder(builder);

        MethodInfo? method = TestContext.CurrentContext.Test.Method?.MethodInfo;

        if (method is not null)
            TestAtataContextMetadata.GetForMethod(method).ApplyToTestBuilder(builder);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [TearDown]
    public void TearDownTestAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
