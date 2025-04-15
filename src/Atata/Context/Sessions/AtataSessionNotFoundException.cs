namespace Atata;

/// <summary>
/// An exception that is thrown when a session cannot be found.
/// </summary>
public class AtataSessionNotFoundException : Exception
{
    public AtataSessionNotFoundException()
    {
    }

    public AtataSessionNotFoundException(string? message)
        : base(message)
    {
    }

    public AtataSessionNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    internal static AtataSessionNotFoundException For<TSession>(AtataContext context, bool recursively = false) =>
        For(typeof(TSession), context, recursively);

    internal static AtataSessionNotFoundException For(Type sessionType, AtataContext context, bool recursively = false) =>
        new($"Failed to find {AtataSession.BuildTypedName(sessionType, null)} in {context}{(recursively ? " and ancestors" : null)}.");

    internal static AtataSessionNotFoundException ByIndex<TSession>(int index, int bounds, AtataContext context) =>
        new($"Failed to find {AtataSession.BuildTypedName(typeof(TSession), null)} with index {index} in {context}. " +
            $"There {(bounds == 1 ? "was" : "were")} {bounds} session{(bounds != 1 ? "s" : null)} of such type.");

    internal static AtataSessionNotFoundException ByName<TSession>(string? name, int bounds, AtataContext context, bool recursively = false) =>
        new($"Failed to find {AtataSession.BuildTypedName(typeof(TSession), name)} in {context}{(recursively ? " and ancestors" : null)}. " +
            $"There {(bounds == 1 ? "was" : "were")} {bounds} session{(bounds != 1 ? "s" : null)} of such type{(bounds > 0 ? ", but none with such name" : null)}.");
}
