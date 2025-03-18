namespace Atata.MSTest;

public static class MSTestGlobalAtataContextSetup
{
    public static void SetUp<TGlobalFixture>(
        TestContext testContext,
        Action<AtataContextBuilder>? configure = null)
        =>
        SetUp(typeof(TGlobalFixture), testContext, configure);

    public static void SetUp(
        Type globalFixtureType,
        TestContext testContext,
        Action<AtataContextBuilder>? configure = null)
    {
        if (globalFixtureType is null)
            throw new ArgumentNullException(nameof(globalFixtureType));

        if (testContext is null)
            throw new ArgumentNullException(nameof(testContext));

        TestSuiteTypeResolver.Assembly = globalFixtureType.Assembly;

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global)
            .UseTestSuiteType(globalFixtureType)
            .UseAssertionExceptionType(typeof(AssertFailedException));

        configure?.Invoke(builder);

        builder.Build(testContext.CancellationTokenSource.Token);
    }

    public static void TearDown(TestContext testContext) =>
        MSTestAtataContextCompletionHandler.Complete(testContext, AtataContext.Global);
}
