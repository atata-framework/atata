namespace Atata.MSTest;

/// <summary>
/// Represents a base class for test suites/classes using Atata with MSTest.
/// Provides setup and tear-down logic for Atata contexts at the suite and test levels.
/// </summary>
public abstract class AtataTestSuite
{
    private static readonly ConcurrentDictionary<string, TestSuiteData> s_testSuiteDataByTypeName = [];

    /// <summary>
    /// Gets or sets the MSTest test context.
    /// The value is set automatically by MSTest framework.
    /// </summary>
    public TestContext TestContext { get; set; } = null!;

    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance for the current test.
    /// </summary>
    protected AtataContext Context { get; private set; } = null!;

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the test suite.
    /// The method is executed once before any tests in the suite are run.
    /// </summary>
    /// <param name="testContext">The MSTest test context.</param>
    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static void SetUpSuiteAtataContext(TestContext testContext)
    {
        string testClassFullName = testContext.FullyQualifiedTestClassName!;
        Type testClassType = TestSuiteTypeResolver.Resolve(testClassFullName);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .UseDefaultCancellationToken(testContext.CancellationTokenSource.Token)
            .UseTestSuiteType(testClassType)
            .UseAssertionExceptionFactory(MSTestAssertionExceptionFactory.Instance);

        TestSuiteAtataContextMetadata suiteContextMetadata = GetAndApplySuiteMetadata(testClassType, builder);
        FindAndInvokeSuiteConfigurationMethods(testClassType, builder);

        AtataContext suiteContext = builder.Build();

        s_testSuiteDataByTypeName[testClassFullName] = new(suiteContext, suiteContextMetadata);
    }

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the test suite.
    /// The method is executed once after all tests in the suite have run.
    /// </summary>
    /// <param name="testContext">The MSTest test context.</param>
    [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass, ClassCleanupBehavior.EndOfClass)]
    public static void TearDownSuiteAtataContext(TestContext testContext)
    {
        if (s_testSuiteDataByTypeName.TryRemove(testContext.FullyQualifiedTestClassName!, out TestSuiteData? suiteData))
            MSTestAtataContextCompletionHandler.Complete(testContext, suiteData.AtataContext);
    }

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the current test.
    /// The method is executed before each test in the suite.
    /// </summary>
    [TestInitialize]
    public void SetUpTestAtataContext()
    {
        string testClassFullName = TestContext.FullyQualifiedTestClassName!;
        Type testClassType = TestSuiteTypeResolver.Resolve(testClassFullName);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseDefaultCancellationToken(TestContext.CancellationTokenSource.Token)
            .UseTestSuiteType(testClassType)
            .UseTestName(TestContext.TestDisplayName)
            .UseAssertionExceptionFactory(MSTestAssertionExceptionFactory.Instance)
            .LogConsumers.Add(new TextOutputLogConsumer(TestContext.WriteLine))
            .EventSubscriptions.Add(new AddArtifactsToMSTestContextEventHandler(TestContext));

        var suiteContextMetadata = s_testSuiteDataByTypeName[testClassFullName].Metadata;
        suiteContextMetadata?.ApplyToTestBuilder(builder);

        MethodInfo? testMethod = testClassType.GetMethod(TestContext.ManagedMethod);

        if (testMethod is not null)
            ApplyTestMetadata(testMethod, builder);

        ConfigureTestAtataContext(builder);

        Context = builder.Build();
        TestContext.SetAtataContext(Context);
    }

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the current test.
    /// The method is executed after each test in the suite.
    /// </summary>
    [TestCleanup]
    public void TearDownTestAtataContext() =>
        MSTestAtataContextCompletionHandler.Complete(TestContext);

    /// <summary>
    /// Configures the test <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }

    private static TestSuiteAtataContextMetadata GetAndApplySuiteMetadata(Type testClassType, AtataContextBuilder builder)
    {
        var suiteContextMetadata = TestSuiteAtataContextMetadata.GetForType(testClassType);

        builder.UseTestTraits(GetTraits(suiteContextMetadata.Attributes));
        suiteContextMetadata.ApplyToTestSuiteBuilder(builder);

        return suiteContextMetadata;
    }

    private static void ApplyTestMetadata(MethodInfo testMethod, AtataContextBuilder builder)
    {
        var testContextMetadata = TestAtataContextMetadata.GetForMethod(testMethod);

        builder.UseTestTraits(GetTraits(testContextMetadata.Attributes));
        testContextMetadata.ApplyToTestBuilder(builder);
    }

    private static void FindAndInvokeSuiteConfigurationMethods(Type testClassType, AtataContextBuilder builder)
    {
        const BindingFlags methodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        var suiteConfigurationMethods = MethodFinder.FindAllWithAttribute<ConfiguresSuiteAtataContextAttribute>(
            testClassType,
            methodFlags);

        foreach (var methodData in suiteConfigurationMethods)
        {
            methodData.Method.InvokeStaticAsLambda([builder]);
        }
    }

    private static List<TestTrait> GetTraits(IEnumerable<object> attributes)
    {
        List<TestTrait> traits = [];

        foreach (var attribute in attributes)
        {
            if (attribute is TestPropertyAttribute testPropertyAttribute)
            {
                traits.Add(new(testPropertyAttribute.Name, testPropertyAttribute.Value));
            }
            else if (attribute is TestCategoryBaseAttribute testCategoryBaseAttribute)
            {
                foreach (string category in testCategoryBaseAttribute.TestCategories)
                {
                    traits.Add(new(TestTrait.CategoryName, category));
                }
            }
        }

        return traits;
    }

    private sealed class TestSuiteData
    {
        internal TestSuiteData(AtataContext atataContext, TestSuiteAtataContextMetadata metadata)
        {
            AtataContext = atataContext;
            Metadata = metadata;
        }

        public AtataContext AtataContext { get; }

        public TestSuiteAtataContextMetadata Metadata { get; }
    }
}
