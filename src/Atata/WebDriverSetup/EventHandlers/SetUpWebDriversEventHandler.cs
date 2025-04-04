namespace Atata;

/// <summary>
/// <para>
/// Sets up drivers with auto version detection for the specified browsers.
/// Invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
/// from <c>Atata.WebDriverSetup</c> package.
/// </para>
/// <para>
/// In order to use this method,
/// ensure that <c>Atata.WebDriverSetup</c> package is installed.
/// </para>
/// </summary>
public sealed class SetUpWebDriversEventHandler : IAsyncEventHandler<AtataContextInitStartedEvent>
{
    private readonly string[] _browserNames;

    public SetUpWebDriversEventHandler(params string[] browserNames)
    {
        browserNames.CheckNotNullOrEmpty(nameof(browserNames));
        _browserNames = browserNames;
    }

    public async Task HandleAsync(AtataContextInitStartedEvent eventData, AtataContext context, CancellationToken cancellationToken) =>
        await WebDriverSetupExecutor.SetUpAsync(context, eventData.ContextBuilder, _browserNames, cancellationToken)
            .ConfigureAwait(false);
}
