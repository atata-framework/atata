namespace Atata;

/// <summary>
/// Represents the basic factory of <see cref="LogEventInfo"/>.
/// </summary>
public class BasicLogEventInfoFactory : ILogEventInfoFactory
{
    /// <inheritdoc/>
    public LogEventInfo Create(LogLevel level, string message) =>
        new()
        {
            Level = level,
            Message = message,

            Timestamp = DateTime.Now
        };
}
