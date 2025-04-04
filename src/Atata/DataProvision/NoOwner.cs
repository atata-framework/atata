namespace Atata;

/// <summary>
/// Represents a stub owner for static classes.
/// </summary>
public sealed class NoOwner
{
    private NoOwner()
    {
    }

    /// <summary>
    /// Gets the default instance.
    /// </summary>
    public static NoOwner Instance { get; } = new();
}
