namespace Atata;

public class AtataSessionBuilderValidationException : Exception
{
    public AtataSessionBuilderValidationException()
    {
    }

    public AtataSessionBuilderValidationException(string? message)
        : base(message)
    {
    }

    public AtataSessionBuilderValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
