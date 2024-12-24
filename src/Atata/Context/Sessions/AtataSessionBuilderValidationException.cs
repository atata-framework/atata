#nullable enable

namespace Atata;

[Serializable]
public class AtataSessionBuilderValidationException : Exception
{
    public AtataSessionBuilderValidationException()
    {
    }

    public AtataSessionBuilderValidationException(string message)
        : base(message)
    {
    }

    public AtataSessionBuilderValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AtataSessionBuilderValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
