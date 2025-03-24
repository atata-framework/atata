namespace Atata.ExtentReports;

public sealed class ExtentLogConsumer : ILogConsumer
{
    private static readonly Regex s_normalizeMessageLineBreakRegex =
        new(@"(?<=\<br\>)\s+", RegexOptions.Compiled);

    public void Log(LogEventInfo eventInfo)
    {
        Status status = ResolveLogStatus(eventInfo);

        string completeMessage = BuildCompleteMessage(eventInfo);
        completeMessage = NormalizeMessage(completeMessage);

        ExtentContext extentContext = ExtentContext.ResolveFor(eventInfo.Context);
        extentContext.Test.Log(status, completeMessage);
        extentContext.LastLogEvent = eventInfo;
    }

    private static Status ResolveLogStatus(LogEventInfo eventInfo)
    {
        switch (eventInfo.Level)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                return 0;
            case LogLevel.Info:
                if (eventInfo.SectionEnd is VerificationLogSection)
                {
                    if (eventInfo.SectionEnd.Exception is not null)
                    {
                        return Status.Fail;
                    }
                    else if (eventInfo.SectionEnd.Message?.StartsWith("Wait", StringComparison.Ordinal) == true)
                    {
                        var lastLogLevel = ExtentContext.ResolveFor(eventInfo.Context)
                            .LastLogEvent?.Level;

                        if (lastLogLevel is LogLevel.Error)
                            return Status.Fail;
                        else if (lastLogLevel is not LogLevel.Warn)
                            return Status.Pass;
                    }
                }

                return Status.Info;
            case LogLevel.Warn:
                return Status.Warning;
            case LogLevel.Error:
                return Status.Error;
            case LogLevel.Fatal:
                return Status.Error;
            default:
                throw ExceptionFactory.CreateForUnsupportedEnumValue(eventInfo.Level, $"{nameof(eventInfo)}.{nameof(LogEventInfo.Level)}");
        }
    }

    private static string BuildCompleteMessage(LogEventInfo eventInfo) =>
        !string.IsNullOrWhiteSpace(eventInfo.Message) && eventInfo.Exception is not null
            ? $"{eventInfo.Message} {eventInfo.Exception}"
            : eventInfo.Exception is not null
            ? eventInfo.Exception.ToString()
            : eventInfo.Message!;

    private static string NormalizeMessage(string message)
    {
        message = HttpUtility.HtmlEncode(message)
            .Replace(Environment.NewLine, "<br>");

        return s_normalizeMessageLineBreakRegex
            .Replace(message, match => string.Concat(Enumerable.Repeat("&nbsp;", match.Length)));
    }

    // TODO: Use GeneratedRegex after .NET 8+ upgrade.
    ////[GeneratedRegex(@"(?<=\<br\>)\s+")]
    ////private static partial Regex GetNormalizeMessageLineBreakRegex();
}
