namespace Atata;

internal readonly record struct PostponedLogEntry
{
    internal PostponedLogEntry(LogEventInfo logEvent, int nestingLevel)
    {
        LogEvent = logEvent;
        NestingLevel = nestingLevel;
    }

    internal LogEventInfo LogEvent { get; }

    internal int NestingLevel { get; }
}
