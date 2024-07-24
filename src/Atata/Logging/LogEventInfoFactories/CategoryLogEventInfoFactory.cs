﻿namespace Atata;

internal sealed class CategoryLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly string _category;

    internal CategoryLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        string category)
    {
        _parentFactory = parentFactory.CheckNotNull(nameof(parentFactory));
        _category = category.CheckNotNullOrWhitespace(nameof(category));
    }

    public LogEventInfo Create(LogLevel level, string message)
    {
        var eventInfo = _parentFactory.Create(level, message);

        eventInfo.Category = _category;

        return eventInfo;
    }
}