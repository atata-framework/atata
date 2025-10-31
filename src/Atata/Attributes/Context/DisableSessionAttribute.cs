namespace Atata;

/// <summary>
/// Represents an attribute that disables all session providers of the specified session type and name.
/// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
/// so that the sessions will not automatically start for any scope.
/// </summary>
public class DisableSessionAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableSessionAttribute"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public DisableSessionAttribute(Type sessionType, string? sessionName = null)
    {
        Guard.ThrowIfNull(sessionType);

        SessionType = sessionType;
        SessionName = sessionName;
    }

    /// <summary>
    /// Gets the type of the session.
    /// </summary>
    public Type SessionType { get; }

    /// <summary>
    /// Gets the name of the session.
    /// </summary>
    public string? SessionName { get; }

    protected internal override void ConfigureAtataContext(AtataContextBuilder builder, object? testSuite) =>
        builder.Sessions.DisableAllBySessionType(SessionType, SessionName);
}
