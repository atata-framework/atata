#nullable enable

namespace Atata;

/// <summary>
/// Represents the log section.
/// </summary>
public class LogSection
{
    private object? _result;

    public LogSection(string message, LogLevel level = LogLevel.Info)
    {
        Message = message;
        Level = level;
    }

    protected LogSection()
    {
    }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string? Message { get; protected set; }

    /// <summary>
    /// Gets or sets the log level.
    /// </summary>
    public LogLevel Level { get; protected set; }

    /// <summary>
    /// Gets the date/time of section start.
    /// </summary>
    public DateTime StartedAt { get; internal set; }

    /// <summary>
    /// Gets the date/time of section start.
    /// </summary>
    internal Stopwatch Stopwatch { get; } = new();

    /// <summary>
    /// Gets a value indicating whether the result is set.
    /// </summary>
    public bool IsResultSet { get; private set; }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    public object? Result
    {
        get => _result;
        set
        {
            _result = value;
            IsResultSet = true;
        }
    }

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; internal set; }

    /// <summary>
    /// Gets the elapsed time of section execution.
    /// </summary>
    public TimeSpan ElapsedTime => Stopwatch.Elapsed;
}
