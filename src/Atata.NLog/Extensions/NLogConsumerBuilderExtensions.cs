#pragma warning disable S103 // Lines should not be too long

using Atata.NLog;

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
        Guard.ThrowIfNullOrWhitespace(fileNameTemplate);

        builder.Consumer.FileNameTemplate = fileNameTemplate;
        return builder;
    }

    /// <summary>
    /// Specifies the layout of log event.
    /// The default value is <c>@"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-source}'!='':inner={${event-property:log-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}"</c>.
    /// </summary>
    /// <remarks>
    /// If you want to replace "time elapsed" column in layout with "timestamp", you can replace the value
    /// <c>"{event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff}"</c> with
    /// <c>"{date:format=yyyy-MM-dd HH\:mm\:ss.fff}"</c>.
    /// </remarks>
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

    /// <summary>
    /// Specifies to use separate log files for log sources.
    /// The main log file will be "Atata.log".
    /// Source log file will be "{source}.log", e.g., "Browser.log", "App.log".
    /// Sets <c>"${{event-property:log-source:whenEmpty=Atata}}.log"</c> to <see cref="NLogFileConsumer.FileNameTemplate"/>.
    /// Sets <c>@"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}"</c> to <see cref="NLogFileConsumer.Layout"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<NLogFileConsumer> WithSeparateSourceLogFiles(
        this LogConsumerBuilder<NLogFileConsumer> builder)
    {
        builder.Consumer.Layout = @"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}";
        builder.Consumer.FileNameTemplate = "${{event-property:log-source:whenEmpty=Atata}}.log";
        return builder;
    }
}
