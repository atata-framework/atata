namespace Atata;

public class TakeScreenshotLogSection : LogSection
{
    public TakeScreenshotLogSection(int screenshotNumber, string? title = null)
    {
        Level = LogLevel.Trace;
        Message = $"Take screenshot #{screenshotNumber:D2}";

        if (!string.IsNullOrWhiteSpace(title))
            Message += $" {title}";
    }
}
