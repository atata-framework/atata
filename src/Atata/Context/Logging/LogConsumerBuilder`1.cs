namespace Atata;

public sealed class LogConsumerBuilder<TLogConsumer>
    where TLogConsumer : ILogConsumer
{
    private readonly ScopeLimitedLogConsumerConfiguration _configuration;

    internal LogConsumerBuilder(ScopeLimitedLogConsumerConfiguration configuration) =>
        _configuration = configuration;

    /// <summary>
    /// Gets the log consumer.
    /// </summary>
    public TLogConsumer Consumer =>
        (TLogConsumer)_configuration.ConsumerConfiguration.Consumer;

    /// <summary>
    /// Sets the output option of log section end.
    /// The default value is <see cref="LogSectionEndOption.Include"/>.
    /// </summary>
    /// <param name="logSectionEnd">The log section end option.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithSectionEnd(LogSectionEndOption logSectionEnd)
    {
        _configuration.ConsumerConfiguration.SectionEnd = logSectionEnd;
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
        _configuration.ConsumerConfiguration.MinLevel = level;
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
        _configuration.ConsumerConfiguration.NestingLevelIndent = nestingLevelIndent;
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
        _configuration.ConsumerConfiguration.SectionStartPrefix = sectionStartPrefix;
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
        _configuration.ConsumerConfiguration.SectionEndPrefix = sectionEndPrefix;
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
        _configuration.ConsumerConfiguration.EmbedSessionLog = enable;
        return this;
    }

    /// <summary>
    /// Sets a value indicating whether source log should be embedded
    /// in <see cref="AtataContext"/> log hierarchy or it should follow its own hierarchy.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <param name="enable">Whether to enable embedding.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithEmbedSourceLog(bool enable)
    {
        _configuration.ConsumerConfiguration.EmbedSourceLog = enable;
        return this;
    }

    /// <summary>
    /// Sets the target scopes for which to apply the log consumer.
    /// The default value is <see cref="AtataContextScopes.All"/>.
    /// </summary>
    /// <param name="scopes">The target scopes.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> WithTargetScopes(AtataContextScopes scopes)
    {
        _configuration.Scopes = scopes;
        return this;
    }

    /// <summary>
    /// Configures a log consumer of the builder.
    /// </summary>
    /// <param name="configureConsumer">The action used to configure the consumer.</param>
    /// <returns>The same builder instance.</returns>
    public LogConsumerBuilder<TLogConsumer> With(Action<TLogConsumer> configureConsumer)
    {
        Guard.ThrowIfNull(configureConsumer);

        configureConsumer.Invoke(Consumer);
        return this;
    }
}
