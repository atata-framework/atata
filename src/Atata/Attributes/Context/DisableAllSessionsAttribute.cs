namespace Atata;

/// <summary>
/// Represents an attribute that disables all session providers of the specified session type regardless of name.
/// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
/// so that the sessions will not automatically start for any scope.
/// </summary>
public class DisableAllSessionsAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableAllSessionsAttribute"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    public DisableAllSessionsAttribute(Type sessionType)
    {
        Guard.ThrowIfNull(sessionType);

        SessionType = sessionType;
    }

    /// <summary>
    /// Gets the type of the session.
    /// </summary>
    public Type SessionType { get; }

    protected internal override void ConfigureAtataContext(AtataContextBuilder builder, object? testSuite) =>
        builder.Sessions.DisableAll(SessionType);
}
