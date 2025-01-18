#nullable enable

namespace Atata;

/// <summary>
/// Represents a log consumer that uses <see cref="Debug.WriteLine(string)"/> method for logging.
/// </summary>
/// <seealso cref="TextOutputLogConsumer" />
public class DebugLogConsumer : TextOutputLogConsumer
{
    protected override void Write(string completeMessage) =>
        Debug.WriteLine(completeMessage);
}
