#nullable enable

namespace Atata;

/// <summary>
/// Represents a log consumer that uses <see cref="Trace.WriteLine(string)"/> method for logging.
/// </summary>
/// <seealso cref="TextOutputLogConsumer" />
public class TraceLogConsumer : TextOutputLogConsumer
{
    protected override void Write(string completeMessage) =>
        Trace.WriteLine(completeMessage);
}
