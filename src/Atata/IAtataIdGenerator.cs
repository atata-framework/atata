namespace Atata;

/// <summary>
/// Provides a functionality to generate identifiers.
/// </summary>
public interface IAtataIdGenerator
{
    /// <summary>
    /// Generates a unique string identifier.
    /// </summary>
    /// <returns>A unique string identifier.</returns>
    string GenerateId();
}
