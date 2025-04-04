namespace Atata;

public class LogConsumerNotFoundException : Exception
{
    public LogConsumerNotFoundException()
    {
    }

    public LogConsumerNotFoundException(string? message)
        : base(message)
    {
    }

    public LogConsumerNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    internal static LogConsumerNotFoundException ByBuilderType(Type logConsumerBuilderType) =>
        new($"Failed to find {logConsumerBuilderType.Name} in {nameof(AtataContextBuilder)}.");
}
