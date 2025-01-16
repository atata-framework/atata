#nullable enable

namespace Atata;

internal static class WebDriverSetupExecutor
{
    internal static async Task SetUpAsync(AtataContext context, AtataContextBuilder contextBuilder, IReadOnlyList<string> browserNames, CancellationToken cancellationToken)
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

    internal static async Task AutoSetUpSafelyAsync(IReadOnlyList<string> browserNames, AtataContext context, CancellationToken cancellationToken) =>
        await context.Log.CreateSubLog().ExecuteSectionAsync(
            new SetUpWebDriversLogSection(browserNames),
            async () =>
                await WebDriverSetupAdapter.AutoSetUpSafelyAsync(browserNames)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);
}
