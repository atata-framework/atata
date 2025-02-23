#nullable enable

namespace Atata;

[Serializable]
public class LogConsumerNotFoundException : Exception
{
    public LogConsumerNotFoundException()
    {
    }

    public LogConsumerNotFoundException(string message)
        : base(message)
    {
    }

    public LogConsumerNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected LogConsumerNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    internal static LogConsumerNotFoundException ByBuilderType(Type logConsumerBuilderType) =>
        new($"Failed to find {logConsumerBuilderType.Name} in {nameof(AtataContextBuilder)}.");
}
