#nullable enable

namespace Atata;

/// <summary>
/// Provides a <see cref="GetComparisonDescription"/> method.
/// </summary>
public interface IDescribesComparison
{
    /// <summary>
    /// Gets the human-readable description of a comparison.
    /// </summary>
    /// <returns>A description.</returns>
    string GetComparisonDescription();
}
