namespace Atata;

/// <summary>
/// Provides log4net extension methods for <see cref="LogConsumersBuilder"/>.
/// </summary>
public static class Log4NetLogConsumersBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<Log4NetConsumer> AddLog4Net(
        this LogConsumersBuilder builder,
        string loggerName = null)
        =>
        builder.Add(new Log4NetConsumer { LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<Log4NetConsumer> AddLog4Net(
        this LogConsumersBuilder builder,
        string repositoryName,
        string loggerName = null)
        =>
        builder.Add(new Log4NetConsumer { RepositoryName = repositoryName, LoggerName = loggerName });

    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<Log4NetConsumer> AddLog4Net(
        this LogConsumersBuilder builder,
        Assembly repositoryAssembly,
        string loggerName = null)
        =>
        builder.Add(new Log4NetConsumer { RepositoryAssembly = repositoryAssembly, LoggerName = loggerName });
}
