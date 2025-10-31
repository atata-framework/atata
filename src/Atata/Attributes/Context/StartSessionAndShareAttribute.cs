namespace Atata;

/// <summary>
/// Represents an attribute that configures test suite <see cref="AtataContextBuilder"/>
/// to start the specified session and share it with tests.
/// </summary>
public class StartSessionAndShareAttribute : TestSuiteAtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartSessionAndShareAttribute"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public StartSessionAndShareAttribute(Type sessionType, string? sessionName = null)
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

    protected internal override void ConfigureAtataContext(AtataContextBuilder builder, object? testSuite)
    {
        builder.Sessions.DisableAllBySessionType(SessionType, SessionName);

        builder.Sessions.Configure(
            SessionType,
            SessionName,
            x =>
            {
                x.StartScopes = AtataContextScopes.TestSuite;
                x.Mode = AtataSessionMode.Shared;
            },
            ConfigurationMode.ConfigureOrAdd);
    }

    protected internal override void ConfigureTestAtataContext(AtataContextBuilder builder, object? testSuite)
    {
        builder.Sessions.DisableAllBySessionType(SessionType, SessionName);

        builder.Sessions.Borrow(SessionType, x => x.Name = SessionName);
    }
}
