namespace Atata;

public static class LogConsumerAliases
{
    public const string Trace = "trace";

    public const string Debug = "debug";

    public const string Console = "console";

    public const string NUnit = "nunit";

    public const string NLog = "nlog";

    public const string NLogFile = "nlog-file";

    public const string Log4Net = "log4net";

    private static readonly Dictionary<string, Func<ILogConsumer>> s_aliasFactoryMap = new(StringComparer.OrdinalIgnoreCase);

    private static readonly Lazy<ConstructorInfo> s_lazyNUnitTestContextLogConsumerConstructor = new(ResolveNUnitTestContextLogConsumerConstructor);

    static LogConsumerAliases()
    {
        Register<TraceLogConsumer>(Trace);
        Register<DebugLogConsumer>(Debug);
        Register<ConsoleLogConsumer>(Console);
        Register<NLogConsumer>(NLog);
        Register<NLogFileConsumer>(NLogFile);
        Register<Log4NetConsumer>(Log4Net);

        Register(NUnit, CreateNUnitTestContextLogConsumer);
    }

    public static void Register<T>(string typeAlias)
        where T : ILogConsumer, new()
        =>
        Register(typeAlias, () => new T());

    public static void Register(string typeAlias, Func<ILogConsumer> logConsumerFactory)
    {
        typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
        logConsumerFactory.CheckNotNull(nameof(logConsumerFactory));

        s_aliasFactoryMap[typeAlias.ToLowerInvariant()] = logConsumerFactory;
    }

    public static ILogConsumer Resolve(string typeNameOrAlias)
    {
        typeNameOrAlias.CheckNotNullOrWhitespace(nameof(typeNameOrAlias));

        return s_aliasFactoryMap.TryGetValue(typeNameOrAlias, out Func<ILogConsumer> factory)
            ? factory()
            : ActivatorEx.CreateInstance<ILogConsumer>(typeNameOrAlias);
    }

    private static ILogConsumer CreateNUnitTestContextLogConsumer() =>
        (ILogConsumer)s_lazyNUnitTestContextLogConsumerConstructor.Value.Invoke(null);

    private static ConstructorInfo ResolveNUnitTestContextLogConsumerConstructor()
    {
        Type type = Type.GetType("Atata.NUnit.NUnitTestContextLogConsumer,Atata.NUnit", true);

        return type.GetConstructor([]);
    }
}
