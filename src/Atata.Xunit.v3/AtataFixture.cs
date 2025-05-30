namespace Atata.Xunit;

/// <summary>
/// Represents a base class for Atata Xunit test classes.
/// </summary>
public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope)
    {
        _contextScope = contextScope;
        AtataContext.PresetCurrentAsyncLocalBox();
    }

    /// <summary>
    /// Gets the current <see cref="AtataContext"/> instance.
    /// </summary>
    public AtataContext Context { get; private set; } = null!;

    /// <summary>
    /// Initializes the <see cref="Context"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> object.</returns>
    public virtual async ValueTask InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope)
            .UseDefaultCancellationToken(TestContext.Current.CancellationToken);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync().ConfigureAwait(false);
    }

    private protected abstract void ConfigureAtataContext(AtataContextBuilder builder);

    /// <summary>
    /// Disposes the <see cref="Context"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> object.</returns>
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
