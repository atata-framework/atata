namespace Atata;

/// <summary>
/// Represents a log consumer that uses <see cref="Trace.WriteLine(string)"/> method for logging.
/// </summary>
/// <seealso cref="TextOutputLogConsumer" />
public class TraceLogConsumer : TextOutputLogConsumer
{
    [SuppressMessage("Minor Code Smell", "S6670:\"Trace.Write\" and \"Trace.WriteLine\" should not be used")]
    protected override void Write(string completeMessage) =>
        Trace.WriteLine(completeMessage);
}
