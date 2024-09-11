namespace Atata;

/// <summary>
/// Represents the builder of log consumers.
/// </summary>
public class LogConsumersBuilder
{
    public LogConsumersBuilder() =>
        Items = [];

    public LogConsumersBuilder(IEnumerable<LogConsumerConfiguration> items) =>
        Items = items.ToList();

    /// <summary>
    /// Gets the list of log consumer configurations.
    /// </summary>
    public List<LogConsumerConfiguration> Items { get; }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">
    /// The type of the log consumer.
    /// Should have default constructor.
    /// </typeparam>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Add<TLogConsumer>()
        where TLogConsumer : ILogConsumer, new()
        =>
        Add(new TLogConsumer());

    /// <summary>
    /// Adds the log consumer by its type name or alias.
    /// Predefined aliases are defined in <see cref="LogConsumerAliases"/> static class.
    /// </summary>
    /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<ILogConsumer> Add(string typeNameOrAlias)
    {
        ILogConsumer consumer = LogConsumerAliases.Resolve(typeNameOrAlias);
        return Add(consumer);
    }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="consumer">The log consumer.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Add<TLogConsumer>(TLogConsumer consumer)
        where TLogConsumer : ILogConsumer
    {
        consumer.CheckNotNull(nameof(consumer));

        var consumerConfiguration = new LogConsumerConfiguration(consumer);
        Items.Add(consumerConfiguration);
        return new LogConsumerBuilder<TLogConsumer>(consumerConfiguration);
    }

    /// <summary>
    /// Returns a log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer or adds a new one.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Configure<TLogConsumer>()
       where TLogConsumer : ILogConsumer
    {
        var consumerConfiguration = Items.LastOrDefault(x => x.Consumer is TLogConsumer);

        if (consumerConfiguration is null)
        {
            var consumer = ActivatorEx.CreateInstance<TLogConsumer>();
            return Add(consumer);
        }
        else
        {
            return new LogConsumerBuilder<TLogConsumer>(consumerConfiguration);
        }
    }

    /// <summary>
    /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TraceLogConsumer> AddTrace() =>
        Add(new TraceLogConsumer());

    /// <summary>
    /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<DebugLogConsumer> AddDebug() =>
        Add(new DebugLogConsumer());

    /// <summary>
    /// Adds the <see cref="ConsoleLogConsumer"/> instance that uses <see cref="Console"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<ConsoleLogConsumer> AddConsole() =>
        Add(new ConsoleLogConsumer());

    /// <summary>
    /// Adds the <see cref="NLogConsumer"/> instance that uses <c>NLog.Logger</c> class for logging.
    /// </summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<NLogConsumer> AddNLog(string loggerName = null) =>
        Add(new NLogConsumer(loggerName));

    /// <summary>
    /// Adds the <see cref="NLogFileConsumer"/> instance that uses <c>NLog.Logger</c> class for logging into file.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<NLogFileConsumer> AddNLogFile() =>
        Add(new NLogFileConsumer());

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<Log4NetConsumer> AddLog4Net(string loggerName = null) =>
        Add(new Log4NetConsumer { LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<Log4NetConsumer> AddLog4Net(string repositoryName, string loggerName = null) =>
        Add(new Log4NetConsumer { RepositoryName = repositoryName, LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<Log4NetConsumer> AddLog4Net(Assembly repositoryAssembly, string loggerName = null) =>
        Add(new Log4NetConsumer { RepositoryAssembly = repositoryAssembly, LoggerName = loggerName });
}
