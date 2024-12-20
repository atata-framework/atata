#nullable enable

namespace Atata;

[Serializable]
public class AtataSessionPoolNotFoundException : Exception
{
    public AtataSessionPoolNotFoundException()
    {
    }

    public AtataSessionPoolNotFoundException(string message)
        : base(message)
    {
    }

    public AtataSessionPoolNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AtataSessionPoolNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
