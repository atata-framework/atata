namespace Atata;

[Serializable]
public class AtataContextNotFoundException : Exception
{
    public AtataContextNotFoundException()
    {
    }

    public AtataContextNotFoundException(string message)
        : base(message)
    {
    }

    public AtataContextNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AtataContextNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    internal static AtataContextNotFoundException ForCurrentIsNull() =>
        new($"Failed to find {nameof(AtataContext)} instance. {nameof(AtataContext)}.{nameof(AtataContext.Current)} property is null.");
}
