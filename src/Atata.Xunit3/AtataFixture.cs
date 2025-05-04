namespace Atata.Xunit;

public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope)
    {
        _contextScope = contextScope;
        AtataContext.PresetCurrentAsyncLocalBox();
    }

    public AtataContext Context { get; private set; } = null!;

    public virtual async ValueTask InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope)
            .UseDefaultCancellationToken(TestContext.Current.CancellationToken);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync().ConfigureAwait(false);
    }

    private protected abstract void ConfigureAtataContext(AtataContextBuilder builder);

    public virtual async ValueTask DisposeAsync()
    {
        if (Context is not null)
        {
            TestResultState? testResultState = TestContext.Current.TestState;

            if (testResultState is not null)
            {
                if (testResultState.Result == TestResult.Skipped)
                {
                    Context.SetInconclusiveTestResult();
                }
                else if (testResultState.ExceptionTypes?.Length > 0)
                {
                    var (message, stackTrace) = testResultState.ExtractExceptionDetails();

                    Context.HandleTestResultException(message, stackTrace);
                }
            }

            await Context.DisposeAsync().ConfigureAwait(false);
        }
    }
}
