namespace Atata;

/// <summary>
/// Represents the configuration of <see cref="ILogConsumer"/>.
/// </summary>
public sealed class LogConsumerConfiguration : ICloneable
{
    private SkipLogCondition _skipCondition;

    private List<PostponedLogEntry>? _postponedLogEntries;

    internal LogConsumerConfiguration(
        ILogConsumer consumer)
        : this(consumer, LogLevel.Trace, LogSectionEndOption.Include)
    {
    }

    internal LogConsumerConfiguration(
        ILogConsumer consumer,
        LogLevel minLevel)
        : this(consumer, minLevel, LogSectionEndOption.Include)
    {
    }

    internal LogConsumerConfiguration(
        ILogConsumer consumer,
        LogSectionEndOption sectionEnd)
        : this(consumer, LogLevel.Trace, sectionEnd)
    {
    }

    internal LogConsumerConfiguration(
        ILogConsumer consumer,
        LogLevel minLevel,
        LogSectionEndOption sectionEnd)
    {
        Guard.ThrowIfNull(consumer);

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

    [Obsolete("Use NestingLevelIndent instead.")] // Obsolete since v4.0.0.
    public string MessageNestingLevelIndent
    {
        get => NestingLevelIndent;
        set => NestingLevelIndent = value;
    }

    /// <summary>
    /// Gets or sets the nesting level indent.
    /// The default value is <c>"- "</c>.
    /// </summary>
    public string NestingLevelIndent { get; set; } = "- ";

    [Obsolete("Use SectionStartPrefix instead.")] // Obsolete since v4.0.0.
    public string MessageStartSectionPrefix { get; set; } = "> ";

    /// <summary>
    /// Gets or sets the prefix of section start.
    /// The default value is <c>"&gt; "</c>.
    /// </summary>
    public string SectionStartPrefix { get; set; } = "> ";

    [Obsolete("Use SectionEndPrefix instead.")] // Obsolete since v4.0.0.
    public string MessageEndSectionPrefix { get; set; } = "< ";

    /// <summary>
    /// Gets or sets the prefix of section end.
    /// The default value is <c>"&lt; "</c>.
    /// </summary>
    public string SectionEndPrefix { get; set; } = "< ";

    /// <summary>
    /// Gets or sets a value indicating whether session log should be embedded
    /// in <see cref="AtataContext"/> log hierarchy or it should follow its own hierarchy.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool EmbedSessionLog { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether source log should be embedded
    /// in <see cref="AtataContext"/> log hierarchy or it should follow its own hierarchy.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool EmbedSourceLog { get; set; }

    /// <summary>
    /// Gets or sets the condition under which logging should be skipped depending on a test result status.
    /// The default value is <see cref="SkipLogCondition.None"/>.
    /// When set to a value other than <see cref="SkipLogCondition.None"/>, log entries are postponed
    /// until the end of the test item.
    /// </summary>
    public SkipLogCondition SkipCondition
    {
        get => _skipCondition;
        set
        {
            _skipCondition = value;

            if (value == SkipLogCondition.None)
                StopPostponing();
            else if (!IsPostponing)
                StartPostponing();
        }
    }

    /// <summary>
    /// Gets a value indicating whether log entries are being postponed.
    /// </summary>
    [MemberNotNullWhen(true, nameof(PostponedLogEntries))]
    [MemberNotNullWhen(true, nameof(PostponingSyncLock))]
    internal bool IsPostponing =>
        _postponedLogEntries is not null;

    internal List<PostponedLogEntry>? PostponedLogEntries =>
        _postponedLogEntries;

    internal object? PostponingSyncLock { get; private set; }

    internal void StartPostponing()
    {
        PostponingSyncLock = new();
        _postponedLogEntries = [];
    }

    internal void StopPostponing() =>
        _postponedLogEntries = null;

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

        clone._postponedLogEntries = null;

        return clone;
    }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() =>
        Clone();
}
