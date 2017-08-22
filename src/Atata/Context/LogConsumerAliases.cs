using System;
using System.Collections.Generic;

namespace Atata
{
    public static class LogConsumerAliases
    {
        private static readonly Dictionary<string, Func<ILogConsumer>> AliasFactoryMap = new Dictionary<string, Func<ILogConsumer>>(StringComparer.OrdinalIgnoreCase);

        static LogConsumerAliases()
        {
            Register<DebugLogConsumer>(Debug);
            Register<TraceLogConsumer>(Trace);
            Register<NUnitTestContextLogConsumer>(NUnit);
            Register<NLogConsumer>(NLog);
        }

        public static string Debug => nameof(Debug);

        public static string Trace => nameof(Trace);

        public static string NUnit => nameof(NUnit);

        public static string NLog => nameof(NLog);

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
            Func<ILogConsumer> factory;

            return AliasFactoryMap.TryGetValue(typeNameOrAlias, out factory)
                ? factory()
                : ActivatorEx.CreateInstance<ILogConsumer>(typeNameOrAlias);
        }
    }
}
