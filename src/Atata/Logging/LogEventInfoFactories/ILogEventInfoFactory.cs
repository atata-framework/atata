namespace Atata;

/// <summary>
/// A factory of <see cref="LogEventInfo"/>.
/// </summary>
internal interface ILogEventInfoFactory
{
    /// <summary>
    /// Creates the <see cref="LogEventInfo"/> instance with the specified <paramref name="level"/> and <paramref name="message"/>.
    /// </summary>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="level">The level.</param>
    /// <param name="message">The message.</param>
    /// <returns>The <see cref="LogEventInfo"/> instance.</returns>
    LogEventInfo Create(DateTime timestamp, LogLevel level, string? message);
}
