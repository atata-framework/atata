namespace Atata;

internal sealed class LoggingBrowserLogHandler : IBrowserLogHandler
{
    private readonly ILogManager _logManager;

    public LoggingBrowserLogHandler(ILogManager logManager) =>
        _logManager = logManager;

    public void Handle(BrowserLogEntry entry) =>
        _logManager.Trace($"Browser log: {entry.Timestamp:HH:mm:ss.ffff} {entry.Level.ToString(TermCase.Upper)} {entry.Message}");
}
