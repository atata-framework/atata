﻿namespace Atata;

/// <summary>
/// Represents the configuration of <see cref="ILogConsumer"/>.
/// </summary>
public class LogConsumerConfiguration : ICloneable
{
    public LogConsumerConfiguration(
        ILogConsumer consumer)
        : this(consumer, LogLevel.Trace, LogSectionEndOption.Include)
    {
    }

    public LogConsumerConfiguration(
        ILogConsumer consumer,
        LogLevel minLevel)
        : this(consumer, minLevel, LogSectionEndOption.Include)
    {
    }

    public LogConsumerConfiguration(
        ILogConsumer consumer,
        LogSectionEndOption sectionEnd)
        : this(consumer, LogLevel.Trace, sectionEnd)
    {
    }

    public LogConsumerConfiguration(
        ILogConsumer consumer,
        LogLevel minLevel,
        LogSectionEndOption sectionEnd)
    {
        Consumer = consumer;
        MinLevel = minLevel;
        SectionEnd = sectionEnd;
    }

    /// <summary>
    /// Gets the log consumer.
    /// </summary>
    public ILogConsumer Consumer { get; private set; }

    /// <summary>
    /// Gets the minimum log level.
    /// The default value is <see cref="LogLevel.Trace"/>.
    /// </summary>
    public LogLevel MinLevel { get; internal set; }

    /// <summary>
    /// Gets the output option of log section end.
    /// The default value is <see cref="LogSectionEndOption.Include"/>.
    /// </summary>
    public LogSectionEndOption SectionEnd { get; internal set; }

    /// <summary>
    /// Gets or sets the message nesting level indent.
    /// The default value is <c>"- "</c>.
    /// </summary>
    public string MessageNestingLevelIndent { get; set; } = "- ";

    /// <summary>
    /// Gets or sets the message start section prefix.
    /// The default value is <c>"&gt; "</c>.
    /// </summary>
    public string MessageStartSectionPrefix { get; set; } = "> ";

    /// <summary>
    /// Gets or sets the message end section prefix.
    /// The default value is <c>"&lt; "</c>.
    /// </summary>
    public string MessageEndSectionPrefix { get; set; } = "< ";

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public LogConsumerConfiguration Clone()
    {
        LogConsumerConfiguration clone = (LogConsumerConfiguration)MemberwiseClone();

        if (Consumer is ICloneable cloneableConsumer)
            clone.Consumer = (ILogConsumer)cloneableConsumer.Clone();

        return clone;
    }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() =>
        Clone();
}
