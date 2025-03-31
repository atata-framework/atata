#nullable enable

namespace Atata;

public interface ISupportsMetadata
{
    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    UIComponentMetadata Metadata { get; set; }

    /// <summary>
    /// Gets the type of the component.
    /// </summary>
    Type ComponentType { get; }
}
