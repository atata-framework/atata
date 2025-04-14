namespace Atata;

internal sealed class CategoryLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly string _category;

    internal CategoryLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        string category)
    {
        Guard.ThrowIfNull(parentFactory);
        Guard.ThrowIfNullOrWhitespace(category);

        _parentFactory = parentFactory;
        _category = category;
    }

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message)
    {
        var eventInfo = _parentFactory.Create(timestamp, level, message);

        eventInfo.Category = _category;

        return eventInfo;
    }
}
