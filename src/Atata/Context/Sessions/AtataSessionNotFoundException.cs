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

    public static AtataSessionNotFoundException For<TSession>() =>
        For(typeof(TSession));

    public static AtataSessionNotFoundException For(Type sessionType) =>
        new($"Failed to find session of type {sessionType.FullName} in {nameof(AtataContext)}.");

    internal static AtataSessionNotFoundException ByIndex<TSession>(int index, int bounds) =>
        new($"Failed to find session of type {typeof(TSession).FullName} with index {index} in {nameof(AtataContext)}. " +
            $"There {(bounds == 1 ? "was" : "where")} {bounds} session{(bounds != 1 ? "s" : null)} of such type.");
}
