namespace Atata;

/// <inheritdoc cref="DisableAllSessionsAttribute"/>
/// <typeparam name="TSession">The type of the session.</typeparam>
public class DisableAllSessionsAttribute<TSession> : DisableAllSessionsAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableAllSessionsAttribute{TSession}"/> class.
    /// </summary>
    public DisableAllSessionsAttribute()
        : base(typeof(TSession))
    {
    }
}
