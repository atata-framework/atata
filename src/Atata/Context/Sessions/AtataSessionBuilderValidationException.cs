namespace Atata;

/// <summary>
/// An exception that occurs during validation of Atata session builder.
/// </summary>
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
