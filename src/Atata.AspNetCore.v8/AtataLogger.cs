namespace Atata.AspNetCore;

/// <summary>
/// Provides an implementation of <see cref="ILogger"/> that integrates Atata logging with Microsoft logging infrastructure.
/// </summary>
/// <remarks>
/// The <see cref="AtataLogger"/> class adapts Atata's <see cref="ILogManager"/> to the <see cref="ILogger"/> interface,
/// allowing log messages from Microsoft.Extensions.Logging to be routed to Atata's logging system.
/// </remarks>
public sealed class AtataLogger : ILogger
{
    private readonly AtataSession _session;

    private readonly string _sourceName;

    private readonly string? _category;

    private readonly MSLogLevel _minLogLevel;

    public AtataLogger(AtataSession session, string sourceName, string? category = null, MSLogLevel minLogLevel = MSLogLevel.Trace)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNullOrWhitespace(sourceName);

        _session = session;
        _sourceName = sourceName;
        _category = category is null or [] ? null : category;
        _minLogLevel = minLogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        =>
        null;

    public bool IsEnabled(MSLogLevel logLevel) =>
        logLevel >= _minLogLevel;

    public void Log<TState>(
        MSLogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        LogLevel atataLogLevel = ConvertToAtataLogLevel(logLevel);
        ILogManager logManager = ResolveLogManager();
        string message = formatter(state, exception);

        logManager.Log(atataLogLevel, message, exception);
    }

    private static LogLevel ConvertToAtataLogLevel(MSLogLevel logLevel) =>
        logLevel switch
        {
            MSLogLevel.Trace => LogLevel.Trace,
            MSLogLevel.Debug => LogLevel.Debug,
            MSLogLevel.Information => LogLevel.Info,
            MSLogLevel.Warning => LogLevel.Warn,
            MSLogLevel.Error => LogLevel.Error,
            MSLogLevel.Critical => LogLevel.Fatal,
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(logLevel)
        };

    private ILogManager ResolveLogManager()
    {
        var logManager = _session.Log.ForSource(_sourceName);

        return _category is null
            ? logManager
            : logManager.ForCategory(_category);
    }
}
