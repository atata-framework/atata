namespace Atata;

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

    /// <summary>
    /// Sets the file name template of the log file.
    /// The default value is <c>"Trace.log"</c>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<NLogFileConsumer> WithFileNameTemplate(
        this LogConsumerBuilder<NLogFileConsumer> builder,
        string fileNameTemplate)
    {
        fileNameTemplate.CheckNotNullOrWhitespace(nameof(fileNameTemplate));

        builder.Consumer.FileNameTemplate = fileNameTemplate;
        return builder;
    }

    /// <summary>
    /// Specifies the layout of log event.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="layout">The layout of log event.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<NLogFileConsumer> WithLayout(
        this LogConsumerBuilder<NLogFileConsumer> builder,
        string layout)
    {
        builder.Consumer.Layout = layout;
        return builder;
    }
}
