namespace Atata;

/// <inheritdoc cref="DisableSessionAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class DisableSessionAttribute<TSession> : DisableSessionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableSessionAttribute{TSession}"/> class.
    /// </summary>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public DisableSessionAttribute(string? sessionName = null)
        : base(typeof(TSession), sessionName)
    {
    }
}
