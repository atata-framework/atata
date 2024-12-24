#nullable enable

namespace Atata;

[Serializable]
public class AtataSessionBuilderNotFoundException : Exception
{
    public AtataSessionBuilderNotFoundException()
    {
    }

    public AtataSessionBuilderNotFoundException(string message)
        : base(message)
    {
    }

    public AtataSessionBuilderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AtataSessionBuilderNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    internal static AtataSessionBuilderNotFoundException For(Type sessionType, string? sessionName, AtataContext context) =>
        new($"Failed to find session builder for {AtataSession.BuildTypedName(sessionType, sessionName)} in {context}.");
}
