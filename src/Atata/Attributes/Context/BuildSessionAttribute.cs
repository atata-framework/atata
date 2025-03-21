#nullable enable

namespace Atata;

/// <summary>
/// Represents an attribute that configures <see cref="AtataContextBuilder"/>
/// to build the specified session.
/// </summary>
public class BuildSessionAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BuildSessionAttribute"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    /// <param name="sessionName">Name of the session. Can be <see langword="null"/>.</param>
    public BuildSessionAttribute(Type sessionType, string? sessionName = null)
    {
        SessionType = sessionType.CheckNotNull(nameof(sessionType));
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

    public override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.Sessions.ConfigureOrAdd(
            SessionType,
            SessionName,
            x => x.StartScopes = AtataContextScopes.All);
}
