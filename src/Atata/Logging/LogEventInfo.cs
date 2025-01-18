#nullable enable

namespace Atata;

/// <summary>
/// Represents the logging event information raised by Atata framework.
/// </summary>
public sealed class LogEventInfo
{
    internal LogEventInfo()
    {
    }

    /// <summary>
    /// Gets the context of the logging event.
    /// </summary>
    public AtataContext? Context { get; internal set; }

    /// <summary>
    /// Gets the session of the logging event.
    /// Can be <see langword="null"/> when the log event is written directly to <see cref="AtataContext"/>.
    /// </summary>
    public AtataSession? Session { get; internal set; }

    /// <summary>
    /// Gets the execution unit identifier: <see cref="AtataSession.Id"/> or <see cref="AtataContext.Id"/>.
    /// </summary>
    /// <value>
    /// The execution unit identifier.
    /// </value>
    public string? ExecutionUnitId =>
        Session?.Id ?? Context?.Id ?? null;

    /// <summary>
    /// Gets the category name.
    /// Can be <see langword="null"/> by default.
    /// </summary>
    public string? Category { get; internal set; }

    /// <summary>
    /// Gets the external source.
    /// Can be <see langword="null"/> by default.
    /// </summary>
    public string? ExternalSource { get; internal set; }

    /// <summary>
    /// Gets the timestamp of the logging event.
    /// </summary>
    public DateTime Timestamp { get; internal set; }

    /// <summary>
    /// Gets the time elapsed from start of the <see cref="Context"/>.
    /// </summary>
    public TimeSpan TimeElapsed { get; internal set; }

    /// <summary>
    /// Gets the level of the logging event.
    /// </summary>
    public LogLevel Level { get; internal set; }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string? Message { get; internal set; }

    /// <summary>
    /// Gets the exception information.
    /// </summary>
    public Exception? Exception { get; internal set; }

    /// <summary>
    /// Gets the section start information.
    /// </summary>
    public LogSection? SectionStart { get; internal set; }

    /// <summary>
    /// Gets the section end information.
    /// </summary>
    public LogSection? SectionEnd { get; internal set; }

    /// <summary>
    /// Gets the nesting level.
    /// </summary>
    public int NestingLevel { get; internal set; }

    /// <summary>
    /// Gets the nesting text.
    /// </summary>
    public string? NestingText { get; internal set; }

    /// <summary>
    /// Gets the properties, which includes "log-external-source", "log-category",
    /// and the variables of <see cref="Session"/>/<see cref="Context"/>.
    /// </summary>
    /// <returns>The properties.</returns>
    public IEnumerable<KeyValuePair<string, object>> GetProperties()
    {
        yield return new("time-elapsed", TimeElapsed);

        var variables = Session?.Variables ?? Context?.Variables;

        if (variables is not null)
        {
            foreach (var item in variables)
                yield return item;

            if (!string.IsNullOrEmpty(NestingText))
                yield return new("log-nesting-text", NestingText!);

            if (!string.IsNullOrEmpty(ExternalSource))
                yield return new("log-external-source", ExternalSource!);

            if (!string.IsNullOrEmpty(Category))
                yield return new("log-category", Category!);
        }
    }
}
