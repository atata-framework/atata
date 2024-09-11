namespace Atata;

/// <summary>
/// Provides extension methods for <see cref="LogConsumerBuilder{TLogConsumer}"/>.
/// </summary>
public static class LogConsumerBuilderExtensions
{
    /// <summary>
    /// Specifies the name of the logger.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithLoggerName<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        string loggerName)
        where TLogConsumer : INamedLogConsumer
    {
        builder.Consumer.LoggerName = loggerName;
        return builder;
    }

    /// <summary>
    /// Specifies the name of the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<Log4NetConsumer> WithRepositoryName(
        this LogConsumerBuilder<Log4NetConsumer> builder,
        string repositoryName)
    {
        builder.Consumer.RepositoryName = repositoryName;
        return builder;
    }

    /// <summary>
    /// Specifies the assembly to use to lookup the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryAssembly">The name of the assembly to use to lookup the repository.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<Log4NetConsumer> WithRepositoryAssembly(
        this LogConsumerBuilder<Log4NetConsumer> builder,
        Assembly repositoryAssembly)
    {
        builder.Consumer.RepositoryAssembly = repositoryAssembly;
        return builder;
    }
}
