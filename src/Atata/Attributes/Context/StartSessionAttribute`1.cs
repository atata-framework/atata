namespace Atata;

/// <inheritdoc cref="StartSessionAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class StartSessionAttribute<TSession> : StartSessionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartSessionAttribute{TSession}"/> class.
    /// </summary>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public StartSessionAttribute(string? sessionName = null)
        : base(typeof(TSession), sessionName)
    {
    }
}
