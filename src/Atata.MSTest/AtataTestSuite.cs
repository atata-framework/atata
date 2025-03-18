namespace Atata.MSTest;

public abstract class AtataTestSuite
{
    private static readonly ConcurrentDictionary<string, TestSuiteData> s_testSuiteDataByTypeName = [];

    public TestContext TestContext { get; set; } = null!;

    protected AtataContext Context { get; private set; } = null!;

    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static void SetUpSuiteAtataContext(TestContext testContext)
    {
        string testClassFullName = testContext.FullyQualifiedTestClassName!;
        Type testClassType = TestSuiteTypeResolver.Resolve(testClassFullName);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .UseTestSuiteType(testClassType)
            .UseAssertionExceptionType(typeof(AssertFailedException));

        TestSuiteAtataContextMetadata suiteContextMetadata = GetAndApplySuiteMetadata(testClassType, builder);
        FindAndInvokeSuiteConfigurationMethods(testClassType, builder);

        AtataContext suiteContext = builder.Build(testContext.CancellationTokenSource.Token);

        s_testSuiteDataByTypeName[testClassFullName] = new(suiteContext, suiteContextMetadata);
    }

    [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass, ClassCleanupBehavior.EndOfClass)]
    public static void TearDownSuiteAtataContext(TestContext testContext)
    {
        if (s_testSuiteDataByTypeName.TryRemove(testContext.FullyQualifiedTestClassName!, out TestSuiteData? suiteData))
            MSTestAtataContextCompletionHandler.Complete(testContext, suiteData.AtataContext);
    }

    [TestInitialize]
    public void SetUpTestAtataContext()
    {
        string testClassFullName = TestContext.FullyQualifiedTestClassName!;
        Type testClassType = TestSuiteTypeResolver.Resolve(testClassFullName);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseTestSuiteType(testClassType)
            .UseTestName(TestContext.TestDisplayName)
            .UseAssertionExceptionType(typeof(AssertFailedException))
            .LogConsumers.Add(new TextOutputLogConsumer(TestContext.WriteLine));

        var suiteContextMetadata = s_testSuiteDataByTypeName[testClassFullName].Metadata;
        suiteContextMetadata?.ApplyToTestBuilder(builder);

        MethodInfo? method = testClassType.GetMethod(TestContext.ManagedMethod);

        if (method is not null)
            TestAtataContextMetadata.GetForMethod(method).ApplyToTestBuilder(builder);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CancellationTokenSource.Token);
        TestContext.SetAtataContext(Context);
    }

    [TestCleanup]
    public void TearDownTestAtataContext() =>
        MSTestAtataContextCompletionHandler.Complete(TestContext);

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }

    private static TestSuiteAtataContextMetadata GetAndApplySuiteMetadata(Type testClassType, AtataContextBuilder builder)
    {
        var suiteContextMetadata = TestSuiteAtataContextMetadata.GetForType(testClassType);
        suiteContextMetadata.ApplyToTestSuiteBuilder(builder);
        return suiteContextMetadata;
    }

    private static void FindAndInvokeSuiteConfigurationMethods(Type testClassType, AtataContextBuilder builder)
    {
        const BindingFlags methodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        var suiteConfigurationMethods = MethodFinder.FindAllWithAttribute<ConfiguresSuiteAtataContextAttribute>(
            testClassType,
            methodFlags);

        foreach (var methodData in suiteConfigurationMethods)
        {
            methodData.Method.InvokeAsLambda(null, [builder]);
        }
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
