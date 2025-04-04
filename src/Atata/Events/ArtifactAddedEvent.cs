namespace Atata;

/// <summary>
/// Represents an event that occurs when an artifact file is saved.
/// </summary>
public sealed class ArtifactAddedEvent
{
    internal ArtifactAddedEvent(string absoluteFilePath, string relativeFilePath, string? artifactType, string? artifactTitle)
    {
        AbsoluteFilePath = absoluteFilePath;
        RelativeFilePath = relativeFilePath;
        ArtifactType = artifactType;
        ArtifactTitle = artifactTitle;
    }

    /// <summary>
    /// Gets the absolute artifact file path.
    /// </summary>
    public string AbsoluteFilePath { get; }

    /// <summary>
    /// Gets the relative artifact file path.
    /// </summary>
    public string RelativeFilePath { get; }

    /// <summary>
    /// Gets the type of the artifact.
    /// The value can be present in <see cref="ArtifactTypes"/>, be a custom one, or just be <see langword="null"/>.
    /// </summary>
    public string? ArtifactType { get; }

    /// <summary>
    /// Gets the artifact title.
    /// Can be <see langword="null"/>.
    /// </summary>
    public string? ArtifactTitle { get; }
}
