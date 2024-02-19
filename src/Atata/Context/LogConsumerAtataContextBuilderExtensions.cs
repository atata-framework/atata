namespace Atata;

public static class LogConsumerAtataContextBuilderExtensions
{
    /// <summary>
    /// Specifies the name of the logger.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerAtataContextBuilder<TLogConsumer> WithLoggerName<TLogConsumer>(
        this LogConsumerAtataContextBuilder<TLogConsumer> builder,
        string loggerName)
        where TLogConsumer : INamedLogConsumer
    {
        builder.Context.LoggerName = loggerName;
        return builder;
    }

    /// <summary>
    /// Specifies the name of the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerAtataContextBuilder<Log4NetConsumer> WithRepositoryName(
        this LogConsumerAtataContextBuilder<Log4NetConsumer> builder,
        string repositoryName)
    {
        builder.Context.RepositoryName = repositoryName;
        return builder;
    }

    /// <summary>
    /// Specifies the assembly to use to lookup the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryAssembly">The name of the assembly to use to lookup the repository.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerAtataContextBuilder<Log4NetConsumer> WithRepositoryAssembly(
        this LogConsumerAtataContextBuilder<Log4NetConsumer> builder,
        Assembly repositoryAssembly)
    {
        builder.Context.RepositoryAssembly = repositoryAssembly;
        return builder;
    }

    /// <summary>
    /// Sets the file name template of the log file.
    /// The default value is <c>"Trace.log"</c>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFileNameTemplate(
        this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
        string fileNameTemplate)
    {
        fileNameTemplate.CheckNotNullOrWhitespace(nameof(fileNameTemplate));

        builder.Context.FileNameTemplate = fileNameTemplate;
        return builder;
    }

    /// <summary>
    /// Specifies the layout of log event.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="layout">The layout of log event.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithLayout(
        this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
        string layout)
    {
        builder.Context.Layout = layout;
        return builder;
    }
}
