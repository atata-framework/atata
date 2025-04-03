#nullable enable

namespace Atata;

/// <summary>
/// Represents the result of UI component scope element finding.
/// </summary>
public abstract class ComponentScopeFindResult
{
    /// <summary>
    /// Gets the missing result.
    /// </summary>
    public static MissingComponentScopeFindResult Missing { get; } = new();
}
