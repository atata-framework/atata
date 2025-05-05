namespace Atata.MSTest;

/// <summary>
/// Provides methods to set up and tear down the global <see cref="AtataContext"/> for MSTest.
/// </summary>
public static class MSTestGlobalAtataContextSetup
{
    /// <summary>
    /// Sets up the global <see cref="AtataContext"/> using the specified test context and optional configuration action.
    /// </summary>
    /// <typeparam name="TGlobalFixture">The type of the global fixture.</typeparam>
    /// <param name="testContext">The MSTest test context.</param>
    /// <param name="configure">An action delegate to configure the global <see cref="AtataContextBuilder"/>.</param>
    public static void SetUp<TGlobalFixture>(
        TestContext testContext,
        Action<AtataContextBuilder>? configure = null)
        =>
        SetUp(typeof(TGlobalFixture), testContext, configure);

    /// <summary>
    /// Sets up the global <see cref="AtataContext"/> using the specified global fixture type, test context, and optional configuration action.
    /// </summary>
    /// <param name="globalFixtureType">The type of the global fixture.</param>
    /// <param name="testContext">The MSTest test context.</param>
    /// <param name="configure">An action delegate to configure the global <see cref="AtataContextBuilder"/>.</param>
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
            .UseDefaultCancellationToken(testContext.CancellationTokenSource.Token)
            .UseTestSuiteType(globalFixtureType)
            .UseAssertionExceptionFactory(MSTestAssertionExceptionFactory.Instance);

        configure?.Invoke(builder);

        builder.Build();
    }

    /// <summary>
    /// Tears down the global <see cref="AtataContext"/> for the specified test context.
    /// </summary>
    /// <param name="testContext">The MSTest test context.</param>
    public static void TearDown(TestContext testContext)
    {
        if (AtataContext.Global is { } globalAtataContext)
            MSTestAtataContextCompletionHandler.Complete(testContext, globalAtataContext);
    }
}
