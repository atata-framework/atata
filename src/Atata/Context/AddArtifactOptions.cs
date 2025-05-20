namespace Atata;

#pragma warning disable SA1623 // Property summary documentation should match accessors

/// <summary>
/// Represents the options for adding an artifact.
/// </summary>
public readonly struct AddArtifactOptions
{
    /// <summary>
    /// Gets or sets the type of the artifact.
    /// The default value is <see langword="null"/>.
    /// The value can be present in <see cref="ArtifactTypes"/>, be a custom one, or just be <see langword="null"/>.
    /// </summary>
    public string? ArtifactType { get; init; }

    /// <summary>
    /// Gets or sets the artifact title.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public string? ArtifactTitle { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to prepend artifact number to file name in a form of "001-{file name}".
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool PrependArtifactNumberToFileName { get; init; }
}
