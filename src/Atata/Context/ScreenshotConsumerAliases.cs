using System;
using System.Collections.Generic;

namespace Atata
{
    public static class ScreenshotConsumerAliases
    {
        public const string File = "file";

        private static readonly Dictionary<string, Func<IScreenshotConsumer>> s_aliasFactoryMap = new Dictionary<string, Func<IScreenshotConsumer>>(StringComparer.OrdinalIgnoreCase);

        static ScreenshotConsumerAliases() =>
            Register<FileScreenshotConsumer>(File);

        public static void Register<T>(string typeAlias)
            where T : IScreenshotConsumer, new() =>
            Register(typeAlias, () => new T());

        public static void Register(string typeAlias, Func<IScreenshotConsumer> logConsumerFactory)
        {
            typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
            logConsumerFactory.CheckNotNull(nameof(logConsumerFactory));

            s_aliasFactoryMap[typeAlias.ToLowerInvariant()] = logConsumerFactory;
        }

        public static IScreenshotConsumer Resolve(string typeNameOrAlias)
        {
            typeNameOrAlias.CheckNotNullOrWhitespace(nameof(typeNameOrAlias));

            return s_aliasFactoryMap.TryGetValue(typeNameOrAlias, out Func<IScreenshotConsumer> factory)
                ? factory()
                : ActivatorEx.CreateInstance<IScreenshotConsumer>(typeNameOrAlias);
        }
    }
}
