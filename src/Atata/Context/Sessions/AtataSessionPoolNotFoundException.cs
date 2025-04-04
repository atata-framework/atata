namespace Atata;

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
