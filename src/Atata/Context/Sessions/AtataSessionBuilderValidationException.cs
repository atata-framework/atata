namespace Atata;

/// <summary>
/// An exception that occurs during the validation of an Atata session builder.
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
