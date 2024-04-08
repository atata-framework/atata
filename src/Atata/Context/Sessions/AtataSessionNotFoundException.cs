namespace Atata;

[Serializable]
public class AtataSessionNotFoundException : Exception
{
    public AtataSessionNotFoundException()
    {
    }

    public AtataSessionNotFoundException(string message)
        : base(message)
    {
    }

    public AtataSessionNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AtataSessionNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public static AssemblyNotFoundException For<TSession>() =>
        For(typeof(TSession));

    public static AssemblyNotFoundException For(Type sessionType) =>
        new($"Failed to find session of type {sessionType.FullName} in {nameof(AtataContext)}.");
}
