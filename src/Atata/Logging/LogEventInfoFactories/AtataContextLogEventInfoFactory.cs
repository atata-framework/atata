namespace Atata;

internal sealed class AtataContextLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly AtataContext _context;

    internal AtataContextLogEventInfoFactory(AtataContext context) =>
        _context = context.CheckNotNull(nameof(context));

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string message) =>
        new()
        {
            Timestamp = timestamp,
            Level = level,
            Message = message,
            Context = _context
        };
}
