namespace Atata;

/// <summary>
/// Represents the factory of <see cref="LogEventInfo"/>,
/// which populates the log event <see cref="LogEventInfo.Context"/> property and
/// sets <see cref="LogEventInfo.Timestamp"/> with a date/time relative to
/// time zone of <see cref="AtataContextGlobalProperties.TimeZone"/> value
/// of <see cref="AtataContext.GlobalProperties"/>.
/// </summary>
internal sealed class AtataContextLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly AtataContext _context;

    internal AtataContextLogEventInfoFactory(AtataContext context) =>
        _context = context.CheckNotNull(nameof(context));

    /// <inheritdoc/>
    public LogEventInfo Create(LogLevel level, string message) =>
        new()
        {
            Level = level,
            Message = message,
            Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AtataContext.GlobalProperties.TimeZone),
            Context = _context
        };
}
