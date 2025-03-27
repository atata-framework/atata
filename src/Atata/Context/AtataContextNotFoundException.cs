#nullable enable

namespace Atata;

public class AtataContextNotFoundException : Exception
{
    public AtataContextNotFoundException()
    {
    }

    public AtataContextNotFoundException(string? message)
        : base(message)
    {
    }

    public AtataContextNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    internal static AtataContextNotFoundException ForCurrentIsNull() =>
        new($"Failed to find {nameof(AtataContext)} instance. {nameof(AtataContext)}.{nameof(AtataContext.Current)} property is null.");
}
