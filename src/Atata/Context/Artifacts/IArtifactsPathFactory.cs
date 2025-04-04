namespace Atata;

/// <summary>
/// A factory that creates a relative artifacts path for a given <see cref="AtataContext"/>.
/// </summary>
public interface IArtifactsPathFactory
{
    /// <summary>
    /// Creates a relative artifacts path for a given <see cref="AtataContext"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A relative path.</returns>
    string Create(AtataContext context);
}
