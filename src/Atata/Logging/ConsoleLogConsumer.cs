#nullable enable

namespace Atata;

/// <summary>
/// Represents a log consumer that uses <see cref="Console.WriteLine(string)"/> method for logging.
/// </summary>
/// <seealso cref="TextOutputLogConsumer" />
public class ConsoleLogConsumer : TextOutputLogConsumer
{
    protected override void Write(string completeMessage) =>
        Console.WriteLine(completeMessage);
}
