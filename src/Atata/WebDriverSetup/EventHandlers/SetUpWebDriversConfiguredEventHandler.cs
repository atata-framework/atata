#nullable enable

namespace Atata;

/// <summary>
/// <para>
/// Sets up drivers with auto version detection for the local configured browsers.
/// Gets the names of configured local browsers from <see cref="WebDriverSessionBuilder.LocalBrowserToUseName"/> property
/// of <see cref="WebDriverSessionBuilder"/> session builders.
/// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
/// from <c>Atata.WebDriverSetup</c> package.
/// </para>
/// <para>
/// In order to use this method,
/// ensure that <c>Atata.WebDriverSetup</c> package is installed.
/// </para>
/// </summary>
public sealed class SetUpWebDriversConfiguredEventHandler : IAsyncEventHandler<AtataContextInitStartedEvent>
{
    public static SetUpWebDriversConfiguredEventHandler Instance { get; } = new();

    public async Task HandleAsync(AtataContextInitStartedEvent eventData, AtataContext context, CancellationToken cancellationToken)
    {
        AtataContextBuilder contextBuilder = eventData.ContextBuilder;

        string[] browserNames = contextBuilder.Sessions.Builders.OfType<WebDriverSessionBuilder>()
            .SelectMany(x => x.ConfiguredLocalBrowserNames)
            .Distinct()
            .ToArray();

        if (browserNames.Length > 0)
        {
            await WebDriverSetupExecutor.SetUpAsync(context, contextBuilder, browserNames, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
