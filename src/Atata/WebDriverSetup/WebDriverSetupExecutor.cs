#nullable enable

namespace Atata;

internal static class WebDriverSetupExecutor
{
    internal static async Task SetUpAsync(AtataContext context, AtataContextBuilder contextBuilder, IEnumerable<string> browserNames, CancellationToken cancellationToken)
    {
        bool executeInParallel = CanExecuteInParallelWithSessionsBuild(context, contextBuilder, browserNames);
        Task task = AutoSetUpSafelyAsync(browserNames, context, cancellationToken);

        if (executeInParallel)
        {
            context.EventBus.Subscribe<AtataContextInitCompletedEvent>(
                async ct => await task.ConfigureAwait(false));
        }
        else
        {
            await task.ConfigureAwait(false);
        }
    }

    internal static bool CanExecuteInParallelWithSessionsBuild(AtataContext context, AtataContextBuilder contextBuilder, IEnumerable<string> browserNames) =>
        !contextBuilder.Sessions.GetProvidersForScope(context.Scope)
            .OfType<WebDriverSessionBuilder>()
            .Any(x => (x.Mode != AtataSessionMode.Pool || x.PoolInitialCapacity > 0) && x.UsesLocalBrowser && browserNames.Contains(x.LocalBrowserToUseName));

    internal static async Task AutoSetUpSafelyAsync(IEnumerable<string> browserNames, AtataContext context, CancellationToken cancellationToken)
    {
        string logMessage = $"Set up web drivers {Stringifier.ToString(browserNames)}";

        await context.Log.CreateSubLog().ExecuteSectionAsync(
            new LogSection(logMessage, LogLevel.Trace),
            async () =>
                await WebDriverSetupAdapter.AutoSetUpSafelyAsync(browserNames)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);
    }
}
