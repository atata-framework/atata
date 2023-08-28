namespace Atata;

internal class WarningBrowserLogHandler : IBrowserLogHandler
{
    private readonly AtataContext _context;

    private readonly LogLevel _minLevelOfWarning;

    public WarningBrowserLogHandler(AtataContext context, LogLevel minLevelOfWarning)
    {
        _context = context;
        _minLevelOfWarning = minLevelOfWarning;
    }

    public void Handle(BrowserLogEntry entry)
    {
        if (entry.Level >= _minLevelOfWarning)
        {
            StringBuilder messageBuilder = new StringBuilder("browser log ");
            messageBuilder.Append(ConvertLogLevelToText(entry.Level));

            if (_context.PageObject is not null)
                messageBuilder.Append(" on ").Append(_context.PageObject.ComponentFullName);

            messageBuilder
                .Append(':')
                .AppendLine()
                .Append(entry.Message);

            _context.RaiseWarning(messageBuilder.ToString());
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
