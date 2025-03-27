#nullable enable

namespace Atata;

/// <summary>
/// Represents an error that occurs during a web driver initialization.
/// </summary>
public class WebDriverInitializationException : Exception
{
    public WebDriverInitializationException()
    {
    }

    public WebDriverInitializationException(string? message)
        : base(message)
    {
    }

    public WebDriverInitializationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
