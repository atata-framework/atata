namespace Atata;

/// <summary>
/// An exception that occurs during validation of Atata session request.
/// </summary>
public class AtataSessionRequestValidationException : Exception
{
    public AtataSessionRequestValidationException()
    {
    }

    public AtataSessionRequestValidationException(string? message)
        : base(message)
    {
    }

    public AtataSessionRequestValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
