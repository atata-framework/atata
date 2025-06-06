﻿namespace Atata;

internal sealed class AtataContextLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly AtataContext _context;

    internal AtataContextLogEventInfoFactory(AtataContext context)
    {
        Guard.ThrowIfNull(context);
        _context = context;
    }

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message) =>
        new(_context)
        {
            Timestamp = timestamp,
            TimeElapsed = _context.ExecutionStopwatch.Elapsed,
            Level = level,
            Message = message
        };
}
