#nullable enable

namespace Atata;

/// <summary>
/// <para>
/// Sets up drivers with auto version detection for the local browsers to use.
/// Gets the name of the local browsers to use from <see cref="WebDriverSessionBuilder.LocalBrowserToUseName"/> property
/// of <see cref="WebDriverSessionBuilder"/> session builders.
/// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafelyAsync(...)</c> static method
/// from <c>Atata.WebDriverSetup</c> package.
/// </para>
/// <para>
/// In order to use this method,
/// ensure that <c>Atata.WebDriverSetup</c> package is installed.
/// </para>
/// </summary>
public sealed class SetUpWebDriversForUseEventHandler : IAsyncEventHandler<AtataContextInitStartedEvent>
{
    public static SetUpWebDriversForUseEventHandler Instance { get; } = new();

    public async Task HandleAsync(AtataContextInitStartedEvent eventData, AtataContext context, CancellationToken cancellationToken)
    {
        AtataContextBuilder contextBuilder = eventData.ContextBuilder;

        string[] browserNames = contextBuilder.Sessions.Builders.OfType<WebDriverSessionBuilder>()
            .Where(x => x.UsesLocalBrowser)
            .Select(x => x.LocalBrowserToUseName!)
            .Distinct()
            .ToArray();

        if (browserNames.Length > 0)
        {
            await WebDriverSetupExecutor.SetUpAsync(context, contextBuilder, browserNames, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
