namespace Atata;

/// <summary>
/// A configuration mode, which defines how to handle the configuration of a component when it is not found.
/// </summary>
public enum ConfigurationMode
{
    /// <summary>
    /// Configures the component or throws an exception if it is not found.
    /// </summary>
    ConfigureOrThrow,

    /// <summary>
    /// Configures the component only if it exists; otherwise, no action is taken.
    /// </summary>
    ConfigureIfExists,

    /// <summary>
    /// Configures the component if it exists, or adds a new component if it does not exist.
    /// </summary>
    ConfigureOrAdd
}
