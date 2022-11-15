namespace Atata
{
    public class TakePageSnapshotLogSection : LogSection
    {
        public TakePageSnapshotLogSection(int snapshotNumber, string title = null)
        {
            Message = $"Take page snapshot #{snapshotNumber:D2}";

            if (!string.IsNullOrWhiteSpace(title))
                Message += $" - {title}";
        }
    }
}
