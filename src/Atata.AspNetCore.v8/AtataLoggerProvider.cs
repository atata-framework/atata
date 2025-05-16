using System.Collections.Concurrent;

namespace Atata.AspNetCore;

/// <summary>
/// Provides an implementation of <see cref="ILoggerProvider"/> that integrates Atata logging with the Microsoft logging infrastructure.
/// </summary>
public sealed class AtataLoggerProvider : ILoggerProvider
{
    private readonly AtataSession _session;

    private readonly string _sourceName;

    private readonly ConcurrentDictionary<string, AtataLogger> _loggers = new();

    public AtataLoggerProvider(AtataSession session, string sourceName)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNullOrWhitespace(sourceName);

        _session = session;
        _sourceName = sourceName;
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName ?? string.Empty, DoCreateLogger);

    private AtataLogger DoCreateLogger(string categoryName) =>
        new(_session, _sourceName, categoryName);

    public void Dispose() =>
        _loggers.Clear();
}
