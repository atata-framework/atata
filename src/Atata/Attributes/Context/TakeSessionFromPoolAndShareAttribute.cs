namespace Atata;

/// <summary>
/// Represents an attribute that configures test suite <see cref="AtataContextBuilder"/>
/// to take the specified session from pool and share it with tests.
/// </summary>
public class TakeSessionFromPoolAndShareAttribute : TestSuiteAtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TakeSessionFromPoolAndShareAttribute"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public TakeSessionFromPoolAndShareAttribute(Type sessionType, string? sessionName = null)
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

        builder.Sessions.TakeFromPool(
            SessionType,
            x => x.UseName(SessionName).UseSharedMode(true));
    }

    protected internal override void ConfigureTestAtataContext(AtataContextBuilder builder, object? testSuite)
    {
        builder.Sessions.DisableAllBySessionType(SessionType, SessionName);

        builder.Sessions.Borrow(SessionType, x => x.Name = SessionName);
    }
}
