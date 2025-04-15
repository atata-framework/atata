namespace Atata;

/// <summary>
/// An exception that is thrown when a session pool cannot be found.
/// </summary>
public class AtataSessionPoolNotFoundException : Exception
{
    public AtataSessionPoolNotFoundException()
    {
    }

    public AtataSessionPoolNotFoundException(string? message)
        : base(message)
    {
    }

    public AtataSessionPoolNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
