namespace Atata;

/// <inheritdoc cref="StartSessionAndShareAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class StartSessionAndShareAttribute<TSession> : StartSessionAndShareAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartSessionAndShareAttribute{TSession}"/> class.
    /// </summary>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public StartSessionAndShareAttribute(string? sessionName = null)
        : base(typeof(TSession), sessionName)
    {
    }
}
