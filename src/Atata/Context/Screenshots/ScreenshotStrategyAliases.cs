using System;
using System.Collections.Generic;

namespace Atata
{
    public static class ScreenshotStrategyAliases
    {
        public const string WebDriverViewport = nameof(WebDriverViewport);

        public const string WebDriverFullPage = nameof(WebDriverFullPage);

        public const string CdpFullPage = nameof(CdpFullPage);

        public const string FullPageOrViewport = nameof(FullPageOrViewport);

        private static readonly Dictionary<string, Func<IScreenshotStrategy>> s_aliasFactoryMap =
            new(StringComparer.OrdinalIgnoreCase);

        static ScreenshotStrategyAliases()
        {
            Register(WebDriverViewport, () => WebDriverViewportScreenshotStrategy.Instance);
            Register(WebDriverFullPage, () => WebDriverFullPageScreenshotStrategy.Instance);
            Register(CdpFullPage, () => CdpFullPageScreenshotStrategy.Instance);
            Register(FullPageOrViewport, () => FullPageOrViewportScreenshotStrategy.Instance);
        }

        public static void Register<T>(string typeAlias)
            where T : IScreenshotStrategy, new() =>
            Register(typeAlias, () => new T());

        public static void Register(string typeAlias, IScreenshotStrategy strategy) =>
            Register(typeAlias, () => strategy);

        public static void Register(string typeAlias, Func<IScreenshotStrategy> strategyFactory)
        {
            typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
            strategyFactory.CheckNotNull(nameof(strategyFactory));

            s_aliasFactoryMap[typeAlias.ToLowerInvariant()] = strategyFactory;
        }

        public static bool TryResolve(string alias, out IScreenshotStrategy strategy)
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
}
