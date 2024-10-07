namespace Atata;

/// <summary>
/// Provides NLog extension methods for <see cref="LogConsumerBuilder{TLogConsumer}"/>.
/// </summary>
public static class NLogConsumerBuilderExtensions
{
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
    /// The default value is <c>@"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5}${when:when='${event-property:log-external-source}'!='':inner= {${event-property:log-external-source}\}}${when:when='${event-property:log-category}'!='':inner= [${event-property:log-category}]} ${message}${onexception:inner= }${exception:format=toString}"</c>.
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
