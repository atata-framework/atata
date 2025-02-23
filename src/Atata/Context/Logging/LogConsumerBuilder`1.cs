namespace Atata;

public class LogConsumerBuilder<TLogConsumer>
    where TLogConsumer : ILogConsumer
{
    private readonly LogConsumerConfiguration _logConsumerConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogConsumerBuilder{TLogConsumer}" /> class.
    /// </summary>
    /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
    public LogConsumerBuilder(LogConsumerConfiguration logConsumerConfiguration)
    {
        logConsumerConfiguration.CheckNotNull(nameof(logConsumerConfiguration));

        if (logConsumerConfiguration.Consumer is not TLogConsumer)
            throw new ArgumentException(
                $"'{nameof(logConsumerConfiguration)}.{nameof(LogConsumerConfiguration.Consumer)}' should be of {typeof(TLogConsumer)} type, but was {logConsumerConfiguration.Consumer.GetType()}.",
                nameof(logConsumerConfiguration));

        _logConsumerConfiguration = logConsumerConfiguration;
    }

    /// <summary>
    /// Gets the log consumer.
    /// </summary>
    public TLogConsumer Consumer =>
        (TLogConsumer)_logConsumerConfiguration.Consumer;

    /// <summary>
    /// Sets the output option of log section end.
    /// The default value is <see cref="LogSectionEndOption.Include"/>.
    /// </summary>
    /// <param name="logSectionEnd">The log section end option.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithSectionEnd(LogSectionEndOption logSectionEnd)
    {
        _logConsumerConfiguration.SectionEnd = logSectionEnd;
        return this;
    }

    /// <summary>
    /// Sets the minimum level of the log event to write to the log.
    /// The default value is <see cref="LogLevel.Trace"/>.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithMinLevel(LogLevel level)
    {
        _logConsumerConfiguration.MinLevel = level;
        return this;
    }

    [Obsolete("Use WithNestingLevelIndent(...) instead.")] // Obsolete since v4.0.0.
    public LogConsumerBuilder<TLogConsumer> WithMessageNestingLevelIndent(string messageNestingLevelIndent) =>
        WithNestingLevelIndent(messageNestingLevelIndent);

    /// <summary>
    /// Sets the nesting level indent.
    /// The default value is <c>"- "</c>.
    /// </summary>
    /// <param name="nestingLevelIndent">The nesting level indent.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithNestingLevelIndent(string nestingLevelIndent)
    {
        _logConsumerConfiguration.NestingLevelIndent = nestingLevelIndent;
        return this;
    }

    [Obsolete("Use WithSectionStartPrefix(...) instead.")] // Obsolete since v4.0.0.
    public LogConsumerBuilder<TLogConsumer> WithMessageStartSectionPrefix(string messageStartSectionPrefix) =>
        WithSectionStartPrefix(messageStartSectionPrefix);

    /// <summary>
    /// Sets the prefix of section start.
    /// The default value is <c>"&gt; "</c>.
    /// </summary>
    /// <param name="sectionStartPrefix">The section start prefix.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithSectionStartPrefix(string sectionStartPrefix)
    {
        _logConsumerConfiguration.SectionStartPrefix = sectionStartPrefix;
        return this;
    }

    [Obsolete("Use WithSectionEndPrefix(...) instead.")] // Obsolete since v4.0.0.
    public LogConsumerBuilder<TLogConsumer> WithMessageEndSectionPrefix(string messageEndSectionPrefix) =>
        WithSectionEndPrefix(messageEndSectionPrefix);

    /// <summary>
    /// Sets the prefix of section end.
    /// The default value is <c>"&lt; "</c>.
    /// </summary>
    /// <param name="sectionEndPrefix">The section end prefix.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithSectionEndPrefix(string sectionEndPrefix)
    {
        _logConsumerConfiguration.SectionEndPrefix = sectionEndPrefix;
        return this;
    }

    /// <summary>
    /// Sets a value indicating whether session log should be embedded
    /// in <see cref="AtataContext"/> log hierarchy or it should follow its own hierarchy.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="enable">Whether to enable embedding.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithEmbedSessionLog(bool enable)
    {
        _logConsumerConfiguration.EmbedSessionLog = enable;
        return this;
    }

    /// <summary>
    /// Sets a value indicating whether external source log should be embedded
    /// in <see cref="AtataContext"/> log hierarchy or it should follow its own hierarchy.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <param name="enable">Whether to enable embedding.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithEmbedExternalSourceLog(bool enable)
    {
        _logConsumerConfiguration.EmbedExternalSourceLog = enable;
        return this;
    }

    /// <summary>
    /// Configures a log consumer of the builder.
    /// </summary>
    /// <param name="configureConsumer">The action used to configure the consumer.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> With(Action<TLogConsumer> configureConsumer)
    {
        configureConsumer.CheckNotNull(nameof(configureConsumer));

        configureConsumer.Invoke(Consumer);
        return this;
    }
}
