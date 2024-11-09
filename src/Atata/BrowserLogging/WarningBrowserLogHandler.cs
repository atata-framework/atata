namespace Atata;

internal class WarningBrowserLogHandler : IBrowserLogHandler
{
    private readonly WebDriverSession _session;

    private readonly LogLevel _minLevelOfWarning;

    public WarningBrowserLogHandler(WebDriverSession session, LogLevel minLevelOfWarning)
    {
        _session = session;
        _minLevelOfWarning = minLevelOfWarning;
    }

    public void Handle(BrowserLogEntry entry)
    {
        if (entry.Level >= _minLevelOfWarning)
        {
            StringBuilder messageBuilder = new("browser log ");
            messageBuilder.Append(ConvertLogLevelToText(entry.Level));

            var pageObject = _session.PageObject;
            if (pageObject is not null)
                messageBuilder.Append(" on ").Append(pageObject.ComponentFullName);

            messageBuilder
                .Append(':')
                .AppendLine()
                .Append(entry.Message);

            if (_session.IsActive)
                _session.RaiseAssertionWarning(messageBuilder.ToString());
        }
    }

    private static string ConvertLogLevelToText(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Fatal => "fatal error",
            LogLevel.Error => "error",
            LogLevel.Warn => "warning",
            _ => $"{logLevel.ToString().ToLowerInvariant()} message"
        };
}
