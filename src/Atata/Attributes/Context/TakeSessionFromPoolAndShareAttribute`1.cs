namespace Atata;

/// <inheritdoc cref="DisableSessionAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class TakeSessionFromPoolAndShareAttribute<TSession> : TakeSessionFromPoolAndShareAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TakeSessionFromPoolAndShareAttribute{TSession}"/> class.
    /// </summary>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public TakeSessionFromPoolAndShareAttribute(string? sessionName = null)
        : base(typeof(TSession), sessionName)
    {
    }
}
