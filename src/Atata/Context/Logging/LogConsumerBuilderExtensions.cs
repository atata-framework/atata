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
        string? loggerName)
        where TLogConsumer : INamedLogConsumer
    {
        builder.Consumer.LoggerName = loggerName;
        return builder;
    }

    /// <summary>
    /// Specifies the text parts separator for the log consumer.
    /// The default value is <c>" "</c>.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithSeparator<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        string separator)
        where TLogConsumer : TextOutputLogConsumer
    {
        builder.Consumer.Separator = separator;
        return builder;
    }

    /// <summary>
    /// Specifies the timestamp format for the log consumer.
    /// The default value is <c>"yyyy-MM-dd HH:mm:ss.fff"</c>.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="timestampFormat">The timestamp format.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithTimestampFormat<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        string timestampFormat)
        where TLogConsumer : TextOutputLogConsumer
    {
        builder.Consumer.TimestampFormat = timestampFormat;
        return builder;
    }

    /// <summary>
    /// Specifies the time elapsed format for the log consumer.
    /// The default value is <c>@"hh\:mm\:ss\.fff"</c>.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="timeElapsedFormat">The time elapsed format.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithTimeElapsedFormat<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        string timeElapsedFormat)
        where TLogConsumer : TextOutputLogConsumer
    {
        builder.Consumer.TimeElapsedFormat = timeElapsedFormat;
        return builder;
    }

    /// <summary>
    /// Specifies a value indicating whether to output <see cref="LogEventInfo.Timestamp"/>
    /// instead of <see cref="LogEventInfo.TimeElapsed"/>.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="outputTimestamp">A value indicating whether to output the timestamp.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithOutputTimestamp<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        bool outputTimestamp)
        where TLogConsumer : TextOutputLogConsumer
    {
        builder.Consumer.OutputTimestamp = outputTimestamp;
        return builder;
    }
}
