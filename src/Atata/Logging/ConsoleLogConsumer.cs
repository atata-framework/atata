namespace Atata;

/// <summary>
/// Represents the log consumer that uses <see cref="Console.WriteLine(string)"/> method for logging.
/// </summary>
/// <seealso cref="Atata.TextOutputLogConsumer" />
public class ConsoleLogConsumer : TextOutputLogConsumer
{
    protected override void Write(string completeMessage) =>
        Console.WriteLine(completeMessage);
}
