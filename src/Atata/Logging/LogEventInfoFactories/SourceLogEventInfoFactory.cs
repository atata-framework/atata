namespace Atata;

internal sealed class SourceLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly string _source;

    internal SourceLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        string source)
    {
        Guard.ThrowIfNull(parentFactory);
        Guard.ThrowIfNullOrWhitespace(source);

        _parentFactory = parentFactory;
        _source = source;
    }

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message)
    {
        var eventInfo = _parentFactory.Create(timestamp, level, message);

        eventInfo.Source = _source;

        return eventInfo;
    }
}
