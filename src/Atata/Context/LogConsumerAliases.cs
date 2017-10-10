using System;
using System.Collections.Generic;

namespace Atata
{
    public static class LogConsumerAliases
    {
        public const string Debug = "debug";

        public const string Trace = "trace";

        public const string NUnit = "nunit";

        public const string NLog = "nlog";

        private static readonly Dictionary<string, Func<ILogConsumer>> AliasFactoryMap = new Dictionary<string, Func<ILogConsumer>>(StringComparer.OrdinalIgnoreCase);

        static LogConsumerAliases()
        {
            Register<DebugLogConsumer>(Debug);
            Register<TraceLogConsumer>(Trace);
            Register<NUnitTestContextLogConsumer>(NUnit);
            Register<NLogConsumer>(NLog);
        }

        public static void Register<T>(string typeAlias)
            where T : ILogConsumer, new()
        {
            Register(typeAlias, () => new T());
        }

        public static void Register(string typeAlias, Func<ILogConsumer> logConsumerFactory)
        {
            typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
            logConsumerFactory.CheckNotNull(nameof(logConsumerFactory));

            AliasFactoryMap[typeAlias.ToLower()] = logConsumerFactory;
        }

        public static ILogConsumer Resolve(string typeNameOrAlias)
        {
            typeNameOrAlias.CheckNotNullOrWhitespace(nameof(typeNameOrAlias));

            Func<ILogConsumer> factory;

            return AliasFactoryMap.TryGetValue(typeNameOrAlias, out factory)
                ? factory()
                : ActivatorEx.CreateInstance<ILogConsumer>(typeNameOrAlias);
        }
    }
}
