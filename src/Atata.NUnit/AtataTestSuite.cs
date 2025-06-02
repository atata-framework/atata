namespace Atata.NUnit;

/// <summary>
/// Represents a base class for test suites/classes using Atata with NUnit.
/// Provides setup and tear-down logic for Atata contexts at the suite and test levels.
/// </summary>
public abstract class AtataTestSuite
{
    private readonly ConcurrentDictionary<string, AtataContext> _testIdContextMap = [];

    private TestSuiteAtataContextMetadata? _testSuiteContextMetadata;

    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance for the test suite.
    /// </summary>
    protected AtataContext SuiteContext { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance for the current test.
    /// </summary>
    protected AtataContext Context =>
        _testIdContextMap.TryGetValue(TestExecutionContext.CurrentContext.CurrentTest.Id, out var context)
            ? context
            : null!;

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the test suite.
    /// The method is executed once before any tests in the suite are run.
    /// </summary>
    [OneTimeSetUp]
    public void SetUpSuiteAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .UseDefaultCancellationToken(TestExecutionContext.CurrentContext.CancellationToken);

        _testSuiteContextMetadata = TestSuiteAtataContextMetadata.GetForType(GetType());
        _testSuiteContextMetadata.ApplyToTestSuiteBuilder(builder);

        ConfigureSuiteAtataContext(builder);

        SuiteContext = builder.Build();
    }

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the test suite.
    /// The method is executed once after all tests in the suite have run.
    /// </summary>
    [OneTimeTearDown]
    public void TearDownSuiteAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(SuiteContext);

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the current test.
    /// The method is executed before each test in the suite.
    /// </summary>
    [SetUp]
    public void SetUpTestAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseDefaultCancellationToken(TestExecutionContext.CurrentContext.CancellationToken);

        _testSuiteContextMetadata?.ApplyToTestBuilder(builder);

        MethodInfo? method = TestContext.CurrentContext.Test.Method?.MethodInfo;

        if (method is not null)
            TestAtataContextMetadata.GetForMethod(method).ApplyToTestBuilder(builder);

        ConfigureTestAtataContext(builder);

        AtataContext context = builder.Build();

        var testId = TestExecutionContext.CurrentContext.CurrentTest.Id;
        _testIdContextMap[testId] = context;
    }

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the current test.
    /// The method is executed after each test in the suite.
    /// </summary>
    [TearDown]
    public void TearDownTestAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    /// <summary>
    /// Configures the test suite <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }

    /// <summary>
    /// Configures the test <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }
}
