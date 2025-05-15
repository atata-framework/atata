namespace Atata.AspNetCore;

public sealed class AtataLogger : ILogger
{
    private readonly AtataSession _session;

    private readonly string _sourceName;

    private readonly string? _category;

    public AtataLogger(AtataSession session, string sourceName, string? category = null)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNullOrWhitespace(sourceName);

        _session = session;
        _sourceName = sourceName;
        _category = category is null or [] ? null : category;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        =>
        null;

    public bool IsEnabled(MSLogLevel logLevel) =>
        true;

    public void Log<TState>(
        MSLogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
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
        var logManager = _session.Log.ForExternalSource(_sourceName);

        return _category is null
            ? logManager
            : logManager.ForCategory(_category);
    }
}
