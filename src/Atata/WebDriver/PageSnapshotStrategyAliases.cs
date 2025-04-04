namespace Atata;

public static class PageSnapshotStrategyAliases
{
    public const string CdpOrPageSource = nameof(CdpOrPageSource);

    public const string PageSource = nameof(PageSource);

    public const string Cdp = nameof(Cdp);

    private static readonly Dictionary<string, Func<IPageSnapshotStrategy<WebDriverSession>>> s_aliasFactoryMap =
        new(StringComparer.OrdinalIgnoreCase);

    static PageSnapshotStrategyAliases()
    {
        Register(CdpOrPageSource, () => CdpOrPageSourcePageSnapshotStrategy.Instance);
        Register(PageSource, () => PageSourcePageSnapshotStrategy.Instance);
        Register(Cdp, () => CdpPageSnapshotStrategy.Instance);
    }

    public static void Register<T>(string typeAlias)
        where T : IPageSnapshotStrategy<WebDriverSession>, new() =>
        Register(typeAlias, () => new T());

    public static void Register(string typeAlias, IPageSnapshotStrategy<WebDriverSession> strategy) =>
        Register(typeAlias, () => strategy);

    public static void Register(string typeAlias, Func<IPageSnapshotStrategy<WebDriverSession>> strategyFactory)
    {
        typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
        strategyFactory.CheckNotNull(nameof(strategyFactory));

        s_aliasFactoryMap[typeAlias.ToLowerInvariant()] = strategyFactory;
    }

    public static bool TryResolve(string alias, [NotNullWhen(true)] out IPageSnapshotStrategy<WebDriverSession>? strategy)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        if (s_aliasFactoryMap.TryGetValue(alias, out var factory))
        {
            strategy = factory.Invoke();
            return true;
        }
        else
        {
            strategy = null;
            return false;
        }
    }
}
