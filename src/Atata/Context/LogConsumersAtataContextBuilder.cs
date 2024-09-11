namespace Atata;

/// <summary>
/// Represents the builder of log consumers.
/// </summary>
public class LogConsumersAtataContextBuilder : AtataContextBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogConsumersAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="buildingContext">The building context.</param>
    public LogConsumersAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">
    /// The type of the log consumer.
    /// Should have default constructor.
    /// </typeparam>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> Add<TLogConsumer>()
        where TLogConsumer : ILogConsumer, new()
        =>
        Add(new TLogConsumer());

    /// <summary>
    /// Adds the log consumer by its type name or alias.
    /// Predefined aliases are defined in <see cref="LogConsumerAliases"/> static class.
    /// </summary>
    /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<ILogConsumer> Add(string typeNameOrAlias)
    {
        ILogConsumer consumer = LogConsumerAliases.Resolve(typeNameOrAlias);
        return Add(consumer);
    }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="consumer">The log consumer.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> Add<TLogConsumer>(TLogConsumer consumer)
        where TLogConsumer : ILogConsumer
    {
        consumer.CheckNotNull(nameof(consumer));

        var consumerConfiguration = new LogConsumerConfiguration(consumer);
        BuildingContext.LogConsumerConfigurations.Add(consumerConfiguration);
        return new LogConsumerAtataContextBuilder<TLogConsumer>(consumerConfiguration);
    }

    /// <summary>
    /// Returns a log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer or adds a new one.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> Configure<TLogConsumer>()
       where TLogConsumer : ILogConsumer
    {
        var consumerConfiguration = BuildingContext.LogConsumerConfigurations.LastOrDefault(x => x.Consumer is TLogConsumer);

        if (consumerConfiguration is null)
        {
            var consumer = ActivatorEx.CreateInstance<TLogConsumer>();
            return Add(consumer);
        }
        else
        {
            return new LogConsumerAtataContextBuilder<TLogConsumer>(consumerConfiguration);
        }
    }

    /// <summary>
    /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<TraceLogConsumer> AddTrace() =>
        Add(new TraceLogConsumer());

    /// <summary>
    /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<DebugLogConsumer> AddDebug() =>
        Add(new DebugLogConsumer());

    /// <summary>
    /// Adds the <see cref="ConsoleLogConsumer"/> instance that uses <see cref="Console"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<ConsoleLogConsumer> AddConsole() =>
        Add(new ConsoleLogConsumer());

    /// <summary>
    /// Adds the <see cref="NUnitTestContextLogConsumer"/> instance that uses <c>NUnit.Framework.TestContext</c> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<NUnitTestContextLogConsumer> AddNUnitTestContext() =>
        Add(new NUnitTestContextLogConsumer());

    /// <summary>
    /// Adds the <see cref="NLogConsumer"/> instance that uses <c>NLog.Logger</c> class for logging.
    /// </summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<NLogConsumer> AddNLog(string loggerName = null) =>
        Add(new NLogConsumer(loggerName));

    /// <summary>
    /// Adds the <see cref="NLogFileConsumer"/> instance that uses <c>NLog.Logger</c> class for logging into file.
    /// </summary>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<NLogFileConsumer> AddNLogFile() =>
        Add(new NLogFileConsumer());

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<Log4NetConsumer> AddLog4Net(string loggerName = null) =>
        Add(new Log4NetConsumer { LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<Log4NetConsumer> AddLog4Net(string repositoryName, string loggerName = null) =>
        Add(new Log4NetConsumer { RepositoryName = repositoryName, LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerAtataContextBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerAtataContextBuilder<Log4NetConsumer> AddLog4Net(Assembly repositoryAssembly, string loggerName = null) =>
        Add(new Log4NetConsumer { RepositoryAssembly = repositoryAssembly, LoggerName = loggerName });
}
