namespace Atata;

/// <inheritdoc cref="TakeSessionFromPoolAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class TakeSessionFromPoolAttribute<TSession> : TakeSessionFromPoolAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TakeSessionFromPoolAttribute{TSession}"/> class.
    /// </summary>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public TakeSessionFromPoolAttribute(string? sessionName = null)
        : base(typeof(TSession), sessionName)
    {
    }
}
