namespace Atata;

/// <summary>
/// Represents an error that occurs during an driver creation.
/// </summary>
[Serializable]
public class WebDriverInitializationException : Exception
{
    public WebDriverInitializationException()
    {
    }

    public WebDriverInitializationException(string message)
        : base(message)
    {
    }

    public WebDriverInitializationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected WebDriverInitializationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
