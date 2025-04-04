#nullable enable

namespace Atata;

internal sealed class LoggingBrowserLogHandler : IBrowserLogHandler
{
    private const string LogSourceName = "Browser";

    private readonly AtataSession _session;

    public LoggingBrowserLogHandler(AtataSession session) =>
        _session = session;

    public void Handle(BrowserLogEntry entry) =>
        _session.Log?.ForExternalSource(LogSourceName)
            .Log(entry.UtcTimestamp, entry.Level, entry.Message);
}
