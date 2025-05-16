using System.Collections.Concurrent;

namespace Atata.AspNetCore;

/// <summary>
/// Provides an implementation of <see cref="ILoggerProvider"/> that integrates Atata logging with the Microsoft logging infrastructure.
/// </summary>
public sealed class AtataLoggerProvider : ILoggerProvider
{
    private readonly AtataSession _session;

    private readonly string _sourceName;

    private readonly MSLogLevel _minLogLevel;

    private readonly ConcurrentDictionary<string, AtataLogger> _loggers = new();

    public AtataLoggerProvider(AtataSession session, string sourceName, MSLogLevel minLogLevel = MSLogLevel.Trace)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNullOrWhitespace(sourceName);

        _session = session;
        _sourceName = sourceName;
        _minLogLevel = minLogLevel;
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName ?? string.Empty, DoCreateLogger);

    private AtataLogger DoCreateLogger(string categoryName) =>
        new(_session, _sourceName, categoryName, _minLogLevel);

    public void Dispose() =>
        _loggers.Clear();
}
