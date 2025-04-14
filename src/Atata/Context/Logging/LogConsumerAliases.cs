namespace Atata;

public static class LogConsumerAliases
{
    public const string Trace = "trace";

    public const string Debug = "debug";

    public const string Console = "console";

    public const string NUnit = "nunit";

    public const string NLog = "nlog";

    public const string NLogFile = "nlog-file";

    [Obsolete("Use NLogFile instead.")] // Obsolete since v4.0.0.
    public const string Log4Net = "log4net";

    private static readonly Dictionary<string, Func<ILogConsumer>> s_aliasFactoryMap =
        new(StringComparer.OrdinalIgnoreCase);

    private static readonly Lazy<ConstructorInfo> s_lazyNUnitTestContextLogConsumerConstructor =
        new(ResolveNUnitTestContextLogConsumerConstructor);

    private static readonly Lazy<ConstructorInfo> s_lazyNLogConsumerConstructor =
        new(ResolveNLogConsumerConstructor);

    private static readonly Lazy<ConstructorInfo> s_lazyNLogFileConsumerConstructor =
        new(ResolveNLogFileConsumerConstructor);

    static LogConsumerAliases()
    {
        Register<TraceLogConsumer>(Trace);
        Register<DebugLogConsumer>(Debug);
        Register<ConsoleLogConsumer>(Console);

#pragma warning disable CS0618 // Type or member is obsolete
        Register<Log4NetConsumer>(Log4Net);
#pragma warning restore CS0618 // Type or member is obsolete

        Register(NUnit, CreateNUnitTestContextLogConsumer);
        Register(NLog, CreateNLogConsumer);
        Register(NLogFile, CreateNLogFileConsumer);
    }

    public static void Register<T>(string typeAlias)
        where T : ILogConsumer, new()
        =>
        Register(typeAlias, () => new T());

    public static void Register(string typeAlias, Func<ILogConsumer> logConsumerFactory)
    {
        Guard.ThrowIfNullOrWhitespace(typeAlias);
        Guard.ThrowIfNull(logConsumerFactory);

        s_aliasFactoryMap[typeAlias.ToLowerInvariant()] = logConsumerFactory;
    }

    public static ILogConsumer Resolve(string typeNameOrAlias)
    {
        Guard.ThrowIfNullOrWhitespace(typeNameOrAlias);

        return s_aliasFactoryMap.TryGetValue(typeNameOrAlias, out Func<ILogConsumer> factory)
            ? factory()
            : ActivatorEx.CreateInstance<ILogConsumer>(typeNameOrAlias);
    }

    private static ILogConsumer CreateNUnitTestContextLogConsumer() =>
        (ILogConsumer)s_lazyNUnitTestContextLogConsumerConstructor.Value.Invoke(null);

    private static ILogConsumer CreateNLogConsumer() =>
        (ILogConsumer)s_lazyNLogConsumerConstructor.Value.Invoke(null);

    private static ILogConsumer CreateNLogFileConsumer() =>
        (ILogConsumer)s_lazyNLogFileConsumerConstructor.Value.Invoke(null);

    private static ConstructorInfo ResolveNUnitTestContextLogConsumerConstructor() =>
        GetDefaultTypeConstructor("Atata.NUnit.NUnitTestContextLogConsumer,Atata.NUnit");

    private static ConstructorInfo ResolveNLogConsumerConstructor() =>
        GetDefaultTypeConstructor("Atata.NLog.NLogConsumer,Atata.NLog");

    private static ConstructorInfo ResolveNLogFileConsumerConstructor() =>
        GetDefaultTypeConstructor("Atata.NLog.NLogFileConsumer,Atata.NLog");

    private static ConstructorInfo GetDefaultTypeConstructor(string typeName)
    {
        Type type = Type.GetType(typeName, true);

        return type.GetConstructor([]);
    }
}
