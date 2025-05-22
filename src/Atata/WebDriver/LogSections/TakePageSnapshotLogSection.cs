namespace Atata;

public class TakePageSnapshotLogSection : LogSection
{
    public TakePageSnapshotLogSection(int snapshotNumber, string? title = null)
    {
        Level = LogLevel.Trace;
        Message = $"Take page snapshot #{snapshotNumber}";

        if (!string.IsNullOrWhiteSpace(title))
            Message += $" {title}";
    }
}
