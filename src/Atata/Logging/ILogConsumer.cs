#nullable enable

namespace Atata;

/// <summary>
/// Defines a method to log the event information.
/// </summary>
public interface ILogConsumer
{
    /// <summary>
    /// Logs the specified event information.
    /// </summary>
    /// <param name="eventInfo">The event information.</param>
    void Log(LogEventInfo eventInfo);
}
