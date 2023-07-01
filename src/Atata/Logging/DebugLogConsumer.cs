namespace Atata;

public class DebugLogConsumer : TextOutputLogConsumer
{
    protected override void Write(string completeMessage) =>
        Debug.WriteLine(completeMessage);
}
