namespace Atata.IntegrationTests;

public abstract class TestSuiteBase
{
    private FakeLogConsumer _fakeLogConsumer;

    protected static AtataContext CurrentContext =>
        AtataContext.ResolveCurrent();

    protected FakeLogConsumer CurrentLog =>
        _fakeLogConsumer;

    protected AtataContextBuilder ConfigureSessionlessAtataContext()
    {
        _fakeLogConsumer = new FakeLogConsumer();

        var builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseCulture("en-US")
            .UseTestName(() => TestContext.CurrentContext.Test.Name)
            .UseTestSuiteName(GetCurrentTestFixtureName)
            .UseTestSuiteType(GetCurrentTestFixtureType);

        builder.LogConsumers.Add(new TextOutputLogConsumer(TestContext.WriteLine));
        builder.LogConsumers.Add(_fakeLogConsumer);

        // TODO: Review. Commented temporarily due to AddArtifactsToNUnitTestContext method migration to Atata.NUnit.
        ////builder.EventSubscriptions.AddArtifactsToNUnitTestContext();

        return builder;
    }

    protected AtataContextBuilder ConfigureAtataContextWithFakeSession()
    {
        var builder = ConfigureSessionlessAtataContext();
        builder.Sessions.Add<FakeSessionBuilder>();
        return builder;
    }

    [TearDown]
    public async Task TearDownTestAtataContextAsync()
    {
        var context = AtataContext.Current;

        if (context is { IsActive: true })
        {
            var testContext = TestContext.CurrentContext;

            if (testContext.Result.Outcome.Status == TestStatus.Failed)
                context.HandleTestResultException(testContext.Result.Message, testContext.Result.StackTrace);

            await context.DisposeAsync().ConfigureAwait(false);
        }
    }

    private static string? GetCurrentTestFixtureName()
    {
        ITest? testItem = TestExecutionContext.CurrentContext.CurrentTest;

        if (testItem is NUnit.Framework.Internal.SetUpFixture)
            return testItem.TypeInfo?.Type.Name;

        do
        {
            if (testItem is TestFixture)
                return testItem.Name;

            testItem = testItem.Parent;
        }
        while (testItem is not null);

        return null;
    }

    private static Type? GetCurrentTestFixtureType()
    {
        ITest? testItem = TestExecutionContext.CurrentContext.CurrentTest;

        if (testItem is NUnit.Framework.Internal.SetUpFixture)
            return testItem.TypeInfo?.Type;

        do
        {
            if (testItem is TestFixture)
                return testItem.TypeInfo?.Type;

            testItem = testItem.Parent;
        }
        while (testItem is not null);

        return null;
    }
}
