namespace Atata;

public class LogConsumerAtataContextBuilder<TLogConsumer> : LogConsumersAtataContextBuilder, IHasContext<TLogConsumer>
    where TLogConsumer : ILogConsumer
{
    private readonly LogConsumerConfiguration _logConsumerConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogConsumerAtataContextBuilder{TLogConsumer}" /> class.
    /// </summary>
    /// <param name="logConsumer">The log consumer.</param>
    /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
    /// <param name="buildingContext">The building context.</param>
    public LogConsumerAtataContextBuilder(TLogConsumer logConsumer, LogConsumerConfiguration logConsumerConfiguration, AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
        Context = logConsumer;
        _logConsumerConfiguration = logConsumerConfiguration;
    }

    /// <inheritdoc/>
    public TLogConsumer Context { get; }

    [Obsolete($"Use {nameof(WithSectionEnd)} instead.")] // Obsolete since v2.13.0.
    public LogConsumerAtataContextBuilder<TLogConsumer> WithoutSectionFinish() =>
        WithSectionEnd(LogSectionEndOption.Exclude);

    /// <summary>
    /// Sets the output option of log section end.
    /// The default value is <see cref="LogSectionEndOption.Include"/>.
    /// </summary>
    /// <param name="logSectionEnd">The log section end option.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> WithSectionEnd(LogSectionEndOption logSectionEnd)
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
    public LogConsumerAtataContextBuilder<TLogConsumer> WithMinLevel(LogLevel level)
    {
        _logConsumerConfiguration.MinLevel = level;
        return this;
    }

    /// <summary>
    /// Sets the nesting level indent of the log message.
    /// The default value is <c>"- "</c>.
    /// </summary>
    /// <param name="messageNestingLevelIndent">The message nesting level indent.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageNestingLevelIndent(string messageNestingLevelIndent)
    {
        _logConsumerConfiguration.MessageNestingLevelIndent = messageNestingLevelIndent;
        return this;
    }

    /// <summary>
    /// Sets the start section prefix of the log message.
    /// The default value is <c>"&gt; "</c>.
    /// </summary>
    /// <param name="messageStartSectionPrefix">The message start section prefix.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageStartSectionPrefix(string messageStartSectionPrefix)
    {
        _logConsumerConfiguration.MessageStartSectionPrefix = messageStartSectionPrefix;
        return this;
    }

    /// <summary>
    /// Sets the end section prefix of the log message.
    /// The default value is <c>"&lt; "</c>.
    /// </summary>
    /// <param name="messageEndSectionPrefix">The message end section prefix.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageEndSectionPrefix(string messageEndSectionPrefix)
    {
        _logConsumerConfiguration.MessageEndSectionPrefix = messageEndSectionPrefix;
        return this;
    }
}
